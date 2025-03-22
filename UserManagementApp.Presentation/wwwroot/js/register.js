document.addEventListener("DOMContentLoaded", () => {
    const confirmPasswordInput = document.querySelector('input[name="Input.ConfirmPassword"]');
    const showPasswordIcon = document.querySelectorAll('.fa-eye-slash');

    // Toggle password visibility
    showPasswordIcon.forEach(icon => {
        icon.addEventListener("click", () => {
            const passwordInput = icon.parentElement.previousElementSibling;
            if (passwordInput.type === "password") {
                passwordInput.type = "text";
                icon.classList.replace("fa-eye-slash", "fa-eye");
            } else {
                passwordInput.type = "password";
                icon.classList.replace("fa-eye", "fa-eye-slash");
            }
        });
    });

    // Form validation
    const form = document.getElementById("registerForm");
    form.addEventListener("submit", (event) => {
        const password = document.querySelector('input[name="Input.Password"]').value;
        const confirmPassword = document.querySelector('input[name="Input.ConfirmPassword"]').value;

        if (password !== confirmPassword) {
            event.preventDefault();
            alert("Passwords do not match.");
        }
    });
});