using System.Text.Json.Nodes;

namespace MyHomePage.Content;

/// <summary>
/// schema.org data (JSON-LD) of a page, built from the content: the site, the person and the page itself.
/// Search engines and AI assistants read it to understand who the site is about.
/// </summary>
public static class StructuredData
{
    private const string Context = "https://schema.org";

    public static string Build(ContentService content, string pageKey, string pageUrl)
    {
        var site = content.Site;
        var baseUrl = site.BaseUrl.TrimEnd('/');
        var personId = $"{baseUrl}/#person";
        var websiteId = $"{baseUrl}/#website";
        var meta = content.Page(pageKey);

        var website = new JsonObject
        {
            ["@type"] = "WebSite",
            ["@id"] = websiteId,
            ["url"] = $"{baseUrl}/",
            ["name"] = site.Name,
            ["inLanguage"] = new JsonArray(Languages.FrenchCulture, Languages.EnglishCulture),
            ["publisher"] = Reference(personId),
        };

        var page = new JsonObject
        {
            ["@type"] = PageType(pageKey),
            ["@id"] = pageUrl,
            ["url"] = pageUrl,
            ["name"] = content.T(meta.Title),
            ["description"] = content.T(meta.Description),
            ["inLanguage"] = Languages.Culture(content.Language).Name,
            ["isPartOf"] = Reference(websiteId),
            ["dateModified"] = content.Config.BuildTimestamp.ToString("yyyy-MM-dd"),
            [IsProfile(pageKey) ? "mainEntity" : "about"] = Reference(personId),
        };

        var graph = new JsonObject
        {
            ["@context"] = Context,
            ["@graph"] = new JsonArray(website, Person(content, personId, baseUrl), page),
        };

        // JSON-LD sits in a <script> element: "<" is escaped by the default encoder, so "</script>" cannot appear.
        return graph.ToJsonString();
    }

    private static JsonObject Person(ContentService content, string personId, string baseUrl)
    {
        var site = content.Site;
        var current = content.CurrentPosition;

        var person = new JsonObject
        {
            ["@type"] = "Person",
            ["@id"] = personId,
            ["name"] = site.Name,
            ["givenName"] = site.GivenName,
            ["familyName"] = site.FamilyName,
            ["description"] = content.T(content.Profile.Intro),
            ["url"] = baseUrl + content.Href(Constants.Pages.Home.Path),
            ["image"] = baseUrl + Constants.Files.ResumePortrait,
            ["email"] = $"mailto:{site.Email}",
            ["address"] = new JsonObject
            {
                ["@type"] = "PostalAddress",
                ["addressRegion"] = content.T(site.Location.Region),
                ["addressCountry"] = site.Location.Country,
            },
            ["knowsAbout"] = new JsonArray(content.Resume.Skills
                .SelectMany(group => group.Items)
                .Select(skill => (JsonNode)content.T(skill)!)
                .ToArray()),
            ["hasCredential"] = new JsonArray(content.Resume.Trainings
                .Select(training => (JsonNode)new JsonObject
                {
                    ["@type"] = "EducationalOccupationalCredential",
                    ["name"] = content.T(training.Label),
                    ["dateCreated"] = training.Year.ToString(),
                })
                .ToArray()),
            ["alumniOf"] = new JsonArray(content.Resume.Education
                .Where(education => education.School is not null)
                .Select(education => (JsonNode)Organization("EducationalOrganization", education.School!))
                .ToArray()),
            ["subjectOf"] = new JsonObject
            {
                ["@type"] = "DigitalDocument",
                ["name"] = content.Ui(Constants.Ui.Cta.Pdf),
                ["url"] = baseUrl + site.CvPdf,
                ["encodingFormat"] = "application/pdf",
            },
            ["sameAs"] = new JsonArray(site.Profiles.Select(url => (JsonNode)url!).ToArray()),
        };

        if (current is not null)
        {
            person["jobTitle"] = content.T(current.Role);
            person["worksFor"] = Organization("Organization", new Organization(current.Company, current.Url));
        }

        return person;
    }

    private static JsonObject Organization(string type, Organization organization)
    {
        var node = new JsonObject { ["@type"] = type, ["name"] = organization.Name };
        if (organization.Url is not null)
            node["url"] = organization.Url;
        return node;
    }

    private static JsonObject Reference(string id) => new() { ["@id"] = id };

    private static bool IsProfile(string pageKey) =>
        pageKey is Constants.Pages.Home.Key or Constants.Pages.Resume.Key;

    private static string PageType(string pageKey) => pageKey switch
    {
        Constants.Pages.Home.Key or Constants.Pages.Resume.Key => "ProfilePage",
        Constants.Pages.About.Key => "AboutPage",
        Constants.Pages.Contact.Key => "ContactPage",
        _ => "WebPage",
    };
}
