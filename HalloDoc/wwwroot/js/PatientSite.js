let light = 1;
function myFunction() {

    if (light == 1) {
        document.body.style.backgroundColor = "black";
        document.getElementById("terms1").style.color = "white";
        document.getElementById("terms2").style.color = "white";
        document.getElementById("darkmode-btn").style.backgroundColor = "black";
        document.getElementById("vr1").style.color = "white";
        light = 0;
        return;
    }
    {
        document.body.style.backgroundColor = "white";
        document.getElementById("terms1").style.color = "#08BCEB";
        document.getElementById("terms2").style.color = "#08BCEB";
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
