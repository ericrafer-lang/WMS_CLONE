// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

    // MODAL SECTION
function initializeModal(modalId, openId, closeId, cancelId) {
    const modal = document.getElementById(modalId);
    const openBtn = document.getElementById(openId);
    const closeBtn = document.getElementById(closeId);
    const cancelBtn = document.getElementById(cancelId);

    if (!modal || !openBtn || !closeBtn || !cancelBtn) return;

    function closeModal() {
        modal.classList.remove("flex");
        modal.classList.add("hidden");
    }

    openBtn.addEventListener("click", () => {
        modal.classList.remove("hidden");
        modal.classList.add("flex");
    });

    closeBtn.addEventListener("click", closeModal);
    cancelBtn.addEventListener("click", closeModal);

    modal.addEventListener("click", (e) => {
        if (e.target === modal) {
            closeModal();
        }
    });
}
