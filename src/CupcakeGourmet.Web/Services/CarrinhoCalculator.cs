using CupcakeGourmet.Web.Models.Carrinho;

namespace CupcakeGourmet.Web.Services;

public class CarrinhoCalculator
{
    public decimal CalcularTotal(IEnumerable<CarrinhoItem> itens) => itens.Sum(i => i.Subtotal);

    public int CalcularQuantidadeTotal(IEnumerable<CarrinhoItem> itens) => itens.Sum(i => i.Quantidade);

    public List<CarrinhoItem> AdicionarOuAtualizar(List<CarrinhoItem> itens, CarrinhoItem novoItem)
    {
        if (novoItem.Quantidade <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.", nameof(novoItem));

        var existente = itens.FirstOrDefault(i => i.ProdutoId == novoItem.ProdutoId);
        if (existente is null)
        {
            itens.Add(novoItem);
        }
        else
        {
            existente.Quantidade += novoItem.Quantidade;
        }

        return itens;
    }

    public List<CarrinhoItem> AtualizarQuantidade(List<CarrinhoItem> itens, int produtoId, int quantidade)
    {
        var existente = itens.FirstOrDefault(i => i.ProdutoId == produtoId);
        if (existente is null)
            return itens;

        if (quantidade <= 0)
            itens.Remove(existente);
        else
            existente.Quantidade = quantidade;

        return itens;
    }

    public List<CarrinhoItem> Remover(List<CarrinhoItem> itens, int produtoId)
    {
        itens.RemoveAll(i => i.ProdutoId == produtoId);
        return itens;
    }
}
