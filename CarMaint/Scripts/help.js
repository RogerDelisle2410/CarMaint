document.addEventListener("DOMContentLoaded", function () {

    window.showHelp = function () {
        let modal = document.getElementById("helpModal");
        if (!modal) return;

        let lang = document.cookie.replace(/(?:(?:^|.*;\s*)lang\s*\=\s*([^;]*).*$)|^.*$/, "$1") || "en";

        fetch(`/Lang/${lang}.json`)
            .then(response => response.json())
            .then(data => {
                document.getElementById("helpTitle").innerText = data.help_title;
                document.getElementById("helpText").innerText = data.help_text;
                modal.style.display = "block";
            });
    };

    window.closeHelp = function () {
        let modal = document.getElementById("helpModal");
        if (!modal) return;
        modal.style.display = "none";
    };
});
