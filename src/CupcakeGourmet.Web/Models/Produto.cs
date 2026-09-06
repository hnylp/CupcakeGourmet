using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CupcakeGourmet.Web.Models;

public class Produto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe o nome do produto.")]
    [StringLength(80)]
    public string Nome { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Descricao { get; set; }

    [Required]
    [Column(TypeName = "decimal(10,2)")]
    [Range(0.01, 9999.99, ErrorMessage = "Informe um preço válido.")]
    [Display(Name = "Preço")]
    public decimal Preco { get; set; }

    [Display(Name = "Imagem (URL)")]
    [StringLength(300)]
    public string? ImagemUrl { get; set; }

    [Range(0, 100000)]
    [Display(Name = "Estoque")]
    public int QuantidadeEmEstoque { get; set; }

    public bool Ativo { get; set; } = true;

    [Required]
    [Display(Name = "Categoria")]
    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }

    public ICollection<ItemPedido> ItensPedido { get; set; } = new List<ItemPedido>();
}
