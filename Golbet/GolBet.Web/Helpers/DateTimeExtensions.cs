// GolBet.Web/Helpers/DateTimeExtensions.cs
namespace GolBet.Web.Helpers;

public static class DateTimeExtensions
{
    private static readonly TimeZoneInfo ColombiaZone =
        TimeZoneInfo.FindSystemTimeZoneById("America/Bogota");

    /// <summary>Convierte una fecha UTC a hora local de Colombia (UTC-5).</summary>
    public static DateTime ToColombiaTime(this DateTime utcDate)
        => TimeZoneInfo.ConvertTimeFromUtc(utcDate, ColombiaZone);
}