using Microsoft.AspNetCore.Components;
using MyHomePage.Code;

namespace MyHomePage.Components;

public partial class MyCultureSelector()
{
    private readonly List<CultureItem> _options = Cultures.GetCultures();
    private string? _previousValue;

    [Parameter]
    public string Value { get; set; } = null!;

    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    private async Task UpdateCulture()
    {
        if (_previousValue != Value)
        {
            _previousValue = Value;
            if (ValueChanged.HasDelegate)
                await ValueChanged.InvokeAsync(Value);
        }
    }
}
