using System.ComponentModel.DataAnnotations;
using CupcakeGourmet.Web.Models.Carrinho;

namespace CupcakeGourmet.Web.Models.ViewModels;

public class CheckoutViewModel
{
    public List<CarrinhoItem> Itens { get; set; } = new();
    public decimal ValorTotal { get; set; }

    [Required(ErrorMessage = "Selecione o endereço de entrega.")]
    [Display(Name = "Endereço de entrega")]
    public int EnderecoEntregaId { get; set; }

    public List<Endereco> EnderecosDisponiveis { get; set; } = new();

    [Required(ErrorMessage = "Selecione a forma de pagamento.")]
    [Display(Name = "Forma de pagamento")]
    public FormaPagamento FormaPagamento { get; set; }
}
