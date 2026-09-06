using System.ComponentModel.DataAnnotations.Schema;

namespace CupcakeGourmet.Web.Models;

public class ItemPedido
{
    public int Id { get; set; }

    public int PedidoId { get; set; }
    public Pedido? Pedido { get; set; }

    public int ProdutoId { get; set; }
    public Produto? Produto { get; set; }

    public int Quantidade { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal PrecoUnitario { get; set; }

    [NotMapped]
    public decimal Subtotal => Quantidade * PrecoUnitario;
}
