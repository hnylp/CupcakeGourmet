using CupcakeGourmet.Web.Data;
using CupcakeGourmet.Web.Models.Carrinho;
using CupcakeGourmet.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace CupcakeGourmet.Web.Controllers;

public class CarrinhoController(ApplicationDbContext context, CarrinhoCalculator calculadora) : Controller
{
    private const string ChaveSessaoCarrinho = "Carrinho";

    [HttpGet]
    public IActionResult Index()
    {
        var itens = ObterCarrinho();
        ViewBag.ValorTotal = calculadora.CalcularTotal(itens);
        return View(itens);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Adicionar(int produtoId, int quantidade = 1)
    {
        var produto = await context.Produtos.FindAsync(produtoId);
        if (produto is null || !produto.Ativo)
        {
            TempData["Erro"] = "Produto não encontrado.";
            return RedirectToAction("Index", "Produtos");
        }

        if (quantidade < 1) quantidade = 1;

        var itens = ObterCarrinho();
        calculadora.AdicionarOuAtualizar(itens, new CarrinhoItem
        {
            ProdutoId = produto.Id,
            Nome = produto.Nome,
            PrecoUnitario = produto.Preco,
            ImagemUrl = produto.ImagemUrl,
            Quantidade = quantidade
        });
        SalvarCarrinho(itens);

        TempData["Sucesso"] = $"{produto.Nome} adicionado ao carrinho.";
        return RedirectToAction("Index", "Produtos");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AtualizarQuantidade(int produtoId, int quantidade)
    {
        var itens = ObterCarrinho();
        calculadora.AtualizarQuantidade(itens, produtoId, quantidade);
        SalvarCarrinho(itens);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Remover(int produtoId)
    {
        var itens = ObterCarrinho();
        calculadora.Remover(itens, produtoId);
        SalvarCarrinho(itens);
        return RedirectToAction(nameof(Index));
    }

    private List<CarrinhoItem> ObterCarrinho()
    {
        return HttpContext.Session.GetObject<List<CarrinhoItem>>(ChaveSessaoCarrinho) ?? new List<CarrinhoItem>();
    }

    private void SalvarCarrinho(List<CarrinhoItem> itens)
    {
        HttpContext.Session.SetObject(ChaveSessaoCarrinho, itens);
    }
}
