document.addEventListener('DOMContentLoaded', function() {
    const toastContainer = document.querySelector('.toast-container');
    if (toastContainer) {
        const toasts = toastContainer.querySelectorAll('.sonner-toast');
        toasts.forEach(toast => {
            const bsToast = new bootstrap.Toast(toast);
            bsToast.show();
        });
    }
});
