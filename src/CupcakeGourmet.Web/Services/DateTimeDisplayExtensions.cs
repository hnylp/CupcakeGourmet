namespace CupcakeGourmet.Web.Services;

public static class DateTimeDisplayExtensions
{
    public static DateTime ParaHorarioBrasilia(this DateTime utc) => utc.AddHours(-3);
}
