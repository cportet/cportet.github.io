using Microsoft.FluentUI.AspNetCore.Components;
using FluentIcons = Microsoft.FluentUI.AspNetCore.Components.Icons;

namespace MyHomePage;

/// <summary>
/// Every identifier shared between the code and the content: pages and routes, anchors,
/// content files, interface string keys, JavaScript functions, icons, theme colors.
/// </summary>
public static class Constants
{
    /// <summary>Pages: key in pages.json, language-neutral path, route templates.</summary>
    public static class Pages
    {
        /// <summary>Prefix of the English routes.</summary>
        public const string EnglishPrefix = "/en";

        /// <summary>Public pages listed for search engines (sitemap.xml, llms.txt): pages.json key and path.</summary>
        public static readonly (string Key, string Path)[] Indexed =
        [
            (Home.Key, Home.Path),
            (Resume.Key, Resume.Path),
            (Projects.Key, Projects.Path),
            (Privacy.Key, Privacy.Path),
            (Perso.Key, Perso.Path),
            (Contact.Key, Contact.Path),
            (About.Key, About.Path),
        ];

        /// <summary>Former URLs (without leading "/") and the path of the page they now show.</summary>
        public static readonly IReadOnlyDictionary<string, string> CanonicalPaths =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                [Home.Aliases.Index[1..]] = Home.Path,
                [Home.Aliases.IndexHtml[1..]] = Home.Path,
                [Home.Aliases.Home[1..]] = Home.Path,
                [Home.Aliases.Default[1..]] = Home.Path,
                [Home.Aliases.DefaultHtml[1..]] = Home.Path,
                [Resume.Aliases.Cv[1..]] = Resume.Path,
                [Resume.Aliases.CurriculumVitae[1..]] = Resume.Path,
                [Resume.Aliases.Experience[1..]] = Resume.Path,
                [Projects.Aliases.Projets[1..]] = Projects.Path,
                [Projects.Aliases.Applications[1..]] = Projects.Path,
                [Projects.Aliases.MesApps[1..]] = Projects.Path,
                [Privacy.Aliases.Charte[1..]] = Privacy.Path,
                [About.Aliases.APropos[1..]] = About.Path,
                [Preferences.Aliases.Options[1..]] = Preferences.Path,
            };

        public static class Home
        {
            public const string Key = "home";
            public const string Path = "";
            public const string Route = "/";
            public const string RouteEn = EnglishPrefix;

            public static class Aliases
            {
                public const string Index = "/index";
                public const string IndexHtml = "/index.html";
                public const string Home = "/home";
                public const string Default = "/default";
                public const string DefaultHtml = "/default.html";
            }
        }

        public static class Resume
        {
            public const string Key = "resume";
            public const string Path = "resume";
            public const string Route = "/" + Path;
            public const string RouteEn = EnglishPrefix + Route;

            public static class Aliases
            {
                public const string Cv = "/cv";
                public const string CurriculumVitae = "/curriculumvitae";
                public const string Experience = "/experience";
            }
        }

        public static class Projects
        {
            public const string Key = "projects";
            public const string Path = "projects";
            public const string Route = "/" + Path;
            public const string RouteEn = EnglishPrefix + Route;

            public static class Aliases
            {
                public const string Projets = "/projets";
                public const string Applications = "/applications";
                public const string MesApps = "/applications/mesapps";
            }
        }

        public static class Privacy
        {
            public const string Key = "charter";
            public const string Path = "projects/privacy";
            public const string Route = "/" + Path;
            public const string RouteEn = EnglishPrefix + Route;

            public static class Aliases
            {
                public const string Charte = "/applications/charte";
            }
        }

        public static class Perso
        {
            public const string Key = "perso";
            public const string Path = "perso";
            public const string Route = "/" + Path;
            public const string RouteEn = EnglishPrefix + Route;
        }

        public static class Contact
        {
            public const string Key = "contact";
            public const string Path = "contact";
            public const string Route = "/" + Path;
            public const string RouteEn = EnglishPrefix + Route;
        }

        public static class About
        {
            public const string Key = "about";
            public const string Path = "about";
            public const string Route = "/" + Path;
            public const string RouteEn = EnglishPrefix + Route;

            public static class Aliases
            {
                public const string APropos = "/apropos";
            }
        }

        public static class Preferences
        {
            public const string Key = "preferences";
            public const string Path = "preferences";
            public const string Route = "/" + Path;
            public const string RouteEn = EnglishPrefix + Route;

            public static class Aliases
            {
                public const string Options = "/options";
            }
        }

        public static class NotFound
        {
            public const string Key = "notFound";
            public const string Path = "404";
            public const string Route = "/" + Path;
            public const string RouteEn = EnglishPrefix + Route;
        }

        /// <summary>Target of wwwroot/404.html (GitHub Pages deep links).</summary>
        public static class Redirection
        {
            public const string Route = "/redirection";

            public static class Aliases
            {
                public const string Redir = "/redir";
            }
        }

        /// <summary>Former document viewer.</summary>
        public static class File
        {
            public const string Route = "/file/{Kind}";
            public const string CvKind = "cv";

            public static class Aliases
            {
                public const string Fichier = "/fichier/{Kind}";
            }
        }
    }

    /// <summary>Anchors (element ids) inside pages.</summary>
    public static class Sections
    {
        public const string Main = "main";

        public static class Resume
        {
            public const string Profile = "profile";
            public const string Experience = "experience";
            public const string Skills = "skills";
            public const string Trainings = "trainings";
            public const string Education = "education";
        }
    }

    /// <summary>Files under wwwroot.</summary>
    public static class Files
    {
        public const string AppConfig = "appconfig.json";
        public const string ResumePortrait = "/img/me2.jpg";

        /// <summary>Generated from the content by MyHomePage.Tools.</summary>
        public const string Sitemap = "sitemap.xml";
        public const string Llms = "llms.txt";
        public const string LlmsFull = "llms-full.txt";

        /// <summary>Link preview image (Open Graph, X), 1200×630.</summary>
        public const string SocialPreview = "/img/preview.jpg";
        public const string NotFoundImage = "/img/404.png";

        public static class Content
        {
            public const string Site = "content/site.json";
            public const string Profile = "content/profile.json";
            public const string Experiences = "content/resume/experiences.json";
            public const string Resume = "content/resume/resume.json";
            public const string Perso = "content/perso.json";
            public const string Projects = "content/projects.json";
            public const string About = "content/about.json";
            public const string Pages = "content/pages.json";
            public const string Ui = "content/ui.json";

            /// <summary>{0}: Markdown path, {1}: language.</summary>
            public const string MarkdownFormat = "content/md/{0}.{1}.md";
        }

        /// <summary>Markdown texts (content/md/{path}.{language}.md).</summary>
        public static class Markdown
        {
            public const string ResumeProfile = "resume/profile";
            public const string PersoIntro = "perso/intro";
            public const string PrivacyPolicy = "projects/charter";
        }
    }

    /// <summary>Live values that can be used in any text of the content.</summary>
    public static class Tokens
    {
        public const string Age = "{{age}}";
        public const string ExperienceYears = "{{experienceYears}}";
        public const string ManagementYears = "{{managementYears}}";
        public const string CurrentRoleDuration = "{{currentRoleDuration}}";
        public const string Year = "{{year}}";
    }

    /// <summary>Keys of the interface strings in ui.json.</summary>
    public static class Ui
    {
        public static class Nav
        {
            public const string Main = "nav.main";
            public const string Open = "nav.open";
            public const string Close = "nav.close";
            public const string Skip = "nav.skip";
            public const string Family = "nav.family";
            public const string Featured = "nav.featured";
            public const string LanguageGroup = "lang.group";
        }

        public static class Theme
        {
            public const string ToDark = "theme.toggleToDark";
            public const string ToLight = "theme.toggleToLight";
        }

        public static class Pwa
        {
            public const string Install = "pwa.install";
            public const string UpdateText = "pwa.updateText";
            public const string UpdateButton = "pwa.updateButton";
        }

        public static class Footer
        {
            public const string About = "footer.about";
        }

        public static class Image
        {
            /// <summary>Format, {0}: description of the photo.</summary>
            public const string Zoom = "image.zoom";
            public const string Close = "image.close";
        }

        public static class Date
        {
            public const string Present = "date.present";
        }

        public static class Cta
        {
            public const string Resume = "cta.resume";
            public const string Pdf = "cta.pdf";
            public const string DownloadPdf = "cta.downloadPdf";
            public const string LinkedIn = "cta.linkedin";
            public const string FullResume = "cta.fullResume";
        }

        public static class Home
        {
            public const string Stats = "home.stats";
            public const string Recent = "home.recent";

            public static class Apps
            {
                public const string Title = "home.apps.title";
                public const string Text = "home.apps.text";
                public const string Link = "home.apps.link";
            }

            public static class Site
            {
                public const string Title = "home.site.title";
                public const string Text = "home.site.text";
                public const string Link = "home.site.link";
            }

            public static class Contact
            {
                public const string Title = "home.contact.title";
                public const string Text = "home.contact.text";
                public const string More = "home.contact.more";
            }

            public static class Links
            {
                public const string Title = "home.links.title";
                public const string Text = "home.links.text";
                public const string More = "home.links.more";
            }
        }

        public static class Resume
        {
            public const string Sections = "resume.sections";
            public const string Profile = "resume.profile";
            public const string Experience = "resume.experience";
            public const string Skills = "resume.skills";
            public const string Trainings = "resume.trainings";
            public const string TrainingsShort = "resume.trainingsShort";
            public const string Education = "resume.education";
            public const string Facts = "resume.facts";
            public const string Others = "resume.others";
            public const string Interests = "resume.interests";
        }

        public static class Projects
        {
            public const string Store = "projects.store";
            public const string Privacy = "projects.privacy";
            public const string PrivacyTitle = "projects.privacyTitle";

            /// <summary>Format, {0}: date of the last update.</summary>
            public const string PrivacyUpdated = "projects.privacyUpdated";
            public const string PrivacyRead = "projects.privacyRead";
            public const string Back = "projects.back";
        }

        public static class Perso
        {
            public const string Family = "perso.family";
            public const string Featured = "perso.featured";
            public const string FeaturedLead = "perso.featuredLead";
            public const string FindMe = "perso.findMe";
            public const string ByEmail = "perso.byEmail";
        }

        public static class Contact
        {
            public const string Methods = "contact.methods";

            public static class Email
            {
                public const string Badge = "contact.email.badge";
                public const string Action = "contact.email.action";
                public const string Subject = "contact.email.subject";
            }

            public static class Phone
            {
                public const string Badge = "contact.phone.badge";
                public const string Action = "contact.phone.action";
            }

            public static class LinkedIn
            {
                public const string Badge = "contact.linkedin.badge";
            }

            public static class Resume
            {
                public const string Title = "contact.resume.title";
                public const string Text = "contact.resume.text";
                public const string Web = "contact.resume.web";
            }
        }

        public static class About
        {
            /// <summary>HTML.</summary>
            public const string Support = "about.support";

            public static class App
            {
                public const string Title = "about.app.title";

                /// <summary>Format, {0}: version.</summary>
                public const string Text = "about.app.text";
                public const string Install = "about.app.install";
                public const string Installed = "about.app.installed";
                public const string Preferences = "about.app.preferences";
            }
        }

        public static class Preferences
        {
            public const string Language = "prefs.language";
            public const string Reset = "prefs.reset";

            public static class Mode
            {
                public const string Title = "prefs.mode";
                public const string System = "prefs.mode.system";
                public const string Light = "prefs.mode.light";
                public const string Dark = "prefs.mode.dark";
            }

            public static class Accent
            {
                public const string Title = "prefs.accent";
                public const string Blue = "prefs.accent.blue";
                public const string Teal = "prefs.accent.teal";
                public const string Purple = "prefs.accent.purple";
                public const string Red = "prefs.accent.red";
            }
        }

        public static class NotFound
        {
            public const string Back = "notFound.back";
            public const string Home = "notFound.home";
        }

        public static class Redirect
        {
            public const string Text = "redirect.text";
        }
    }

    /// <summary>Date formats (month names come from the page language).</summary>
    public static class Formats
    {
        public const string MonthYear = "MMMM yyyy";
        public const string LongDate = "d MMMM yyyy";
    }

    /// <summary>Keyboard keys handled by the components.</summary>
    public static class Keys
    {
        public const string Escape = "Escape";
    }

    /// <summary>Functions of wwwroot/js/scripts.js.</summary>
    public static class Js
    {
        public const string Started = "cp.started";
        public const string SetDocumentLanguage = "cp.setDocumentLanguage";
        public const string ScrollToTop = "cp.scrollToTop";

        public static class Pwa
        {
            public const string CanInstall = "cp.pwa.canInstall";
            public const string Install = "cp.pwa.install";
        }
    }

    public static class Theme
    {
        public static class Accents
        {
            public const string Blue = "#0F6CBD";
            public const string Teal = "#0B7A75";
            public const string Purple = "#6B4FBB";
            public const string Red = "#C4314B";

            public const string Default = Blue;
        }
    }

    /// <summary>Fluent UI System icons, by size (the size of the icon is its drawing size).</summary>
    public static class Icons
    {
        public static class Small
        {
            public static Icon Open => new FluentIcons.Regular.Size16.Open();
        }

        public static class Regular
        {
            public static Icon ArrowRight => new FluentIcons.Regular.Size20.ArrowRight();
            public static Icon Download => new FluentIcons.Regular.Size20.ArrowDownload();
            public static Icon Mail => new FluentIcons.Regular.Size20.Mail();
            public static Icon Open => new FluentIcons.Regular.Size20.Open();
            public static Icon Checkmark => new FluentIcons.Regular.Size20.Checkmark();
            public static Icon Reset => new FluentIcons.Regular.Size20.ArrowReset();
            public static Icon Moon => new FluentIcons.Regular.Size20.WeatherMoon();
            public static Icon Sun => new FluentIcons.Regular.Size20.WeatherSunny();
        }

        public static class Large
        {
            public static Icon Menu => new FluentIcons.Regular.Size24.Navigation();
            public static Icon Close => new FluentIcons.Regular.Size24.Dismiss();
        }

        public static class Contact
        {
            public static Icon Mail => new FluentIcons.Regular.Size28.Mail();
            public static Icon Phone => new FluentIcons.Regular.Size28.Call();
            public static Icon Network => new FluentIcons.Regular.Size28.PeopleTeam();
        }

        public static class Huge
        {
            public static Icon App => new FluentIcons.Regular.Size48.Apps();
        }
    }
}
