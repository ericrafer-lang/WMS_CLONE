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

function initializeSidebar() {
    const sidebar = document.getElementById("appSidebar");
    const collapseBtn = document.getElementById("sidebarCollapseBtn");

    if (!sidebar || !collapseBtn) return;

    if (localStorage.getItem("wms-sidebar-collapsed") === "true") {
        sidebar.classList.add("collapsed");
    }

    collapseBtn.addEventListener("click", () => {
        sidebar.classList.toggle("collapsed");
        localStorage.setItem("wms-sidebar-collapsed", sidebar.classList.contains("collapsed"));
    });
}

initializeSidebar();
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
    if (document.documentElement.classList.contains('dark')) {
        btn.textContent = '☀️';
    } else {
        btn.textContent = '🌙';
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
