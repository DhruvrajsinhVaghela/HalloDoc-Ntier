function ClassRemover() {
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

        localStorage.setItem("light", 0);
    }
    else {
        document.body.setAttribute('data-bs-theme', 'light');

        localStorage.setItem("light", 1);
    }
}

/*function SearchTable() {
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("myInput");
    filter = input.value.toUpperCase();
    table = document.getElementById("myTable");
    tr = table.getElementsByTagName("tr");
    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[0];
        if (td) {
            txtValue = td.textContent || td.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}*/
/*function load() {
    var light = localStorage.getItem("light");
    if (light == 0) {
        document.body.setAttribute('data-bs-theme', 'dark');

    }
    else {
        document.body.setAttribute('data-bs-theme', 'light');

    }

}*/

function load() {


    $.ajax({
        url: '/Admin/NavTabs',
        method: 'GET',
        data: { nav: 1 },

        success: function (response) {
            $('#main-div').html(response);
        },
        error: function (error) {
            console.log(error);
            console.error('Error in AJAX', error);
        }
    });


    var light = localStorage.getItem("light");
    if (light == 0) {
        document.body.setAttribute('data-bs-theme', 'dark');

    }
    else {
        document.body.setAttribute('data-bs-theme', 'light');

    }

}

function tab(val) {
    $.ajax({
        url: '/Admin/NavTabs',
        method: 'GET',
        data: { nav: val },

        success: function (response) {
            $('#main-div').html(response);
        },
        error: function (error) {
            console.log(error);
            console.error('Error in AJAX', error);
        }
    });
};


