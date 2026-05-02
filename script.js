document.addEventListener("DOMContentLoaded", () => {
    setActiveNavigation();
    updateFooterYear();
    setupApiForms();
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

function setupApiForms() {
    const forms = document.querySelectorAll("form[data-endpoint]");

    forms.forEach((form) => {
        const feedback = getOrCreateFormFeedback(form);
        const submitButton = form.querySelector('button[type="submit"]');

        form.addEventListener("submit", async (event) => {
            event.preventDefault();

            if (!form.checkValidity()) {
                form.reportValidity();
                return;
            }

            const endpoint = form.dataset.endpoint;
            if (!endpoint) {
                return;
            }

            const payload = Object.fromEntries(new FormData(form).entries());

            setFormBusyState(form, submitButton, true);
            showPendingFeedback(feedback, "Sending your submission...");

            try {
                const response = await fetch(endpoint, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        Accept: "application/json"
                    },
                    body: JSON.stringify(payload)
                });

                const data = await response.json().catch(() => ({}));

                if (!response.ok) {
                    throw new Error(data.message || "We could not submit the form right now.");
                }

                showFormFeedback(feedback, data.message || "Submission received.", true);
                form.reset();
            } catch (error) {
                showFormFeedback(feedback, error instanceof Error ? error.message : "Something went wrong.", false);
            } finally {
                setFormBusyState(form, submitButton, false);
            }
        });
    });
}

function getOrCreateFormFeedback(form) {
    let feedback = form.querySelector(".form-feedback");
    if (!feedback) {
        feedback = document.createElement("p");
        feedback.className = "form-feedback";
        feedback.hidden = true;
        form.appendChild(feedback);
    }

    feedback.setAttribute("aria-live", "polite");
    return feedback;
}

function setFormBusyState(form, submitButton, isBusy) {
    form.classList.toggle("is-submitting", isBusy);

    if (submitButton instanceof HTMLButtonElement) {
        submitButton.disabled = isBusy;
    }
}

function showPendingFeedback(element, message) {
    element.hidden = false;
    element.textContent = message;
    element.classList.remove("success", "error");
}

function showFormFeedback(element, message, isSuccess) {
    element.hidden = false;
    element.textContent = message;
    element.classList.toggle("success", isSuccess);
    element.classList.toggle("error", !isSuccess);
}
