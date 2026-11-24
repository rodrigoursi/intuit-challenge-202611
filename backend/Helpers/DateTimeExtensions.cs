namespace ConsultasPlan.Helpers;

public static class DateTimeExtensions
{
    public static bool IsInCurrentYear(this DateTime fecha)
    {
        return fecha.Year == DateTime.Now.Year;
    }
}
