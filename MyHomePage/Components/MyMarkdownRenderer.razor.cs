using System.Text.RegularExpressions;
using System.Xml.Linq;
using Markdig;
using Microsoft.AspNetCore.Components;

namespace MyHomePage.Components;

public partial class MyMarkdownRenderer
{
    [Parameter]
    public string? MarkdownContent { get; set; }

    [Parameter]
    public Dictionary<string, string>? Variables { get; set; }

    private RenderFragment? _renderedContent;
    private int? _previousMarkdownContentHash;
    private int? _previousVariablesHash;

    protected override void OnParametersSet()
    {
        var newMarkdownHash = MarkdownContent?.GetHashCode();
        var newVariablesHash = Variables?.GetHashCode();

        if (newMarkdownHash == _previousMarkdownContentHash && newVariablesHash == _previousVariablesHash)
            return;

        _previousMarkdownContentHash = newMarkdownHash;
        _previousVariablesHash = newVariablesHash;

        if (!string.IsNullOrEmpty(MarkdownContent))
        {
            var contentWithVariablesReplaced = ReplaceVariables(MarkdownContent, Variables);
            var htmlContent = Markdown.ToHtml(contentWithVariablesReplaced);
            _renderedContent = RenderContent(htmlContent);
        }
    }

    private static string ReplaceVariables(string content, Dictionary<string, string>? variables)
    {
        if (variables == null || variables.Count == 0)
            return content;

        var regexPattern = string.Join("|", variables.Keys.Select(Regex.Escape));
        var regex = new Regex($"<v-({regexPattern})>", RegexOptions.Compiled);

        return regex.Replace(content, match =>
        {
            var key = match.Groups[1].Value;
            return variables.TryGetValue(key, out var value) ? value : match.Value;
        });
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
