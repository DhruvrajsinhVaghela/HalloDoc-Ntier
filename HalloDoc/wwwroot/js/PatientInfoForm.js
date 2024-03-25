

let size = 0;
function showhide() {

    icon = document.getElementById("eye");
    pw = document.getElementById("floatingPassword");
    if (size == 0) {
        pw.type = "text";
        icon.src = "/images/eye-slash.svg";
        size = 1;
    }
    else {
        pw.type = "password";
        icon.src = "/images/eye.svg";
        size = 0;

    }
}



const load = () => {
    
    const phoneInputField = document.getElementsByClassName("phone");
    for (let i = 0; i < phoneInputField.length; ++i) {
        const phoneInput = window.intlTelInput(phoneInputField[i], {
            utilsScript:
                "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
        });
    }
    let btn = document.getElementById('btn_modal');
    if (btn) {
        btn.setAttribute('data-bs-target', '#exampleModal');
        btn.setAttribute('data-bs-toggle', 'modal');
        btn.click();
    }

    var light = localStorage.getItem("light");
    if (light == 0) {
        document.body.setAttribute('data-bs-theme', 'dark');

    }
    else {
        document.body.setAttribute('data-bs-theme', 'light');

    }
}


// function load(){


async function fetch_email() {
    var email = document.getElementById('email').value;
    var exist;
    await fetch('/Login/ValidateEmail?email=' + email).then(res => res.json()).then(res1 => { exist = res1.exist });
    if (exist == false) {
        document.getElementById('passwordDiv').classList.remove('d-none');
        document.getElementById('cpasswordDiv').classList.remove('d-none');

    }
    else if (exist == true) {
        document.getElementById('passwordDiv').classList.add('d-none');
        document.getElementById('cpasswordDiv').classList.add('d-none');
    }


    console.log(exist);



}

const getFileData = (myFile) => {
    names = myFile.files;
    let master = "";
    for (i = 0; i < names.length; i++) {
        master = master + "  " + names[i].name;
    }
    document.getElementById("form-label").innerHTML = `${master}`;
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

