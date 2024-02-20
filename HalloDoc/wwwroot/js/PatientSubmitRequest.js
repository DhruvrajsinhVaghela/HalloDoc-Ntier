function load() {
    consol.log("hi");
    document.getElementById("terms1").style.color = "red";
    document.getElementById("terms2").style.color = "#08BCEB";
    document.getElementById("vr1").style.color = "#08BCEB";
    // document.getElementByClassName("darkmode-img").style.backgroundColor="pink";
}



let light = 1;
// document.getElementById("vr1").style.color = "black";
function myFunction() {
    if (light == 1) {
        document.body.style.backgroundColor = "black";
        document.getElementById("terms1").style.color = "#08BCEB";
        document.getElementById("terms2").style.color = "#08BCEB";
        document.getElementById("heading_text").style.color = "#08BCEB";
        document.getElementById("back-btn").style.color = "white";
        document.getElementById("back-btn").style.borderColor="white";
        document.getElementById("vr1").style.color = "#08BCEB";
        document.getElementById("footer").style.backgroundColor="white";
        light = 0;
        return;
    }
    {
        document.body.style.backgroundColor = "white";
        document.getElementById("terms1").style.color = "white";
        document.getElementById("terms2").style.color = "white";
        document.getElementById("heading_text").style.color = "black";
        document.getElementById("back-btn").style.color = "black";
        document.getElementById("back-btn").style.borderColor="black";
        document.getElementById("vr1").style.color = "black";
        document.getElementById("footer").style.backgroundColor="black";

        light = 1;
    }

}
