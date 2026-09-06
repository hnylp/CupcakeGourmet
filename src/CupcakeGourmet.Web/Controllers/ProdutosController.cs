using CupcakeGourmet.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CupcakeGourmet.Web.Controllers;

public class ProdutosController(ApplicationDbContext context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(int? categoriaId, string? busca)
    {
        var produtosQuery = context.Produtos
            .Include(p => p.Categoria)
            .Where(p => p.Ativo)
            .AsQueryable();

        if (categoriaId.HasValue)
            produtosQuery = produtosQuery.Where(p => p.CategoriaId == categoriaId.Value);

        if (!string.IsNullOrWhiteSpace(busca))
            produtosQuery = produtosQuery.Where(p => p.Nome.Contains(busca));

        ViewBag.Categorias = await context.Categorias.OrderBy(c => c.Nome).ToListAsync();
        ViewBag.CategoriaSelecionada = categoriaId;
        ViewBag.Busca = busca;

        var produtos = await produtosQuery.OrderBy(p => p.Nome).ToListAsync();
        return View(produtos);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var produto = await context.Produtos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id && p.Ativo);

        if (produto is null)
            return NotFound();

        return View(produto);
    }
}
