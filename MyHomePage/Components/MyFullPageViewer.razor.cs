using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MyHomePage.Components
{
    public partial class MyFullPageViewer
    {

        [Parameter]
        [EditorRequired]
        public string? File { get; set; }

        [Parameter]
        [EditorRequired]
        public string? FileType { get; set; }

        [Parameter]
        public string Id { get; set; } = "pdfContainer";

        private string? _pdfContainerId;
        private string? _file;
        private string? _fileType;
        private bool _hasFile;
        private bool _wasSet;

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (string.IsNullOrWhiteSpace(Id))
                throw new ArgumentException("Id is required.");

            _file = File;
            _fileType = FileType;
            _pdfContainerId = Id;

            _hasFile = !string.IsNullOrWhiteSpace(_file) && !string.IsNullOrWhiteSpace(_fileType);
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!_wasSet && _hasFile)
            {
                _wasSet = true;
                await JSRuntime.InvokeVoidAsync("addResizeListener", _pdfContainerId);
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_wasSet)
            {
                _wasSet = false;
                await JSRuntime.InvokeVoidAsync("removeResizeListener", _pdfContainerId);
            }
        }
    }
}