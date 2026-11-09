# Cyril Portet — personal website

Source of [www.portet.org](https://www.portet.org): home page, resume (web version of the PDF), projects and personal links, in French and English.

## Stack

- **Blazor WebAssembly** (.NET 10, C#), client only, installable **PWA** with automatic updates ([Toolbelt.Blazor.PWA.Updater](https://github.com/jsakamoto/Toolbelt.Blazor.PWA.Updater))
- **Fluent UI Blazor v5** (Fluent 2 design tokens, light/dark themes, brand color) plus a small "Fluent éditorial" design layer in `wwwroot/css/app.css`
- Hosted on **GitHub Pages**, built, tested and deployed by **GitHub Actions** (`.github/workflows/deploy.yml`)

## Content

All texts live in `MyHomePage/wwwroot/content`, as JSON plus Markdown for longer texts. No code change is needed to update them.

| File | Contents |
| --- | --- |
| `site.json` | Name, links, menu, social networks |
| `profile.json` | Birth date, career start, home page hero and key figures |
| `resume/experiences.json` | Positions (one entry each; no `end` for the current one) |
| `resume/resume.json` | Facts, skills, training, education, earlier experience |
| `perso.json` | Family sites and the "À découvrir" section |
| `projects.json` | Apps, project cards, privacy summary |
| `about.json` | "About this site" page |
| `pages.json` | Title, SEO description and header of each page |
| `ui.json` | Interface strings |
| `md/**/*.{fr,en}.md` | Longer texts (resume profile, personal intro, privacy policy) |

Translatable texts are either a plain string (same in every language) or `{ "fr": "…", "en": "…" }`.
Ages and durations are never written by hand: use `{{age}}`, `{{experienceYears}}` or `{{currentRoleDuration}}`, computed from the dates.

The URL sets the language: `/resume` is French, `/en/resume` is English.

## Develop

```bash
dotnet run --project MyHomePage
```

```bash
dotnet test --project MyHomePage.Tests
```

The tests check that every content file is valid, that every text exists in both languages, and that every key used by the pages exists.

## Licence

GNU GPL v3. Author: Cyril Portet.
