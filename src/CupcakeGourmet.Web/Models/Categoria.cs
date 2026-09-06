using System.ComponentModel.DataAnnotations;

namespace CupcakeGourmet.Web.Models;

public class Categoria
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Informe o nome da categoria.")]
    [StringLength(60)]
    [Display(Name = "Nome")]
    public string Nome { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Descricao { get; set; }

    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}
