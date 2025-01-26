let resizeHandlers = {};

function adjustPdfHeight(id) {
    var pdfContainer = document.getElementById(id);
    if (pdfContainer) {
        var marginBottom = 20;
        var topOffset = pdfContainer.getBoundingClientRect().top;
        var availableHeight = window.innerHeight - topOffset - marginBottom;
        pdfContainer.style.height = availableHeight + 'px';
    }
}

function addResizeListener(id) {
    function resizeHandler() {
        adjustPdfHeight(id);
    }
    window.addEventListener('resize', resizeHandler);
    document.addEventListener('DOMContentLoaded', resizeHandler);

    resizeHandlers[id] = resizeHandler;

    adjustPdfHeight(id);
}

function removeResizeListener(id) {
    const resizeHandler = resizeHandlers[id];
    if (resizeHandler) {
        window.removeEventListener('resize', resizeHandler);
        document.removeEventListener('DOMContentLoaded', resizeHandler);
        delete resizeHandlers[id];
    }
}
