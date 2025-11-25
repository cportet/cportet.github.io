using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace MyHomePage.Components
{
    public partial class MyIcon(
        HttpClient http)
    {
        private string? _svgContent;
        private string? _previousIconName;

        [Parameter]
        public string? IconName { get; set; }

        [Parameter]
        public string? Title { get; set; }

        [Parameter]
        public IconSize Size { get; set; } = IconSize.Size20;

        [Parameter]
        public string? Href { get; set; }

        [Parameter]
        public string? Target { get; set; }

        [Parameter]
        public EventCallback OnClick { get; set; }

        [Parameter]
        public string? Class { get; set; }


        private string ClassString
        {
            get
            {
                string baseClass = "my-icon";
                if (!string.IsNullOrEmpty(Class))
                    baseClass += " " + Class;
                return baseClass.Trim();
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            if (_previousIconName != IconName)
            {
                _previousIconName = IconName;

                if (!string.IsNullOrEmpty(IconName))
                {
                    var iconName = IconName.Trim().ToLower();
                    var src = $"/img/{iconName}.svg";

                    _svgContent = await http.GetStringAsync(src);
                }
            }
        }

        private bool HasHref => !string.IsNullOrEmpty(Href);

        private string Cursor => !HasHref && !OnClick.HasDelegate
            ? "default"
            : "pointer";

        private string IconStyle => $"width: {(int)Size}px; height: {(int)Size}px; cursor: {Cursor};";

        private MarkupString? CustomIconContent => CustomIconRef?.ToMarkup();

        private Icon? CustomIconRef => _svgContent is null
            ? null
            : new Icon("Name", IconVariant.Regular, Size, _svgContent);

        private async Task DivOnClickHandler()
        {
            if (OnClick.HasDelegate)
                await OnClick.InvokeAsync();
        }
    }
}
