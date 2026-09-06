using CupcakeGourmet.Web.Models;
using CupcakeGourmet.Web.Models.Carrinho;

namespace CupcakeGourmet.Web.Services;

public class PedidoFactory
{
    public Pedido CriarPedido(string clienteId, int enderecoEntregaId, List<CarrinhoItem> itensCarrinho, FormaPagamento formaPagamento)
    {
        if (itensCarrinho is null || itensCarrinho.Count == 0)
            throw new InvalidOperationException("Não é possível criar um pedido com o carrinho vazio.");

        var pedido = new Pedido
        {
            ClienteId = clienteId,
            EnderecoEntregaId = enderecoEntregaId,
            DataPedido = DateTime.UtcNow,
            Status = StatusPedido.Recebido,
            Itens = itensCarrinho.Select(i => new ItemPedido
            {
                ProdutoId = i.ProdutoId,
                Quantidade = i.Quantidade,
                PrecoUnitario = i.PrecoUnitario
            }).ToList()
        };

        pedido.ValorTotal = pedido.Itens.Sum(i => i.Subtotal);

        pedido.Pagamento = new Pagamento
        {
            Forma = formaPagamento,
            ValorPago = pedido.ValorTotal,
            Status = StatusPagamento.Aprovado,
            DataPagamento = DateTime.UtcNow
        };

        return pedido;
    }
}
