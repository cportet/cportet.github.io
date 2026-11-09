using System.Text;
using System.Text.RegularExpressions;
using MyHomePage;
using MyHomePage.Content;
using MyHomePage.Tools;

// Build-time tasks for the website.
//
//   generate <wwwroot> [--lastmod yyyy-MM-dd]
//       Writes sitemap.xml, llms.txt and llms-full.txt from the content.
//   postpublish <published wwwroot>
//       Fixes the prerendered English pages: <html lang="en">.

var utf8 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

switch (args)
{
    case ["generate", var wwwroot, ..]:
        var lastModified = args.SkipWhile(a => a != "--lastmod").Skip(1).FirstOrDefault() is { } date
            ? DateOnly.Parse(date)
            : DateOnly.FromDateTime(DateTime.UtcNow);
        var content = new SiteContent(wwwroot, DateOnly.FromDateTime(DateTime.UtcNow));

        Write(Constants.Files.Sitemap, Generators.Sitemap(content, lastModified));
        Write(Constants.Files.Llms, Generators.LlmsTxt(content));
        Write(Constants.Files.LlmsFull, Generators.LlmsFullTxt(content));

        void Write(string file, string text)
        {
            File.WriteAllText(Path.Combine(wwwroot, file), text.ReplaceLineEndings("\n"), utf8);
            Console.WriteLine($"Generated {file}");
        }
        return 0;

    case ["postpublish", var published]:
        var englishPages = Directory.GetFiles(Path.Combine(published, Languages.English), "*.html", SearchOption.AllDirectories)
            .Append(Path.Combine(published, $"{Languages.English}.html"))
            .Where(File.Exists);

        var manifestPath = Path.Combine(published, "service-worker-assets.js");
        var manifest = File.Exists(manifestPath) ? File.ReadAllText(manifestPath) : null;

        foreach (var page in englishPages)
        {
            var html = File.ReadAllText(page);
            var fixedHtml = Regex.Replace(html, "<html lang=\"[a-z-]+\"", $"<html lang=\"{Languages.English}\"");
            if (fixedHtml == html)
                continue;

            File.WriteAllText(page, fixedHtml, utf8);

            // Precompressed copies are now stale.
            File.Delete(page + ".br");
            File.Delete(page + ".gz");

            // The service worker checks the integrity of every cached file: update its hash.
            if (manifest is not null)
            {
                // As written in the manifest: "en\/resume.html".
                var url = Path.GetRelativePath(published, page).Replace('\\', '/').Replace("/", @"\/");
                var hash = "sha256-" + Convert.ToBase64String(System.Security.Cryptography.SHA256.HashData(File.ReadAllBytes(page)));
                manifest = Regex.Replace(manifest,
                    $"(\"hash\":\\s*\")[^\"]+(\",\\s*\"url\":\\s*\"{Regex.Escape(url)}\")",
                    m => m.Groups[1].Value + hash.Replace("/", "\\/") + m.Groups[2].Value);
            }
        }

        if (manifest is not null)
        {
            File.WriteAllText(manifestPath, manifest, utf8);
            File.Delete(manifestPath + ".br");
            File.Delete(manifestPath + ".gz");
        }

        Console.WriteLine("Prerendered English pages: lang=\"en\"");
        return 0;

    default:
        Console.Error.WriteLine("Usage: generate <wwwroot> [--lastmod yyyy-MM-dd] | postpublish <published wwwroot>");
        return 1;
}
