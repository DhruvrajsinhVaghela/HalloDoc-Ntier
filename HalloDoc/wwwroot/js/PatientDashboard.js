

function load1() {
    document.body.style.backgroundColor = "#black";
}

let light = 1;
// document.getElementById("vr1").style.color = "black";
function myFunction() {
    console.log(light);
    if (light == 1) {
        document.body.style.backgroundColor = "black";
        document.getElementById("home-tab-pane").style.backgroundColor = "black";
        document.getElementById("home-tab-pane").style.color = "#08BCEB";
        document.getElementById("profile-div").style.backgroundColor = "black";
        document.getElementById("profile-div").style.color = "#08BCEB";
        document.getElementById("main-div").style.backgroundColor = "rgba(153, 145, 145, 0.872)";
        document.getElementById("main-form-div").style.backgroundColor = "rgba(153, 145, 145, 0.872)";


        light = 0;
        return;
    }
    {
        document.body.style.backgroundColor = "#F8f9fa";
        document.getElementById("home-tab-pane").style.backgroundColor = "#F8f9fa";
        document.getElementById("home-tab-pane").style.color = "black";
        document.getElementById("profile-div").style.backgroundColor = "white";
        document.getElementById("profile-div").style.color = "black";
        document.getElementById("main-div").style.backgroundColor = "white";
        document.getElementById("main-form-div").style.backgroundColor = "white";

        light = 1;
    }

}
function updateDropdown(value) {
    document.getElementById("dropdownMenuButton").innerText = value;
}
function load() {
    document.getElementById("save-btn").style.display = "none";
}
function toggleFormFields() {
    console.log("hi");
    // Replace with your actual class or selector
    document.getElementById("myForm").disabled = false;
    document.getElementById("edit-btn").style.display = "none";
    document.getElementById("save-btn").style.display = "block";

    // Toggle the disabled state for each form field

}
function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition);
    } else {
        alert("Geolocation is not supported by this browser.");
    }
}

// Function to display user's location
function showPosition(position) {
    var latitude = position.coords.latitude;
    var longitude = position.coords.longitude;

    // Set latitude and longitude values in input fields
    document.getElementById("latitudeInput").value = latitude;
    document.getElementById("longitudeInput").value = longitude;
}
document.addEventListener('DOMContentLoaded', function () {
    var form = document.getElementById('myForm');
    var editButton = document.getElementById('edit-btn');
    var saveButton = document.getElementById('save-btn');


    editButton.addEventListener('click', function () {
        toggleFormFields(false); // Enable form fields
        editButton.classList.add('d-none'); // Hide edit button
        saveButton.classList.add('block'); // Show save button
    });

    // Event listener for save button
    saveButton.addEventListener('click', function () {
        toggleFormFields(true); // Disable form fields
        editButton.classList.add('block'); // Show edit button
        saveButton.classList.add('d-none'); // Hide save button
    });
});

const getFileData = (myFile) => {
    names = myFile.files;
    let master = "";
    for (i = 0; i < names.length; i++) {
        master = master + "  " + names[i].name;
    }
    document.getElementById("form-label").innerHTML = `${master}`;
}
function hideaccordian() {
    console.log("hide");
    document.getElementById("accordionExample").style.display = "none";
}
function showaccordian() {
    console.log("show");

    document.getElementById("accordionExample").style.display = "block";
}