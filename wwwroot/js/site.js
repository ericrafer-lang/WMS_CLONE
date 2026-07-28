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
