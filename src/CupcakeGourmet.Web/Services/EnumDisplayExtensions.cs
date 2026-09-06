using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CupcakeGourmet.Web.Services;

public static class EnumDisplayExtensions
{
    public static string GetDisplayName(this Enum valor)
    {
        var membro = valor.GetType().GetMember(valor.ToString()).FirstOrDefault();
        var atributo = membro?.GetCustomAttribute<DisplayAttribute>();
        return atributo?.Name ?? valor.ToString();
    }
}
