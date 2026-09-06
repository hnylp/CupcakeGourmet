using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CupcakeGourmet.Web.Models;

public class Pedido
{
    public int Id { get; set; }

    [Required]
    public string ClienteId { get; set; } = string.Empty;
    public ApplicationUser? Cliente { get; set; }

    [Required]
    public int EnderecoEntregaId { get; set; }
    public Endereco? EnderecoEntrega { get; set; }

    public DateTime DataPedido { get; set; } = DateTime.Now;

    public StatusPedido Status { get; set; } = StatusPedido.Recebido;

    [Column(TypeName = "decimal(10,2)")]
    public decimal ValorTotal { get; set; }

    public ICollection<ItemPedido> Itens { get; set; } = new List<ItemPedido>();

    public Pagamento? Pagamento { get; set; }
}
