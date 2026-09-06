using System.Diagnostics;
using CupcakeGourmet.Web.Data;
using Microsoft.AspNetCore.Mvc;
using CupcakeGourmet.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CupcakeGourmet.Web.Controllers;

public class HomeController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var destaques = await context.Produtos
            .Include(p => p.Categoria)
            .Where(p => p.Ativo)
            .OrderByDescending(p => p.Id)
            .Take(6)
            .ToListAsync();

        return View(destaques);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
