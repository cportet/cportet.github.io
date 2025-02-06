using MyHomePage.Code;
using MyHomePage.Services;

namespace MyHomePage.Components;

public partial class MyCompetences(CompetencesService competencesService)
{
    private IQueryable<Competence>? _competences;

    protected override async Task OnInitializedAsync()
    {
        var competences = await competencesService.LoadCompetences();
        _competences = competences.AsQueryable()!;

        await base.OnInitializedAsync();
    }
}
