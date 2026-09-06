using CupcakeGourmet.Web.Data;
using CupcakeGourmet.Web.Models;
using CupcakeGourmet.Web.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CupcakeGourmet.Web.Controllers;

public class ContaController(
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager,
    ILogger<ContaController> logger) : Controller
{
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var resultado = await signInManager.PasswordSignInAsync(model.Email, model.Senha, model.LembrarMe, lockoutOnFailure: true);

        if (resultado.Succeeded)
        {
            logger.LogInformation("Usuário {Email} autenticado.", model.Email);
            return RedirectToLocalOrHome(model.ReturnUrl);
        }

        if (resultado.IsLockedOut)
        {
            ModelState.AddModelError(string.Empty, "Conta bloqueada temporariamente por várias tentativas inválidas. Tente novamente mais tarde.");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos.");
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Registrar()
    {
        return View(new RegistroViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Registrar(RegistroViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var usuario = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            NomeCompleto = model.NomeCompleto,
            EmailConfirmed = true
        };

        var resultado = await userManager.CreateAsync(usuario, model.Senha);
        if (resultado.Succeeded)
        {
            await userManager.AddToRoleAsync(usuario, DbInitializer.PerfilCliente);
            await signInManager.SignInAsync(usuario, isPersistent: false);
            logger.LogInformation("Novo cliente registrado: {Email}", model.Email);
            return RedirectToAction("Index", "Home");
        }

        foreach (var erro in resultado.Errors)
            ModelState.AddModelError(string.Empty, TraduzirErroIdentity(erro));

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Sair()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult AcessoNegado()
    {
        return View();
    }

    private IActionResult RedirectToLocalOrHome(string? returnUrl)
    {
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Home");
    }

    private static string TraduzirErroIdentity(IdentityError erro) => erro.Code switch
    {
        "DuplicateUserName" or "DuplicateEmail" => "Já existe uma conta cadastrada com este e-mail.",
        "PasswordTooShort" => "A senha é muito curta.",
        "PasswordRequiresNonAlphanumeric" => "A senha deve conter ao menos um caractere especial.",
        "PasswordRequiresDigit" => "A senha deve conter ao menos um número.",
        "PasswordRequiresUpper" => "A senha deve conter ao menos uma letra maiúscula.",
        _ => erro.Description
    };
}
