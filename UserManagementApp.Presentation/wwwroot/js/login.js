document.addEventListener("DOMContentLoaded", () => {
    const passwordInput = document.querySelector('input[type="password"]');
    const showPasswordIcon = document.querySelector('.fa-lock');

    // Toggle password visibility
    showPasswordIcon.addEventListener("click", () => {
        if (passwordInput.type === "password") {
            passwordInput.type = "text";
            showPasswordIcon.classList.replace("fa-lock", "fa-eye-slash");
        } else {
            passwordInput.type = "password";
            showPasswordIcon.classList.replace("fa-eye-slash", "fa-lock");
        }
    });
});