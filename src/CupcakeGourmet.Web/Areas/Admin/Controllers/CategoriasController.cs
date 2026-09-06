using CupcakeGourmet.Web.Data;
using CupcakeGourmet.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CupcakeGourmet.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = DbInitializer.PerfilAdministrador)]
public class CategoriasController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await context.Categorias.OrderBy(c => c.Nome).ToListAsync());
    }

    public IActionResult Criar() => View(new Categoria());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Criar(Categoria categoria)
    {
        if (!ModelState.IsValid) return View(categoria);
        context.Categorias.Add(categoria);
        await context.SaveChangesAsync();
        TempData["Sucesso"] = "Categoria criada com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Editar(int id)
    {
        var categoria = await context.Categorias.FindAsync(id);
        if (categoria is null) return NotFound();
        return View(categoria);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Categoria categoria)
    {
        if (id != categoria.Id) return NotFound();
        if (!ModelState.IsValid) return View(categoria);

        context.Update(categoria);
        await context.SaveChangesAsync();
        TempData["Sucesso"] = "Categoria atualizada com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Excluir(int id)
    {
        var categoria = await context.Categorias.Include(c => c.Produtos).FirstOrDefaultAsync(c => c.Id == id);
        if (categoria is null) return NotFound();

        if (categoria.Produtos.Count > 0)
        {
            TempData["Erro"] = "Não é possível excluir uma categoria que possui produtos vinculados.";
            return RedirectToAction(nameof(Index));
        }

        context.Categorias.Remove(categoria);
        await context.SaveChangesAsync();
        TempData["Sucesso"] = "Categoria excluída com sucesso.";
        return RedirectToAction(nameof(Index));
    }
}
