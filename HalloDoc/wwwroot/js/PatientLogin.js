let light = 1;
function myFunction() {

    if (light == 1) {
        document.body.style.backgroundColor = "black";
        document.getElementById("terms1").style.color = "white";
        document.getElementById("terms2").style.color = "white";
        document.getElementById("P-Login-heading").style.color = "#08BCEB";
        document.getElementById("darkmode-btn").style.backgroundColor = "black";
        document.getElementById("vr1").style.color = "white";
        light = 0;
        return;
    }
    {
        document.body.style.backgroundColor = "white";
        document.getElementById("terms1").style.color = "#08BCEB";
        document.getElementById("terms2").style.color = "#08BCEB";
        document.getElementById("P-Login-heading").style.color = "black";
        document.getElementById("darkmode-btn").style.backgroundColor = "white";
        document.getElementById("vr1").style.color = "black";
        light = 1;
    }

}
function load() {


    document.getElementById("terms1").style.color = "#08BCEB";
    document.getElementById("terms2").style.color = "#08BCEB";
    document.getElementById("vr1").style.color = "#08BCEB";


}

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

