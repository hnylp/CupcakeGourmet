using System.ComponentModel.DataAnnotations;

namespace CupcakeGourmet.Web.Models;

public enum StatusPedido
{
    [Display(Name = "Recebido")]
    Recebido = 0,

    [Display(Name = "Em preparo")]
    EmPreparo = 1,

    [Display(Name = "Saiu para entrega")]
    SaiuParaEntrega = 2,

    [Display(Name = "Entregue")]
    Entregue = 3,

    [Display(Name = "Cancelado")]
    Cancelado = 4
}
