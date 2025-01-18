document.addEventListener("DOMContentLoaded", function () {
    const menuItems = document.querySelectorAll(".menu-item a");
    const currentUrl = window.location.pathname;

    menuItems.forEach(item => {
        if (item.getAttribute("href") === currentUrl) {
            document.querySelector(".menu-item.active")?.classList.remove("active");
            item.parentElement.classList.add("active");
        }
    });
});
