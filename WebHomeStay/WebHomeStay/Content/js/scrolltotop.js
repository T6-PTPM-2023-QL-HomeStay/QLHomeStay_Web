// Show the scroll-to-top button when user scrolls down 20px from the top of the document
window.onscroll = function () {
    scrollFunction();
};

function scrollFunction() {
    const scrollToTopBtn = document.getElementById('scrollToTopBtn');
    if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        scrollToTopBtn.style.display = 'block';
    } else {
        scrollToTopBtn.style.display = 'none';
    }
}

// Scroll to the top of the document when the button is clicked
document.getElementById('scrollToTopBtn').addEventListener('click', () => {
    document.body.scrollTop = 0; // For Safari
    document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE, and Opera
});
// Smooth scroll to the top of the document when the button is clicked
document.getElementById('scrollToTopBtn').addEventListener('click', () => {
    // Get the current scroll position
    const currentScroll = document.documentElement.scrollTop || document.body.scrollTop;

    // Scroll up with smooth behavior
    window.scrollTo({
        top: 0,
        behavior: 'smooth'
    });
});
