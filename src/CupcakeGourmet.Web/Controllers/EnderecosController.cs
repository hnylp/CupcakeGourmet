using CupcakeGourmet.Web.Data;
using CupcakeGourmet.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CupcakeGourmet.Web.Controllers;

[Authorize]
public class EnderecosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var clienteId = userManager.GetUserId(User)!;
        var enderecos = await context.Enderecos
            .Where(e => e.ClienteId == clienteId)
            .OrderBy(e => e.Apelido)
            .ToListAsync();
        return View(enderecos);
    }

    [HttpGet]
    public IActionResult Criar(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View(new Endereco());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Criar(Endereco endereco, string? returnUrl = null)
    {
        ModelState.Remove(nameof(Endereco.ClienteId));
        if (!ModelState.IsValid)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(endereco);
        }

        endereco.ClienteId = userManager.GetUserId(User)!;
        context.Enderecos.Add(endereco);
        await context.SaveChangesAsync();

        TempData["Sucesso"] = "Endereço cadastrado com sucesso.";

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Excluir(int id)
    {
        var clienteId = userManager.GetUserId(User)!;
        var endereco = await context.Enderecos.FirstOrDefaultAsync(e => e.Id == id && e.ClienteId == clienteId);
        if (endereco is not null)
        {
            context.Enderecos.Remove(endereco);
            await context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
