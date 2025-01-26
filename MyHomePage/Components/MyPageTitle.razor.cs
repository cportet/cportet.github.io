using Microsoft.AspNetCore.Components;

namespace MyHomePage.Components
{
    public partial class MyPageTitle
    {

        [Parameter]
        [EditorRequired]
        public string? Title { get; set; }

        [Parameter]
        public string? SubTitle { get; set; }
    }
}