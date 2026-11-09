using Xunit;
using MyHomePage.Content;

namespace MyHomePage.Tests;

public class LiveDatesTests
{
    [Theory]
    [InlineData("1979-11-07", "2026-11-06", 46)]
    [InlineData("1979-11-07", "2026-11-07", 47)]
    [InlineData("1998-09-01", "2026-11-09", 28)]
    public void YearsBetween_counts_full_years(string from, string to, int expected) =>
        Assert.Equal(expected, LiveDates.YearsBetween(DateOnly.Parse(from), DateOnly.Parse(to)));

    [Theory]
    // Current position: counted up to today.
    [InlineData("2021-05-01", null, "fr", "5 ans")]
    [InlineData("2026-06-01", null, "fr", "5 mois")]
    [InlineData("2021-05-01", null, "en", "5 years")]
    // Ended positions: the last month is included.
    [InlineData("2020-06-01", "2021-05-01", "fr", "1 an")]
    [InlineData("2020-06-01", "2021-05-01", "en", "1 year")]
    [InlineData("2013-06-01", "2014-04-01", "fr", "11 mois")]
    [InlineData("2014-04-01", "2020-04-01", "en", "6 years")]
    public void Duration_is_human_readable(string start, string? end, string language, string expected)
    {
        var today = new DateOnly(2026, 11, 9);
        var endDate = end is null ? (DateOnly?)null : DateOnly.Parse(end);

        Assert.Equal(expected, LiveDates.Duration(DateOnly.Parse(start), endDate, today, language));
    }

    [Fact]
    public void Period_uses_month_names_of_the_language()
    {
        var start = new DateOnly(2021, 5, 1);

        Assert.Equal("mai 2021 — aujourd’hui", LiveDates.Period(start, null, "fr", "aujourd’hui"));
        Assert.Equal("May 2021 — present", LiveDates.Period(start, null, "en", "present"));
    }
}
