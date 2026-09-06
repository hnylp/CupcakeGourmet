using CupcakeGourmet.Web.Data;
using CupcakeGourmet.Web.Models;
using CupcakeGourmet.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CupcakeGourmet.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = DbInitializer.PerfilAdministrador)]
public class PedidosController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index(StatusPedido? status)
    {
        var query = context.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.Pagamento)
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(p => p.Status == status.Value);

        ViewBag.StatusSelecionado = status;
        var pedidos = await query.OrderByDescending(p => p.DataPedido).ToListAsync();
        return View(pedidos);
    }

    public async Task<IActionResult> Details(int id)
    {
        var pedido = await context.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.EnderecoEntrega)
            .Include(p => p.Pagamento)
            .Include(p => p.Itens).ThenInclude(i => i.Produto)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pedido is null) return NotFound();
        return View(pedido);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AtualizarStatus(int id, StatusPedido novoStatus)
    {
        var pedido = await context.Pedidos.FindAsync(id);
        if (pedido is null) return NotFound();

        pedido.Status = novoStatus;
        await context.SaveChangesAsync();
        TempData["Sucesso"] = $"Status do pedido #{id} atualizado para {novoStatus.GetDisplayName()}.";
        return RedirectToAction(nameof(Details), new { id });
    }
}
