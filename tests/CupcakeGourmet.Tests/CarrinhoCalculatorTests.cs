using CupcakeGourmet.Web.Models.Carrinho;
using CupcakeGourmet.Web.Services;
using Xunit;

namespace CupcakeGourmet.Tests;

public class CarrinhoCalculatorTests
{
    private readonly CarrinhoCalculator _calculadora = new();

    [Fact]
    public void CalcularTotal_DeveSomarSubtotalDeTodosOsItens()
    {
        var itens = new List<CarrinhoItem>
        {
            new() { ProdutoId = 1, Nome = "Baunilha", PrecoUnitario = 8.90m, Quantidade = 2 },
            new() { ProdutoId = 2, Nome = "Chocolate", PrecoUnitario = 9.50m, Quantidade = 1 }
        };

        var total = _calculadora.CalcularTotal(itens);

        Assert.Equal(27.30m, total);
    }

    [Fact]
    public void CalcularTotal_CarrinhoVazio_DeveRetornarZero()
    {
        var total = _calculadora.CalcularTotal(new List<CarrinhoItem>());

        Assert.Equal(0m, total);
    }

    [Fact]
    public void AdicionarOuAtualizar_ProdutoNovo_DeveAdicionarUmItem()
    {
        var itens = new List<CarrinhoItem>();

        _calculadora.AdicionarOuAtualizar(itens, new CarrinhoItem { ProdutoId = 1, Nome = "Baunilha", PrecoUnitario = 8.90m, Quantidade = 2 });

        var item = Assert.Single(itens);
        Assert.Equal(2, item.Quantidade);
    }

    [Fact]
    public void AdicionarOuAtualizar_ProdutoJaNoCarrinho_DeveSomarQuantidade()
    {
        var itens = new List<CarrinhoItem>
        {
            new() { ProdutoId = 1, Nome = "Baunilha", PrecoUnitario = 8.90m, Quantidade = 1 }
        };

        _calculadora.AdicionarOuAtualizar(itens, new CarrinhoItem { ProdutoId = 1, Nome = "Baunilha", PrecoUnitario = 8.90m, Quantidade = 3 });

        var item = Assert.Single(itens);
        Assert.Equal(4, item.Quantidade);
    }

    [Fact]
    public void AdicionarOuAtualizar_QuantidadeZeroOuNegativa_DeveLancarExcecao()
    {
        var itens = new List<CarrinhoItem>();

        Assert.Throws<ArgumentException>(() =>
            _calculadora.AdicionarOuAtualizar(itens, new CarrinhoItem { ProdutoId = 1, Quantidade = 0 }));
    }

    [Fact]
    public void AtualizarQuantidade_ParaZero_DeveRemoverItem()
    {
        var itens = new List<CarrinhoItem>
        {
            new() { ProdutoId = 1, Nome = "Baunilha", PrecoUnitario = 8.90m, Quantidade = 2 }
        };

        _calculadora.AtualizarQuantidade(itens, 1, 0);

        Assert.Empty(itens);
    }

    [Fact]
    public void Remover_DeveRemoverApenasOProdutoInformado()
    {
        var itens = new List<CarrinhoItem>
        {
            new() { ProdutoId = 1, Nome = "Baunilha", Quantidade = 1 },
            new() { ProdutoId = 2, Nome = "Chocolate", Quantidade = 1 }
        };

        _calculadora.Remover(itens, 1);

        var restante = Assert.Single(itens);
        Assert.Equal(2, restante.ProdutoId);
    }
}
