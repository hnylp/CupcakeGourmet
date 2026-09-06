namespace CupcakeGourmet.Web.Models.Carrinho;

public class CarrinhoItem
{
    public int ProdutoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal PrecoUnitario { get; set; }
    public string? ImagemUrl { get; set; }
    public int Quantidade { get; set; }

    public decimal Subtotal => PrecoUnitario * Quantidade;
}
