// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// MODAL SECTION
function initializeModals() {
    // open modal
    document.querySelectorAll(".open-modal").forEach((button) => {
        button.addEventListener("click", () => {
            const modalId = button.dataset.modal;
            const modal = document.getElementById(modalId);
            modal.classList.remove("hidden");
            modal.classList.add("flex");
        });
    });

    // close modal
    document.querySelectorAll(".close-modal").forEach((button) => {
        button.addEventListener("click", () => {
            const modal = button.closest(".modal-overlay");
            modal.classList.remove("flex");
            modal.classList.add("hidden");
        });
    });

    document.querySelectorAll(".modal-overlay").forEach((modal) => {
        modal.addEventListener("click", (e) => {
            if (e.target === modal) {
                modal.classList.remove("flex");
                modal.classList.add("hidden");
            }
        })
    })
}

initializeModals();

// THEME (dark / light) support
function setTheme(theme) {
    const root = document.documentElement;
    if (theme === 'dark') {
        root.classList.add('dark');
        root.setAttribute('data-theme', 'dark');
    } else {
        root.classList.remove('dark');
        root.removeAttribute('data-theme');
    }
    localStorage.setItem('wms-theme', theme);
    updateThemeToggle();
}

function getPreferredTheme() {
    const stored = localStorage.getItem('wms-theme');
    if (stored) return stored;
    if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) return 'dark';
    return 'light';
}

function updateThemeToggle() {
    const btn = document.getElementById('themeToggleBtn');
    if (!btn) return;

    const sunSVG = `
        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="lucide lucide-sun-icon lucide-sun"><circle cx="12" cy="12" r="4"/><path d="M12 2v2"/><path d="M12 20v2"/><path d="m4.93 4.93 1.41 1.41"/><path d="m17.66 17.66 1.41 1.41"/><path d="M2 12h2"/><path d="M20 12h2"/><path d="m6.34 17.66-1.41 1.41"/><path d="m19.07 4.93-1.41 1.41"/></svg>
    `;

    const moonSVG = `
        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="lucide lucide-moon-icon lucide-moon"><path d="M20.985 12.486a9 9 0 1 1-9.473-9.472c.405-.022.617.46.402.803a6 6 0 0 0 8.268 8.268c.344-.215.825-.004.803.401"/></svg>
    `;

    if (document.documentElement.classList.contains('dark')) {
        btn.innerHTML = sunSVG;
    } else {
        btn.innerHTML = moonSVG;
    }
}


function initializeTheme() {
    try {
        setTheme(getPreferredTheme());
        const btn = document.getElementById('themeToggleBtn');
        if (btn) {
            btn.addEventListener('click', function () {
                const current = document.documentElement.classList.contains('dark') ? 'dark' : 'light';
                setTheme(current === 'dark' ? 'light' : 'dark');
            });
        }
    } catch (e) {
        console.error('Theme initialization failed', e);
    }
}

initializeTheme();
