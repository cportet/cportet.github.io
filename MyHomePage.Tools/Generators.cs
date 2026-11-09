using System.Text;
using System.Xml.Linq;
using MyHomePage.Content;

namespace MyHomePage.Tools;

/// <summary>Files for search engines and AI assistants, generated from the content.</summary>
internal static class Generators
{
    private static readonly XNamespace SitemapNs = "http://www.sitemaps.org/schemas/sitemap/0.9";
    private static readonly XNamespace XhtmlNs = "http://www.w3.org/1999/xhtml";

    /// <summary>sitemap.xml: every public page in both languages, with its alternates.</summary>
    public static string Sitemap(SiteContent content, DateOnly lastModified)
    {
        var lastmod = lastModified.ToString("yyyy-MM-dd");
        var urls = new List<XElement>();

        foreach (var (key, path) in Constants.Pages.Indexed)
        {
            var (priority, frequency) = key switch
            {
                Constants.Pages.Home.Key => ("1.0", "monthly"),
                Constants.Pages.Resume.Key => ("0.9", "monthly"),
                Constants.Pages.Contact.Key => ("0.7", "yearly"),
                Constants.Pages.Projects.Key or Constants.Pages.Perso.Key => ("0.6", "monthly"),
                _ => ("0.4", "yearly"),
            };

            foreach (var language in Languages.Supported)
            {
                urls.Add(new XElement(SitemapNs + "url",
                    new XElement(SitemapNs + "loc", content.Url(path, language)),
                    new XElement(SitemapNs + "lastmod", lastmod),
                    new XElement(SitemapNs + "changefreq", frequency),
                    new XElement(SitemapNs + "priority", priority),
                    Languages.Supported.Select(alternate => Alternate(alternate, content.Url(path, alternate))),
                    Alternate("x-default", content.Url(path, Languages.Default))));
            }
        }

        urls.Add(new XElement(SitemapNs + "url",
            new XElement(SitemapNs + "loc", content.BaseUrl + content.Site.CvPdf),
            new XElement(SitemapNs + "lastmod", lastmod),
            new XElement(SitemapNs + "changefreq", "monthly"),
            new XElement(SitemapNs + "priority", "0.8")));

        var document = new XDocument(
            new XDeclaration("1.0", "UTF-8", null),
            new XElement(SitemapNs + "urlset", new XAttribute(XNamespace.Xmlns + "xhtml", XhtmlNs), urls));

        return document.Declaration + "\n" + document.Root + "\n";
    }

    private static XElement Alternate(string language, string url) =>
        new(XhtmlNs + "link",
            new XAttribute("rel", "alternate"),
            new XAttribute("hreflang", language),
            new XAttribute("href", url));

    /// <summary>llms.txt (https://llmstxt.org): who this site is about, and where to find what.</summary>
    public static string LlmsTxt(SiteContent content)
    {
        var fr = Languages.French;
        var en = Languages.English;
        var text = new StringBuilder();

        text.AppendLine($"# {content.Site.Name}");
        text.AppendLine();
        text.AppendLine($"> {Headline(content, fr)}");
        text.AppendLine($"> {Headline(content, en)}");
        text.AppendLine();
        text.AppendLine(content.T(content.Profile.Intro, fr));
        text.AppendLine();
        text.AppendLine(content.T(content.Profile.Intro, en));
        text.AppendLine();
        text.AppendLine("Site personnel bilingue : le français est à la racine, l'anglais sous /en. / Bilingual personal website: French at the root, English under /en.");

        foreach (var language in Languages.Supported)
        {
            text.AppendLine();
            text.AppendLine(language == fr ? "## Pages (français)" : "## Pages (English)");
            text.AppendLine();
            foreach (var (key, path) in Constants.Pages.Indexed)
            {
                var meta = content.Page(key);
                text.AppendLine($"- [{content.T(meta.Title, language)}]({content.Url(path, language)}): {content.T(meta.Description, language)}");
            }
        }

        text.AppendLine();
        text.AppendLine("## Documents");
        text.AppendLine();
        text.AppendLine($"- [CV (PDF)]({content.BaseUrl}{content.Site.CvPdf}): CV complet / full resume");
        text.AppendLine($"- [CV complet en texte / Full resume as text]({content.BaseUrl}/{Constants.Files.LlmsFull}): profil, expériences, compétences, formations, en français et en anglais");

        text.AppendLine();
        text.AppendLine("## Contact & profils");
        text.AppendLine();
        text.AppendLine($"- E-mail: {content.Site.Email}");
        foreach (var profile in content.Site.Profiles)
            text.AppendLine($"- {profile}");

        return text.ToString();
    }

    /// <summary>llms-full.txt: the whole resume as plain Markdown, in French then in English.</summary>
    public static string LlmsFullTxt(SiteContent content)
    {
        var text = new StringBuilder();
        foreach (var language in Languages.Supported)
        {
            if (text.Length > 0)
                text.AppendLine().AppendLine("---").AppendLine();
            AppendResume(text, content, language);
        }
        return text.ToString();
    }

    private static void AppendResume(StringBuilder text, SiteContent content, string language)
    {
        string T(LocalizedText? value) => content.T(value, language);
        string Ui(string key) => content.Ui(key, language);
        var resume = content.Resume;
        var site = content.Site;
        var colon = language == Languages.French ? " : " : ": ";

        text.AppendLine($"# {site.Name} — {T(content.Page(Constants.Pages.Resume.Key).Title)}");
        text.AppendLine();
        text.AppendLine(T(content.Profile.Role));
        text.AppendLine();
        text.AppendLine($"- Web: {content.Url(Constants.Pages.Resume.Path, language)}");
        text.AppendLine($"- PDF: {content.BaseUrl}{site.CvPdf}");
        text.AppendLine($"- E-mail: {site.Email}");
        text.AppendLine($"- LinkedIn: {site.LinkedIn}");

        text.AppendLine().AppendLine($"## {Ui(Constants.Ui.Resume.Facts)}").AppendLine();
        foreach (var fact in resume.Facts)
            text.AppendLine($"- {T(fact.Label)}{colon}{T(fact.Value)}");

        text.AppendLine().AppendLine($"## {Ui(Constants.Ui.Resume.Profile)}").AppendLine();
        text.AppendLine(content.Markdown(Constants.Files.Markdown.ResumeProfile, language));

        text.AppendLine().AppendLine($"## {Ui(Constants.Ui.Resume.Experience)}");
        var present = Ui(Constants.Ui.Date.Present);
        foreach (var experience in content.Experiences)
        {
            text.AppendLine();
            text.AppendLine($"### {T(experience.Role)} — {experience.Company}");
            text.AppendLine();
            text.AppendLine($"{LiveDates.Period(experience.Start, experience.End, language, present)} ({LiveDates.Duration(experience.Start, experience.End, content.Today, language)}) · {T(experience.Location)}");
            text.AppendLine();
            foreach (var paragraph in experience.Details)
                text.AppendLine(T(paragraph)).AppendLine();
            text.AppendLine(string.Join(", ", experience.Tags.Select(T)));
        }

        text.AppendLine();
        text.AppendLine($"### {Ui(Constants.Ui.Resume.Others)} ({T(resume.Others.Period)})");
        text.AppendLine();
        text.AppendLine(T(resume.Others.Intro));
        foreach (var other in resume.Others.Items)
            text.AppendLine($"- {other.Company} ({T(other.Location)}){colon}{T(other.Role)}");

        text.AppendLine().AppendLine($"## {Ui(Constants.Ui.Resume.Skills)}").AppendLine();
        foreach (var group in resume.Skills)
            text.AppendLine($"- {T(group.Label)}{colon}{string.Join(", ", group.Items.Select(T))}");

        text.AppendLine().AppendLine($"## {Ui(Constants.Ui.Resume.Trainings)}").AppendLine();
        foreach (var training in resume.Trainings)
            text.AppendLine($"- {training.Year}{colon}{T(training.Label)}");
        if (resume.TrainingsNote is not null)
            text.AppendLine().AppendLine(T(resume.TrainingsNote));

        text.AppendLine().AppendLine($"## {Ui(Constants.Ui.Resume.Education)}").AppendLine();
        foreach (var education in resume.Education)
            text.AppendLine($"- {education.Year}{colon}{T(education.Title)}. {T(education.Text)}");

        text.AppendLine().AppendLine($"## {Ui(Constants.Ui.Resume.Interests)}").AppendLine();
        text.AppendLine(T(resume.Interests));
    }

    private static string Headline(SiteContent content, string language) =>
        content.T(content.Profile.Role, language);
}
