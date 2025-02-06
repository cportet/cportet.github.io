using System.Text.RegularExpressions;
using System.Xml.Linq;
using Markdig;
using Microsoft.AspNetCore.Components;

namespace MyHomePage.Components;

public partial class MyMarkdownRenderer
{
    [Parameter]
    public string? MarkdownContent { get; set; }

    private RenderFragment? _renderedContent;
    private int? _previousMarkdownContentHash;

    protected override void OnParametersSet()
    {
        var newHash = MarkdownContent?.GetHashCode();

        if (newHash == _previousMarkdownContentHash)
            return;

        _previousMarkdownContentHash = newHash;

        if (!string.IsNullOrEmpty(MarkdownContent))
        {
            var htmlContent = Markdown.ToHtml(MarkdownContent);
            _renderedContent = RenderContent(htmlContent);
        }
    }

    private static RenderFragment RenderContent(string htmlContent) => builder =>
    {
        var segments = ComponentTagRegex().Split(htmlContent);
        var sequenceState = new SequenceState();

        foreach (var segment in segments)
        {
            if (segment.StartsWith("<c-"))
            {
                var element = XElement.Parse(segment);
                var tagName = element.Name.LocalName[2..];
                var content = element.Value;
                var type = Type.GetType($"MyHomePage.Components.{tagName}");

                if (type is not null)
                {
                    builder.OpenComponent(sequenceState.Next(), type);

                    foreach (var attribute in element.Attributes())
                    {
                        builder.AddAttribute(sequenceState.Next(), attribute.Name.LocalName, attribute.Value);
                    }

                    if (!string.IsNullOrEmpty(content))
                    {
                        builder.AddContent(sequenceState.Next(), content);
                    }

                    builder.CloseComponent();
                }
                else
                {
                    builder.AddMarkupContent(sequenceState.Next(), segment);
                }
            }
            else
            {
                builder.AddMarkupContent(sequenceState.Next(), segment);
            }
        }
    };

    private sealed class SequenceState
    {
        private int _sequence;

        public int Next() => _sequence++;
    }

    [GeneratedRegex(@"(<c-[^>]+>.*?</c-[^>]+>|<c-[^>]+/>)")]
    private static partial Regex ComponentTagRegex();
}
