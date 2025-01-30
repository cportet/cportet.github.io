using Microsoft.AspNetCore.Components;
using MyHomePage.Code;

namespace MyHomePage.Components;

public partial class MySelect<TItem>
where TItem : class, ISelectItem
{
    private string? _previousValue;

    [Parameter]
    [EditorRequired]
    public string Id { get; set; } = null!;

    [Parameter]
    [EditorRequired]
    public string Value { get; set; } = null!;

    [Parameter]
    [EditorRequired]
    public List<TItem>? Items { get; set; }

    [Parameter]
    public string? Label { get; set; }

    [Parameter]
    public bool DisplayKeyInText { get; set; } = true;

    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    private async Task UpdateValue()
    {
        if (_previousValue != Value)
        {
            _previousValue = Value;
            if (ValueChanged.HasDelegate)
                await ValueChanged.InvokeAsync(Value);
        }
    }

    private string FormatItem(TItem item) => DisplayKeyInText
        ? $"{item.SelectText} ({item.SelectKey})"
        : item.SelectText;
}
