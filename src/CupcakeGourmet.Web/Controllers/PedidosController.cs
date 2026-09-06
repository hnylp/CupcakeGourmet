using CupcakeGourmet.Web.Data;
using CupcakeGourmet.Web.Models;
using CupcakeGourmet.Web.Models.Carrinho;
using CupcakeGourmet.Web.Models.ViewModels;
using CupcakeGourmet.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CupcakeGourmet.Web.Controllers;

[Authorize]
public class PedidosController(
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    CarrinhoCalculator calculadora,
    PedidoFactory pedidoFactory) : Controller
{
    private const string ChaveSessaoCarrinho = "Carrinho";

    [HttpGet]
    public async Task<IActionResult> Checkout()
    {
        var itens = HttpContext.Session.GetObject<List<CarrinhoItem>>(ChaveSessaoCarrinho) ?? new List<CarrinhoItem>();
        if (itens.Count == 0)
        {
            TempData["Erro"] = "Seu carrinho está vazio.";
            return RedirectToAction("Index", "Carrinho");
        }

        var clienteId = userManager.GetUserId(User)!;
        var enderecos = await context.Enderecos.Where(e => e.ClienteId == clienteId).ToListAsync();

        var modelo = new CheckoutViewModel
        {
            Itens = itens,
            ValorTotal = calculadora.CalcularTotal(itens),
            EnderecosDisponiveis = enderecos
        };
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(CheckoutViewModel modelo)
    {
        var itens = HttpContext.Session.GetObject<List<CarrinhoItem>>(ChaveSessaoCarrinho) ?? new List<CarrinhoItem>();
        if (itens.Count == 0)
        {
            TempData["Erro"] = "Seu carrinho está vazio.";
            return RedirectToAction("Index", "Carrinho");
        }

        var clienteId = userManager.GetUserId(User)!;
        var enderecoValido = await context.Enderecos.AnyAsync(e => e.Id == modelo.EnderecoEntregaId && e.ClienteId == clienteId);
        if (!enderecoValido)
            ModelState.AddModelError(nameof(modelo.EnderecoEntregaId), "Selecione um endereço de entrega válido.");

        if (!ModelState.IsValid)
        {
            modelo.Itens = itens;
            modelo.ValorTotal = calculadora.CalcularTotal(itens);
            modelo.EnderecosDisponiveis = await context.Enderecos.Where(e => e.ClienteId == clienteId).ToListAsync();
            return View(modelo);
        }

        var pedido = pedidoFactory.CriarPedido(clienteId, modelo.EnderecoEntregaId, itens, modelo.FormaPagamento);
        context.Pedidos.Add(pedido);
        await context.SaveChangesAsync();

        HttpContext.Session.Remove(ChaveSessaoCarrinho);

        TempData["Sucesso"] = "Pedido realizado com sucesso!";
        return RedirectToAction(nameof(Confirmacao), new { id = pedido.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Confirmacao(int id)
    {
        var pedido = await ObterPedidoDoClienteAsync(id);
        if (pedido is null) return NotFound();
        return View(pedido);
    }

    [HttpGet]
    public async Task<IActionResult> MeusPedidos()
    {
        var clienteId = userManager.GetUserId(User)!;
        var pedidos = await context.Pedidos
            .Include(p => p.Pagamento)
            .Where(p => p.ClienteId == clienteId)
            .OrderByDescending(p => p.DataPedido)
            .ToListAsync();
        return View(pedidos);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var pedido = await ObterPedidoDoClienteAsync(id);
        if (pedido is null) return NotFound();
        return View(pedido);
    }

    private async Task<Pedido?> ObterPedidoDoClienteAsync(int id)
    {
        var clienteId = userManager.GetUserId(User)!;
        return await context.Pedidos
            .Include(p => p.Itens).ThenInclude(i => i.Produto)
            .Include(p => p.EnderecoEntrega)
            .Include(p => p.Pagamento)
            .FirstOrDefaultAsync(p => p.Id == id && p.ClienteId == clienteId);
    }
}
