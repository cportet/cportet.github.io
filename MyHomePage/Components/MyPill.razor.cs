using Microsoft.AspNetCore.Components;

namespace MyHomePage.Components
{
    public partial class MyPill
    {
        [Parameter]
        public string? Text { get; set; }
    }
}