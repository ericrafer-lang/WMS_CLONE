// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// MODAL SECTION
const modal = document.getElementById("productModal");
const openBtn = document.getElementById("openModal"); //Add Product Btn
const closeBtn = document.getElementById("closeModal"); //Close Btn
const cancelBtn = document.getElementById("cancelModal");

openBtn.addEventListener("click", () => {
    modal.classList.remove("hidden");
    modal.classList.add("flex");
    console.log("clicked")
});

function modalClose() {
    modal.classList.remove("flex");
    modal.classList.add("hidden");
}

closeBtn.addEventListener("click", modalClose);
cancelBtn.addEventListener("click", modalClose);

modal.addEventListener("click", (e) => {
    if (e.target === modal) modalClose()
});
