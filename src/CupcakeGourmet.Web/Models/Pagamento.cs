using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CupcakeGourmet.Web.Models;

public enum FormaPagamento
{
    [Display(Name = "Cartão de crédito")]
    CartaoCredito = 0,

    [Display(Name = "Pix")]
    Pix = 1,

    [Display(Name = "Dinheiro na entrega")]
    DinheiroNaEntrega = 2
}

public enum StatusPagamento
{
    Pendente = 0,
    Aprovado = 1,
    Recusado = 2
}

public class Pagamento
{
    public int Id { get; set; }

    public int PedidoId { get; set; }
    public Pedido? Pedido { get; set; }

    [Display(Name = "Forma de pagamento")]
    public FormaPagamento Forma { get; set; }

    public StatusPagamento Status { get; set; } = StatusPagamento.Pendente;

    [Column(TypeName = "decimal(10,2)")]
    public decimal ValorPago { get; set; }

    public DateTime? DataPagamento { get; set; }
}
