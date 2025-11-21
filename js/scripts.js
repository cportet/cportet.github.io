window.pwaInstall = {
    deferredPrompt: null,
    isInstalled: false,

    init: function () {
        const isStandalone = window.matchMedia('(display-mode: standalone)').matches ||
            window.navigator.standalone === true;

        if (isStandalone) {
            window.pwaInstall.isInstalled = true;
            return;
        }

        window.addEventListener('beforeinstallprompt', (e) => {
            e.preventDefault();
            window.pwaInstall.deferredPrompt = e;
        });

        window.addEventListener('appinstalled', () => {
            window.pwaInstall.isInstalled = true;
            window.pwaInstall.deferredPrompt = null;
        });
    },
    canInstall: function () {
        return !!window.pwaInstall.deferredPrompt && !window.pwaInstall.isInstalled;
    },
    showInstallPrompt: async function () {
        if (!window.pwaInstall.deferredPrompt) return false;

        const prompt = window.pwaInstall.deferredPrompt;
        prompt.prompt();

        const { outcome } = await prompt.userChoice;
        window.pwaInstall.deferredPrompt = null;

        if (outcome === 'accepted') {
            window.pwaInstall.isInstalled = true;
        }

        return outcome === 'accepted';
    }
};


window.pwaInstall.init();

let resizeHandlers = {};

function scrollToTop() {
    window.scrollTo(0, 0);
    const scrollable = document.querySelector('.my-body');
    if (scrollable) scrollable.scrollTop = 0;
}

function applyInitialTheme() {
    var theme = localStorage.getItem('theme');
    if (theme === 'dark') {
        document.body.classList.add('initial-dark');
    }
}

function removeInitialDarkClass() {
    document.body.classList.remove('initial-dark');
}


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
