document.addEventListener("DOMContentLoaded", () => {

    const codeBlocks = document.querySelectorAll(".note-card pre");

    codeBlocks.forEach((pre) => {

        // Contenitore del blocco di codice
        const container = document.createElement("div");
        container.classList.add("code-block");

        pre.parentNode.insertBefore(container, pre);
        container.appendChild(pre);

        // Pulsante copia
        const button = document.createElement("button");
        button.classList.add("copy-button");
        button.textContent = "Copia";

        container.appendChild(button);

        button.addEventListener("click", async () => {

            const code = pre.querySelector("code");

            if (!code) {
                return;
            }

            try {

                await navigator.clipboard.writeText(code.innerText);

                button.textContent = "Copiato!";
                button.classList.add("copied");

                setTimeout(() => {
                    button.textContent = "Copia";
                    button.classList.remove("copied");
                }, 1500);

            } catch (error) {

                button.textContent = "Errore";

                setTimeout(() => {
                    button.textContent = "Copia";
                }, 1500);

            }

        });

    });

});