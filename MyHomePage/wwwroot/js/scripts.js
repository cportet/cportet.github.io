window.cp = {
    pwa: {
        deferredPrompt: null,
        installed: window.matchMedia('(display-mode: standalone)').matches || window.navigator.standalone === true,

        canInstall: function () {
            return !!window.cp.pwa.deferredPrompt && !window.cp.pwa.installed;
        },

        install: async function () {
            const prompt = window.cp.pwa.deferredPrompt;
            if (!prompt)
                return false;

            prompt.prompt();
            const { outcome } = await prompt.userChoice;
            window.cp.pwa.deferredPrompt = null;
            if (outcome === 'accepted')
                window.cp.pwa.installed = true;
            return outcome === 'accepted';
        }
    },

    setDocumentLanguage: function (language) {
        document.documentElement.lang = language;
    },

    // Once the app runs, the Fluent theme takes over the boot-time dark colors.
    started: function () {
        document.documentElement.classList.remove('cp-dark-boot');
    },

    scrollToTop: function () {
        window.scrollTo({ top: 0, left: 0, behavior: 'instant' });
    }
};

window.addEventListener('beforeinstallprompt', e => {
    e.preventDefault();
    window.cp.pwa.deferredPrompt = e;
});

window.addEventListener('appinstalled', () => {
    window.cp.pwa.installed = true;
    window.cp.pwa.deferredPrompt = null;
});
