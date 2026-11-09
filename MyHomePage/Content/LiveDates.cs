namespace MyHomePage.Content;

/// <summary>
/// Date computations that stay "alive": ages, seniority and durations are always
/// computed from the dates stored in the content, relative to today.
/// </summary>
public static class LiveDates
{
    public static int YearsBetween(DateOnly from, DateOnly to)
    {
        var years = to.Year - from.Year;
        if (to < from.AddYears(years))
            years--;
        return Math.Max(0, years);
    }

    /// <summary>Number of months covered by a position, the end month being included.</summary>
    public static int MonthsCovered(DateOnly start, DateOnly? end, DateOnly today)
    {
        var last = end ?? today;
        var months = (last.Year - start.Year) * 12 + last.Month - start.Month;
        return Math.Max(1, end is null ? months : months + 1);
    }

    public static string Duration(DateOnly start, DateOnly? end, DateOnly today, string language)
    {
        var months = MonthsCovered(start, end, today);
        var english = language == Languages.English;

        if (months < 12)
            return english
                ? $"{months} month{(months > 1 ? "s" : "")}"
                : $"{months} mois";

        var years = months / 12;
        return english
            ? $"{years} year{(years > 1 ? "s" : "")}"
            : $"{years} an{(years > 1 ? "s" : "")}";
    }

    public static string Period(DateOnly start, DateOnly? end, string language, string presentLabel)
    {
        var culture = Languages.Culture(language);
        var from = start.ToString("MMMM yyyy", culture);
        var to = end?.ToString("MMMM yyyy", culture) ?? presentLabel;
        return $"{from} — {to}";
    }

    public static string PeriodYears(DateOnly start, DateOnly? end, string presentLabel) =>
        end is null || end.Value.Year != start.Year
            ? $"{start.Year} — {end?.Year.ToString() ?? presentLabel}"
            : $"{start.Year}";
}
