using Microsoft.AspNetCore.Components;
using MyHomePage.Layout;

namespace MyHomePage.Components
{
    public partial class MyPageTitle
    {
        [CascadingParameter]
        public MainLayout? MainLayout { get; set; }

        [Parameter]
        [EditorRequired]
        public string? Title { get; set; }

        [Parameter]
        public string? SubTitle { get; set; }

        private string? _previousTitle;
        protected override void OnParametersSet()
        {
            if (_previousTitle != Title)
            {
                _previousTitle = Title;
                MainLayout?.SetPageTitle(Title);
            }

            base.OnParametersSet();
        }
    }
}