using System.ComponentModel.DataAnnotations;

namespace CupcakeGourmet.Web.Models;

public class Endereco
{
    public int Id { get; set; }

    [Required]
    public string ClienteId { get; set; } = string.Empty;
    public ApplicationUser? Cliente { get; set; }

    [Required(ErrorMessage = "Informe o apelido do endereço (ex.: Casa, Trabalho).")]
    [StringLength(40)]
    [Display(Name = "Apelido")]
    public string Apelido { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o CEP.")]
    [StringLength(9)]
    public string Cep { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o logradouro.")]
    [StringLength(120)]
    public string Logradouro { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o número.")]
    [StringLength(10)]
    public string Numero { get; set; } = string.Empty;

    [StringLength(60)]
    public string? Complemento { get; set; }

    [Required(ErrorMessage = "Informe o bairro.")]
    [StringLength(60)]
    public string Bairro { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe a cidade.")]
    [StringLength(60)]
    public string Cidade { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o estado (UF).")]
    [StringLength(2)]
    public string Uf { get; set; } = string.Empty;

    public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
