



function myFunction() {
    var light = localStorage.getItem("light");
    console.log(light);
    if (light == 1) {
        document.body.setAttribute('data-bs-theme', 'dark');
        document.getElementById('main-div').classList.remove('bg-light');
        document.getElementById('myTabContent').classList.remove('bg-light');
        document.getElementById('moon').classList.add('d-none');
        document.getElementById('sun').classList.remove('d-none');
        localStorage.setItem("light", 0);
    }
    else {
        document.body.setAttribute('data-bs-theme', 'light');
        document.getElementById('main-div').classList.add('bg-light');
        document.getElementById('myTabContent').classList.add('bg-light');
        document.getElementById('moon').classList.remove('d-none');
        document.getElementById('sun').classList.add('d-none');
        localStorage.setItem("light", 1);
    }

}
function updateDropdown(value) {
    document.getElementById("dropdownMenuButton").innerText = value;
}
function load() {
    document.getElementById("save-btn").style.display = "none";
    var light = localStorage.getItem("light");
    if (light == 0) {
        document.body.setAttribute('data-bs-theme', 'dark');
        document.getElementById('main-div').classList.remove('bg-light');
        document.getElementById('myTabContent').classList.remove('bg-light');
        document.getElementById('moon').classList.add('d-none');
        document.getElementById('sun').classList.remove('d-none');
    }
    else {
        document.body.setAttribute('data-bs-theme', 'light');
        document.getElementById('main-div').classList.add('bg-light');
        document.getElementById('myTabContent').classList.add('bg-light');
        document.getElementById('moon').classList.remove('d-none');
        document.getElementById('sun').classList.add('d-none');
    }
}
function toggleFormFields() {
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
    document.getElementById("accordionExample").style.display = "none";


}
function showaccordian() {

    document.getElementById("accordionExample").style.display = "block";
}

