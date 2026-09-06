using CupcakeGourmet.Web.Models;
using CupcakeGourmet.Web.Services;
using Xunit;

namespace CupcakeGourmet.Tests;

public class EnumDisplayExtensionsTests
{
    [Theory]
    [InlineData(StatusPedido.Recebido, "Recebido")]
    [InlineData(StatusPedido.EmPreparo, "Em preparo")]
    [InlineData(StatusPedido.SaiuParaEntrega, "Saiu para entrega")]
    [InlineData(StatusPedido.Entregue, "Entregue")]
    [InlineData(StatusPedido.Cancelado, "Cancelado")]
    public void GetDisplayName_StatusPedido_DeveRetornarNomeAmigavel(StatusPedido status, string esperado)
    {
        Assert.Equal(esperado, status.GetDisplayName());
    }

    [Theory]
    [InlineData(FormaPagamento.CartaoCredito, "Cartão de crédito")]
    [InlineData(FormaPagamento.Pix, "Pix")]
    [InlineData(FormaPagamento.DinheiroNaEntrega, "Dinheiro na entrega")]
    public void GetDisplayName_FormaPagamento_DeveRetornarNomeAmigavel(FormaPagamento forma, string esperado)
    {
        Assert.Equal(esperado, forma.GetDisplayName());
    }
}
