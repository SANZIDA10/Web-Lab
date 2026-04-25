document.addEventListener("DOMContentLoaded", () => {
    setActiveNavigation();
    updateFooterYear();
    setupContactForm();
});

function setActiveNavigation() {
    const currentPage = window.location.pathname.split("/").pop() || "home.html";
    const navLinks = document.querySelectorAll(".nav-links a");

    navLinks.forEach((link) => {
        const target = link.getAttribute("href");
        if (!target) {
            return;
        }

        const isActive = target === currentPage;
        link.classList.toggle("active", isActive);
        if (isActive) {
            link.setAttribute("aria-current", "page");
        } else {
            link.removeAttribute("aria-current");
        }
    });
}

function updateFooterYear() {
    const footer = document.querySelector(".site-footer p");
    if (!footer) {
        return;
    }

    const currentYear = new Date().getFullYear();
    footer.textContent = footer.textContent.replace(/©\s*\d{4}/, `© ${currentYear}`);
}

function setupContactForm() {
    const form = document.querySelector(".contact-form");
    if (!form) {
        return;
    }

    const successMessage = document.createElement("p");
    successMessage.className = "form-feedback";
    successMessage.hidden = true;
    form.appendChild(successMessage);

    form.addEventListener("submit", (event) => {
        event.preventDefault();

        const nameInput = form.querySelector("#name");
        const emailInput = form.querySelector("#email");
        const messageInput = form.querySelector("#message");

        const name = nameInput?.value.trim() || "";
        const email = emailInput?.value.trim() || "";
        const message = messageInput?.value.trim() || "";

        const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

        if (name.length < 2) {
            showFormFeedback(successMessage, "Please enter your full name.", false);
            return;
        }

        if (!emailPattern.test(email)) {
            showFormFeedback(successMessage, "Please enter a valid email address.", false);
            return;
        }

        if (message.length < 10) {
            showFormFeedback(successMessage, "Your message should be at least 10 characters long.", false);
            return;
        }

        showFormFeedback(successMessage, "Thanks! Your message has been received.", true);
        form.reset();
    });
}

function showFormFeedback(element, message, isSuccess) {
    element.hidden = false;
    element.textContent = message;
    element.classList.toggle("success", isSuccess);
    element.classList.toggle("error", !isSuccess);
}
