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
        var from = start.ToString(Constants.Formats.MonthYear, culture);
        var to = end?.ToString(Constants.Formats.MonthYear, culture) ?? presentLabel;
        return $"{from} — {to}";
    }

    /// <summary>Replaces the live tokens ({{age}}, {{experienceYears}}…) of a text.</summary>
    public static string Expand(string text, Profile profile, IReadOnlyList<Experience> experiences, string language, DateOnly today)
    {
        if (!text.Contains("{{", StringComparison.Ordinal)) // Every token starts with "{{".
            return text;

        var current = experiences.FirstOrDefault(e => e.IsCurrent);

        return text
            .Replace(Constants.Tokens.Age, YearsBetween(profile.BirthDate, today).ToString(), StringComparison.Ordinal)
            .Replace(Constants.Tokens.ExperienceYears, YearsBetween(profile.CareerStart, today).ToString(), StringComparison.Ordinal)
            .Replace(Constants.Tokens.ManagementYears, YearsBetween(profile.ManagementStart, today).ToString(), StringComparison.Ordinal)
            .Replace(Constants.Tokens.CurrentRoleDuration, current is null ? string.Empty : Duration(current.Start, current.End, today, language), StringComparison.Ordinal)
            .Replace(Constants.Tokens.Year, today.Year.ToString(), StringComparison.Ordinal);
    }

    public static string PeriodYears(DateOnly start, DateOnly? end, string presentLabel) =>
        end is null || end.Value.Year != start.Year
            ? $"{start.Year} — {end?.Year.ToString() ?? presentLabel}"
            : $"{start.Year}";
}
