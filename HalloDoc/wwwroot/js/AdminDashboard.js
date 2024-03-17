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



function load() {
    var light = localStorage.getItem("light");
    if (light == 0) {
        document.body.setAttribute('data-bs-theme', 'dark');

    }
    else {
        document.body.setAttribute('data-bs-theme', 'light');

    }

}
//var light = 1;
function DarkMode() {
    var light = localStorage.getItem("light");
    if (light == 1) {
        document.body.setAttribute('data-bs-theme', 'dark');

        localStorage.setItem("light", 0);
    }
    else {
        document.body.setAttribute('data-bs-theme', 'light');

        localStorage.setItem("light", 1);
    }
}
$(document).ready(function () {
    var Action = 1;
    var box = "";
    $.ajax
        ({
            url: '/Admin/CardsView',
            method: 'POST',
            data: { status: Action },
            success: function (response) {
                $('#Content').html(response);
                $('#PageName').html('<h4>Patients</h4><strong class="tabname mb-1">(New)</strong>');
            },
            error: function (error) {
                console.error('Error in AJAX', error);
            }
        })

    $('.Cards').click(function () {
        var cardValue = $(this).attr('value');
        switch (cardValue) {
            case '1': Action = 1; box = 'New'; break;
            case '2': Action = 2; box = 'Pending'; break;
            case '3': Action = 3; box = 'Active'; break;
            case '4': Action = 4; box = 'Conclude'; break;
            case '5': Action = 5; box = 'To Close'; break;
            case '6': Action = 6; box = 'Unpaid'; break;
        }

        $.ajax
            ({
                url: '/Admin/CardsView',
                method: 'POST',
                data: { status: Action },
                success: function (response) {
                    $('#Content').html(response);
                    $('#PageName').html('<h4>Patients</h4><strong class="tabname mb-1">(' + box + ')</strong>');
                },
                error: function (error) {
                    console.error('Error in AJAX', error);
                }
            })
    });


    $('.Person').click(function () {
        var cardValue = $(this).attr('value');
        switch (cardValue) {
            case '0': act = 0; break;
            case '1': act = 1; break;
            case '2': act = 2; break;
            case '3': act = 3; break;
            case '4': act = 4; break;

        }

        $.ajax({
            url: '/Admin/CardsView',
            method: 'POST',
            data: {
                status: Action,
                Id: act
            },
            success: function (response) {
                $('#Content').html(response);
            },
            error: function (error) {
                console.error('Error in AJAX', error);
            }
        })
        console.log(Action);
    });




});

function SearchTable() {
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
}
