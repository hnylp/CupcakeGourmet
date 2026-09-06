using CupcakeGourmet.Web.Models;
using CupcakeGourmet.Web.Models.Carrinho;
using CupcakeGourmet.Web.Services;
using Xunit;

namespace CupcakeGourmet.Tests;

public class PedidoFactoryTests
{
    private readonly PedidoFactory _factory = new();

    [Fact]
    public void CriarPedido_ComItensValidos_DeveCalcularValorTotalCorretamente()
    {
        var itens = new List<CarrinhoItem>
        {
            new() { ProdutoId = 1, Nome = "Baunilha", PrecoUnitario = 8.90m, Quantidade = 2 },
            new() { ProdutoId = 2, Nome = "Chocolate", PrecoUnitario = 9.50m, Quantidade = 1 }
        };

        var pedido = _factory.CriarPedido("cliente-1", enderecoEntregaId: 5, itens, FormaPagamento.Pix);

        Assert.Equal(27.30m, pedido.ValorTotal);
        Assert.Equal(2, pedido.Itens.Count);
        Assert.Equal(StatusPedido.Recebido, pedido.Status);
    }

    [Fact]
    public void CriarPedido_DeveGerarPagamentoAprovadoComValorIgualAoPedido()
    {
        var itens = new List<CarrinhoItem>
        {
            new() { ProdutoId = 1, Nome = "Baunilha", PrecoUnitario = 8.90m, Quantidade = 1 }
        };

        var pedido = _factory.CriarPedido("cliente-1", 5, itens, FormaPagamento.CartaoCredito);

        Assert.NotNull(pedido.Pagamento);
        Assert.Equal(StatusPagamento.Aprovado, pedido.Pagamento!.Status);
        Assert.Equal(pedido.ValorTotal, pedido.Pagamento.ValorPago);
        Assert.Equal(FormaPagamento.CartaoCredito, pedido.Pagamento.Forma);
    }

    [Fact]
    public void CriarPedido_CarrinhoVazio_DeveLancarExcecao()
    {
        Assert.Throws<InvalidOperationException>(() =>
            _factory.CriarPedido("cliente-1", 5, new List<CarrinhoItem>(), FormaPagamento.Pix));
    }

    [Fact]
    public void CriarPedido_DeveAssociarClienteEEnderecoInformados()
    {
        var itens = new List<CarrinhoItem>
        {
            new() { ProdutoId = 1, Nome = "Baunilha", PrecoUnitario = 8.90m, Quantidade = 1 }
        };

        var pedido = _factory.CriarPedido("cliente-42", enderecoEntregaId: 7, itens, FormaPagamento.DinheiroNaEntrega);

        Assert.Equal("cliente-42", pedido.ClienteId);
        Assert.Equal(7, pedido.EnderecoEntregaId);
    }

    [Fact]
    public void CriarPedido_DatasDevemSerUtc()
    {
        // PostgreSQL (Npgsql) rejeita DateTime com Kind=Local em colunas timestamp;
        // este teste evita a regressao para DateTime.Now que quebrava o checkout em producao.
        var itens = new List<CarrinhoItem>
        {
            new() { ProdutoId = 1, Nome = "Baunilha", PrecoUnitario = 8.90m, Quantidade = 1 }
        };

        var pedido = _factory.CriarPedido("cliente-1", 5, itens, FormaPagamento.Pix);

        Assert.Equal(DateTimeKind.Utc, pedido.DataPedido.Kind);
        Assert.Equal(DateTimeKind.Utc, pedido.Pagamento!.DataPagamento!.Value.Kind);
    }
}
