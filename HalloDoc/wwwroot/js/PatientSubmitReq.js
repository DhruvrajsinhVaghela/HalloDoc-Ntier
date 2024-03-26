function load() {
    var light = localStorage.getItem("light");
    if (light == 0) {
        document.body.setAttribute('data-bs-theme', 'dark');
        document.getElementById('moon').classList.add('d-none');
        document.getElementById('sun').classList.remove('d-none');
    }
    else {
        document.body.setAttribute('data-bs-theme', 'light');
        document.getElementById('moon').classList.remove('d-none');
        document.getElementById('sun').classList.add('d-none');
    }
}



function myFunction() {
    var light = localStorage.getItem("light");
    console.log(light);
    if (light == 1) {
        document.body.setAttribute('data-bs-theme', 'dark');
        document.getElementById('moon').classList.add('d-none');
        document.getElementById('sun').classList.remove('d-none');
        localStorage.setItem("light", 0);
    }
    else {
        document.body.setAttribute('data-bs-theme', 'light');
        document.getElementById('moon').classList.remove('d-none');
        document.getElementById('sun').classList.add('d-none');
        localStorage.setItem("light", 1);
    }
}