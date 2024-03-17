function load() {
    var light = localStorage.getItem("light");
    if (light == 0) {
        document.body.setAttribute('data-bs-theme', 'dark');

    }
    else {
        document.body.setAttribute('data-bs-theme', 'light');

    }
}



function myFunction() {
    var light = localStorage.getItem("light");
    console.log(light);
    if (light == 1) {
        document.body.setAttribute('data-bs-theme', 'dark');

        localStorage.setItem("light", 0);
    }
    else {
        document.body.setAttribute('data-bs-theme', 'light');

        localStorage.setItem("light", 1);
    }
}