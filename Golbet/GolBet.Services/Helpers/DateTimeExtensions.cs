// GolBet.Services/Helpers/DateTimeExtensions.cs
namespace GolBet.Services.Helpers;

public static class DateTimeExtensions
{
    private static readonly TimeZoneInfo ColombiaZone =
        TimeZoneInfo.FindSystemTimeZoneById("America/Bogota");

    /// <summary>UTC (DB) -> Colombia local (form editing).</summary>
    public static DateTime ToColombiaTime(this DateTime utc)
        => TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.SpecifyKind(utc, DateTimeKind.Utc), ColombiaZone);

    /// <summary>Colombia local (form input) -> UTC (DB).</summary>
    public static DateTime ToUtcFromColombia(this DateTime colombiaLocal)
        => TimeZoneInfo.ConvertTimeToUtc(
            DateTime.SpecifyKind(colombiaLocal, DateTimeKind.Unspecified), ColombiaZone);
}