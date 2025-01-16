
const toggler = document.querySelector('.navbar-toggler');
const navMenu = document.querySelector('.navbar-nav');

toggler.addEventListener('click', () => {
    navMenu.classList.toggle('active');
});
