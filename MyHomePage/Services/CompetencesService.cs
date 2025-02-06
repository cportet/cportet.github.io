using System.Globalization;
using MyHomePage.Code;
using MyHomePage.Extensions;

namespace MyHomePage.Services;

public class CompetencesService(HttpClient http, AppConfig appConfig)
{
    private const string DefaultCompetenceFileName = "competences";

    public async Task<List<Competence>> LoadCompetences(string? culture = null)
    {
        var currentCulture = culture ?? CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        var result = await http.LoadJsonAsync<Competence[]>(appConfig.VersionUrlString, DefaultCompetenceFileName, currentCulture);
        return result?.ToList() ?? [];
    }
}
