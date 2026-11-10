# Cyril Portet — personal website

Source of [www.portet.org](https://www.portet.org): home page, resume (web version of the PDF), projects and personal links, in French and English.

This README is the reference for anyone changing the site, people and AI assistants alike: read **Ground rules** before any change.

## Ground rules

These are decisions, not suggestions. Ask the owner before going against one.

1. **Blazor WebAssembly, client only.** No server, no API, no database: the site is static files on GitHub Pages.
2. **Installable PWA with forced updates** through [Toolbelt.Blazor.PWA.Updater](https://github.com/jsakamoto/Toolbelt.Blazor.PWA.Updater). Keep it (or an equivalent): a new deployment must reach installed apps.
3. **Content is JSON and Markdown**, in a folder tree under `MyHomePage/wwwroot/content`. **No YAML in the app** (YAML is only for GitHub Actions). Texts are never hard-coded in Razor files.
4. **Microsoft design system**: Fluent UI Blazor v5 and its Fluent 2 tokens, with the "Fluent éditorial" layer of `wwwroot/css/app.css`. Light and dark themes and the accent colors must keep working.
5. **Bilingual**: the URL sets the language (`/resume` is French, `/en/resume` is English). Every text exists in both languages; the tests enforce it.
6. **Live dates**: ages and durations are computed, never typed (see Content). In the PDF resume, prefer rounded and stable wording ("plus de 25 ans", "plus de 10 ans").
7. **Sober tone**, no marketing wording. No "Print" button: the PDF resume is the printable version.
8. **Privacy**: no tracking, no cookies, no third-party scripts. In structured data (JSON-LD), the e-mail address only, never the phone number.
9. **Every identifier lives in `MyHomePage/Constants.cs`** (routes, content files, string keys, JS functions, icons, colors). No magic strings in pages.
10. **Work on a branch and open a pull request** to `master`; the CI builds and tests every pull request and deploys only `master`. Direct pushes to `master` only when the owner asks.

## Stack

- **Blazor WebAssembly**, .NET 10 (C#), SDK pinned in `global.json`
- **Fluent UI Blazor v5** (`Microsoft.FluentUI.AspNetCore.Components`, plus Icons), package versions centralized in `Directory.Packages.props`
- **Prerendering** at publish time ([BlazorWasmPreRendering.Build](https://github.com/jsakamoto/BlazorWasmPreRendering.Build)): one static HTML file per page
- **Tests**: xUnit v3 on Microsoft Testing Platform
- **Hosting**: GitHub Pages (custom domain in `CNAME`), built and deployed by GitHub Actions

## Repository layout

| Path | Contents |
| --- | --- |
| `MyHomePage/` | The Blazor app |
| `MyHomePage/Constants.cs` | Every shared identifier (pages, files, string keys, icons…) |
| `MyHomePage/Content/` | Content model (`Models.cs`), JSON source generation (`ContentJsonContext.cs`), `ContentService`, `LanguageService`, `LiveDates`, `StructuredData` |
| `MyHomePage/Pages/`, `Layout/`, `Components/` | Razor pages, header / footer / layout, shared components (`Seo`, `PageHeader`, `Markdown`, `Tag`, `ZoomImage`, `CpIcon`) |
| `MyHomePage/Theming/` | Light / dark and accent color preferences |
| `MyHomePage/wwwroot/content/` | **All the texts** (see Content) |
| `MyHomePage/wwwroot/css/app.css` | The design layer, on top of the Fluent tokens |
| `MyHomePage/wwwroot/doc/cv-cyril-portet.pdf` | The PDF resume linked from the site |
| `MyHomePage.Tools/` | Console tool: generates `sitemap.xml`, `llms.txt`, `llms-full.txt`, fixes prerendered English pages |
| `MyHomePage.Tests/` | Content validation and unit tests |
| `.github/workflows/deploy.yml` | Build, test, publish and deploy |

## Content

All texts live in `MyHomePage/wwwroot/content`. Updating them needs no code change.

| File | Contents |
| --- | --- |
| `site.json` | Name, links, menu, social networks |
| `profile.json` | Birth date, career and management start dates, home page hero and key figures |
| `resume/experiences.json` | Positions, most recent first (no `end` for the current one; the home page shows the first three) |
| `resume/resume.json` | Facts, skills, training, education, earlier experience, interests |
| `perso.json` | Family photo and sites, "À découvrir" section (also in the mobile menu) |
| `projects.json` | Apps, project cards, privacy summary |
| `about.json` | "About this site" page |
| `pages.json` | Title, SEO description and header of each page |
| `ui.json` | Interface strings (keys are in `Constants.Ui`) |
| `md/**/*.{fr,en}.md` | Longer texts (resume profile, personal intro, privacy policy) |

Rules for the JSON files:

- A translatable text is either a plain string (same in both languages) or `{ "fr": "…", "en": "…" }`.
- Comments (`// …`) are allowed and welcome to explain a file.
- Ages and durations: write `{{age}}`, `{{experienceYears}}`, `{{managementYears}}`, `{{currentRoleDuration}}` or `{{year}}`; they are computed from the dates of `profile.json`.
- Dates are `YYYY-MM-DD`; an experience's `end` is its last month, inclusive.
- Typography: French texts use the typographic apostrophe (’) and non-breaking spaces where French needs them.
- Interface strings stay short, the PWA update bar above all ("MàJ", "New version available!"): long texts overflow on phones.

### Common updates

- **New position**: add it at the top of `resume/experiences.json`, give the previous one its `end`. Update `profile.json` key figures if needed, then the PDF resume.
- **New skill, training or fact**: `resume/resume.json`.
- **New "À découvrir" site**: an entry in the `featured` list of `perso.json`, with its picture in `wwwroot/img/featured/` (JPEG, 16:9).
- **New page**: a class in `Constants.Pages` (key, path, routes, aliases), an entry in `pages.json`, a Razor page using `@attribute [Route(Constants.Pages.X.Route)]` (and `RouteEn`), the menu in `site.json` if it belongs there, and `Constants.Pages.Indexed` if search engines should list it. Former URLs go in `CanonicalPaths` and, to be prerendered, in `BlazorWasmPrerenderingUrlPathToExplicitFetch` (`MyHomePage.csproj`).
- **New PDF resume**: replace `wwwroot/doc/cv-cyril-portet.pdf` (same name: every link points to it). Keep it consistent with `experiences.json` and `resume.json`.
- **App shortcuts and credits**: `wwwroot/manifest.webmanifest` (shortcuts), `wwwroot/humans.txt`.

## Coding conventions

- C# with nullable enabled; follow the style of the surrounding code (naming, comment density, idioms). Code, comments and commit messages are in English.
- Pages inherit `CpComponentBase` and get texts through `T(…)` (localized text), `Ui(key)` (interface string) and `Href(path)` (localized link).
- Content models are classes deserialized by System.Text.Json **source generation** (`ContentJsonContext`, camelCase). A new model type must be added there. Lists are declared `{ get; init => field = value ?? []; } = [];` so that a missing list is never null.
- Styles: use the Fluent tokens (`--colorNeutralBackground1`, `--colorBrandForeground1`…) and the `--cp-*` variables of `app.css`, never fixed colors, so both themes keep working. Check every change at phone width (375 px): no horizontal scroll, menus that scroll.
- Accessibility: real links and buttons, `aria-label` on icon-only buttons, 4.5:1 text contrast in both themes.
- JavaScript stays minimal, in `wwwroot/js/scripts.js` under `window.cp`, called through `Constants.Js`.

## Search engines and AI assistants

- **Prerendering**: `/resume` is served from `resume.html` with its real content and an HTTP 200, then Blazor takes over. The service worker serves the same prerendered pages offline.
- **Per page** (`Components/Seo.razor`): title, description, canonical URL, `hreflang` alternates, Open Graph / X cards and schema.org JSON-LD (`Content/StructuredData.cs`), all built from the content.
- **Generated files**, by `MyHomePage.Tools` (the CI runs it, no need to commit the result by hand):

  ```bash
  dotnet run --project MyHomePage.Tools -- generate MyHomePage/wwwroot
  ```

  writes `sitemap.xml`, `llms.txt` (summary for AI assistants) and `llms-full.txt` (the whole resume as text, FR and EN). After publishing, `postpublish <wwwroot>` sets `lang="en"` on the English pages and updates the service worker hashes.
- `robots.txt` allows every crawler, AI crawlers included.
- **IndexNow**: after each deployment the CI sends the sitemap URLs to IndexNow (Bing, DuckDuckGo, Ecosia, Yahoo…). The key file `wwwroot/<32 hex characters>.txt` and `BingSiteAuth.xml` must stay at the root of the site.

## Develop

```bash
dotnet run --project MyHomePage
```

```bash
dotnet test --project MyHomePage.Tests -c Release
```

The tests check that every content file is valid, that every text exists in both languages and that every key used by the pages exists. Run them before every commit. Use `-c Release` when Visual Studio is open: it locks the Debug output.

To check the prerendered site as deployed:

```bash
dotnet publish MyHomePage/MyHomePage.csproj -c Release -o release
```

After a package upgrade, delete `bin/` and `obj/` if the browser keeps loading old files.

## Build and deployment

`.github/workflows/deploy.yml`, on every push and pull request to `master`:

1. tests, then stamps the build time (`appconfig.json` and the service worker, which makes installed apps update);
2. generates the SEO files (sitemap `lastmod` = date of the last content change);
3. publishes with prerendering, fixes the English pages, adds `.nojekyll` and `CNAME`;
4. on `master` only: deploys to the `gh-pages` branch, then notifies IndexNow.

A pull request that fails the build cannot break the live site: deployment only runs after a successful build on `master`.

## Licence

GNU GPL v3. Author: Cyril Portet.
