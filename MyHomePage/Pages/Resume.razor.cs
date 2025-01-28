using Microsoft.AspNetCore.Components;

namespace MyHomePage.Pages;

[Route("resume")]
[Route("cv")]
[Route("curriculumvitae")]
public partial class Resume
{
    private IQueryable<Competences> _competences = new[]
    {
        new Competences("Compétences techniques",["AzureDevOps", "Blazor (WASM)", "Blazor (Server)", "ASP.NET", "ASP.NET MVC", "Entity Framework Core"]),
        new ("Langages de développement",["C#","T-SQL", "SQL ISO", "Visual Basic", "Powershell", "HTML","CSS", "Typescript","Javascript"]),
        new ("Solutions de développement",["Visual Studio", "Visual Studio Code", "SQL Server Management Studio", "SQL Profiler","Azure Data Studio", "Postman", "Fiddler", "Git", "TFS"]),
        new ("Compétences fonctionnelles",["Agile","Scrum","Kanban","Gestion de projet", "Qualité", "Architecture", "Documentation",  "Relation client", "Rédaction", "Communication"]),
        new ("Compétences métier",["Administration publique","Edition de logiciels","Gestion d'entreprise","Contenu marketing","Service"])
    }.AsQueryable();
}

public sealed record Competences(string Title, string[] Items);