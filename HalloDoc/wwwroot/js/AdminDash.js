﻿function ClassRemover() {
    if (document.getElementById('arw1').classList.contains('NewStateArw')) {
        document.getElementById('new-tab').classList.remove('NewStateBg');
        document.getElementById('arw1').classList.remove('NewStateArw', "d-sm-block");
    }
    if (document.getElementById('arw2').classList.contains('PendingStateArw')) {
        document.getElementById('Pending-tab').classList.remove('PendingStateBg');
        document.getElementById('arw2').classList.remove('PendingStateArw', "d-sm-block");
    }
    if (document.getElementById('arw3').classList.contains('ActiveStateArw')) {
        document.getElementById('Active-tab').classList.remove('ActiveStateBg');
        document.getElementById('arw3').classList.remove('ActiveStateArw', "d-sm-block");
    }
    if (document.getElementById('arw4').classList.contains('ConcludeStateArw')) {
        document.getElementById('Conclude-tab').classList.remove('ConcludeStateBg');
        document.getElementById('arw4').classList.remove('ConcludeStateArw', "d-sm-block");
    }
    if (document.getElementById('arw5').classList.contains('CloseStateArw')) {
        document.getElementById('ToClose-tab').classList.remove('CloseStateBg');
        document.getElementById('arw5').classList.remove('CloseStateArw', "d-sm-block");
    }
    if (document.getElementById('arw6').classList.contains('UnpaidStateArw')) {
        document.getElementById('Unpaid-tab').classList.remove('UnpaidStateBg');
        document.getElementById('arw6').classList.remove('UnpaidStateArw', "d-sm-block");
    }
}
function NewTab() {
    console.log("hello");
    ClassRemover()
    document.getElementById("new-tab").classList.add("NewStateBg");
    document.getElementById("arw1").classList.add("NewStateArw", "d-sm-block");
}

function PendingTab() {
    ClassRemover()
    document.getElementById("Pending-tab").classList.add("PendingStateBg");
    document.getElementById("arw2").classList.add("PendingStateArw", "d-sm-block");
}

function ActiveTab() {
    ClassRemover()
    document.getElementById("Active-tab").classList.add("ActiveStateBg");
    document.getElementById("arw3").classList.add("ActiveStateArw", "d-sm-block");
}

function ConcludeTab() {
    ClassRemover()
    document.getElementById("Conclude-tab").classList.add("ConcludeStateBg");
    document.getElementById("arw4").classList.add("ConcludeStateArw", "d-sm-block");
}

function CloseTab() {
    ClassRemover()
    document.getElementById("ToClose-tab").classList.add("CloseStateBg");
    document.getElementById("arw5").classList.add("CloseStateArw", "d-sm-block");
}

function UnpaidTab() {
    ClassRemover()
    document.getElementById("Unpaid-tab").classList.add("UnpaidStateBg");
    document.getElementById("arw6").classList.add("UnpaidStateArw", "d-sm-block");
}

//var light = 1;
function DarkMode() {
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

document.addEventListener('DOMContentLoaded', function () {
    // Find all elements with the class "go-back-button"
    var goBackButtons = document.querySelectorAll('.go-back-button');

    // Attach click event listener to each button
    goBackButtons.forEach(function (button) {
        button.addEventListener('click', function () {
            window.history.back();
        });
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
const getFileDataProvider1 = (myFile) => {
    names = myFile.files;
    let master = "";
    for (i = 0; i < names.length; i++) {
        master = master + "  " + names[i].name;
    }
    document.getElementById("form-label-1").innerHTML = `${master}`;

    if (document.getElementById('FileCheck1').files.length > 0)
    {
        document.getElementById('CheckBox1').checked = true;
    }
    else {
        document.getElementById('CheckBox1').checked = false;
    }
}
const getFileDataProvider2 = (myFile) => {
    names = myFile.files;
    let master = "";
    for (i = 0; i < names.length; i++) {
        master = master + "  " + names[i].name;
    }
    document.getElementById("form-label-2").innerHTML = `${master}`;

    if (document.getElementById('FileCheck2').files.length > 0) {
        document.getElementById('CheckBox2').checked = true;
    }
    else {
        document.getElementById('CheckBox2').checked = false;
    }
}
const getFileDataProvider3 = (myFile) => {
    names = myFile.files;
    let master = "";
    for (i = 0; i < names.length; i++) {
        master = master + "  " + names[i].name;
    }
    document.getElementById("form-label-3").innerHTML = `${master}`;

    if (document.getElementById('FileCheck3').files.length > 0) {
        document.getElementById('CheckBox3').checked = true;
    }
    else {
        document.getElementById('CheckBox3').checked = false;
    }
}
const getFileDataProvider4 = (myFile) => {
    names = myFile.files;
    let master = "";
    for (i = 0; i < names.length; i++) {
        master = master + "  " + names[i].name;
    }
    document.getElementById("form-label-4").innerHTML = `${master}`;

    if (document.getElementById('FileCheck4').files.length > 0) {
        document.getElementById('CheckBox4').checked = true;
    }
    else {
        document.getElementById('CheckBox4').checked = false;
    }
}