using Microsoft.AspNetCore.Identity;

namespace CupcakeGourmet.Web.Models;

public class ApplicationUser : IdentityUser
{
    public string NomeCompleto { get; set; } = string.Empty;

    public ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();
    public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
