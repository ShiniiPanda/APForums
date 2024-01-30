function setColors(primaryColor, secondaryColor) {
    document.documentElement.style.setProperty('--color-primary', primaryColor);
    document.documentElement.style.setProperty('--color-secondary', secondaryColor);
}

function toggleSidebar() {
    const sidebar = document.getElementById('sidebar');
    if (sidebar) {
        if (sidebar.classList.contains('hidden')) {
            sidebar.classList.remove('hidden');
        } else {
            sidebar.classList.add('hidden');
        }
    }
}

window.addEventListener('resize', () => {
    const sidebar = document.getElementById('sidebar');
    if (sidebar) {
        if (window.devicePixelRatio > 1.75) {
            sidebar.classList.add('hidden');
        }
    }
});

window.addEventListener('load', () => {
    const sidebar = document.getElementById('sidebar');
    if (sidebar) {
        if (window.devicePixelRatio > 1) {
            sidebar.classList.remove('hidden');
        }
    }
});




