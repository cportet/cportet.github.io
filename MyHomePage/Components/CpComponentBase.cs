using Microsoft.AspNetCore.Components;
using MyHomePage.Content;

namespace MyHomePage.Components;

/// <summary>Base class of pages and components: gives access to the content and the current language.</summary>
public abstract class CpComponentBase : ComponentBase
{
    [Inject]
    protected ContentService Content { get; set; } = null!;

    [Inject]
    protected LanguageService Lang { get; set; } = null!;

    protected string T(LocalizedText? text) => Content.T(text);

    protected string Ui(string key) => Content.Ui(key);

    protected string Href(string path) => Lang.Href(path);
}
