using CupcakeGourmet.Web.Data;
using CupcakeGourmet.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CupcakeGourmet.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = DbInitializer.PerfilAdministrador)]
public class ProdutosController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var produtos = await context.Produtos.Include(p => p.Categoria).OrderBy(p => p.Nome).ToListAsync();
        return View(produtos);
    }

    public async Task<IActionResult> Criar()
    {
        await CarregarCategoriasAsync();
        return View(new Produto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Criar(Produto produto)
    {
        if (!ModelState.IsValid)
        {
            await CarregarCategoriasAsync(produto.CategoriaId);
            return View(produto);
        }

        context.Produtos.Add(produto);
        await context.SaveChangesAsync();
        TempData["Sucesso"] = "Produto criado com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Editar(int id)
    {
        var produto = await context.Produtos.FindAsync(id);
        if (produto is null) return NotFound();
        await CarregarCategoriasAsync(produto.CategoriaId);
        return View(produto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, Produto produto)
    {
        if (id != produto.Id) return NotFound();
        if (!ModelState.IsValid)
        {
            await CarregarCategoriasAsync(produto.CategoriaId);
            return View(produto);
        }

        context.Update(produto);
        await context.SaveChangesAsync();
        TempData["Sucesso"] = "Produto atualizado com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AlternarAtivo(int id)
    {
        var produto = await context.Produtos.FindAsync(id);
        if (produto is null) return NotFound();

        produto.Ativo = !produto.Ativo;
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task CarregarCategoriasAsync(int? categoriaSelecionada = null)
    {
        var categorias = await context.Categorias.OrderBy(c => c.Nome).ToListAsync();
        ViewBag.Categorias = new SelectList(categorias, "Id", "Nome", categoriaSelecionada);
    }
}
