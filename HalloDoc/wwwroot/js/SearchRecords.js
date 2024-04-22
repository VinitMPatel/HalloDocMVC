var requestedData = {
    selectedStatus: 0,
    selectedType: 0,
    searchedPatient: "",
    searchedProvider: "",
    searchedEmail: "",
    searchedPhone: "",
    requestedPage: 1,
    totalEntity: 3,
    fromDate,
    toDate,
}

$('#searchBtn').on('click', function () {

    requestedData.selectedStatus = $('#selectedStatus').val();
    requestedData.selectedType = $('#selectedType').val();
    requestedData.searchedPatient = $('#searchedPatient').val();
    requestedData.searchedProvider = $('#searchedProvider').val();
    requestedData.searchedEmail = $('#searchedEmail').val();
    requestedData.searchedPhone = $('#searchedPhone').val();
    requestedData.requestedPage = 1;
    requestedData.fromDate = $('#fromDate').val();
    requestedData.toDate = $('#toDate').val();
    console.log(requestedData.fromDate)
    $.ajax({
        url: '/Admin/SearchRecordTable',
        data: requestedData,
        success: function (response) {
            $('#dataTable').html(response);
            var temp = document.getElementById('page-1');
            if (temp != null) {
                temp.style.backgroundColor = "#5dafb2";
                temp.style.color = "white"
            }
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
})

$('#clearBtn').on('click', function () {
    requestedData.selectedStatus = 0;
    requestedData.selectedType = 0;
    requestedData.searchedPatient = "";
    requestedData.searchedProvider = "";
    requestedData.searchedEmail = "";
    requestedData.searchedPhone = "";
    requestedData.requestedPage = 1;
    requestedData.totalEntity = 3;
    requestedData.fromDate = "",
        requestedData.toDate = "",
    $.ajax({
        url: '/Admin/SearchRecordTable',
        data: requestedData,
        success: function (response) {
            $('#dataTable').html(response);
            var temp = document.getElementById('page-1');
            if (temp != null) {
                temp.style.backgroundColor = "#5dafb2";
                temp.style.color = "white"
            }
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
})

function ChangePage(Page) {
    debugger
    requestedData.requestedPage = Page;
    var temp = document.getElementById('page-' + Page);
    if (temp != null) {
        temp.style.backgroundColor = "#5dafb2";
        temp.style.color = "white"
    }
    debugger
    $.ajax({
        url: '/Admin/SearchRecordTable',
        data: requestedData,
        success: function (response) {
            $('#dataTable').html(response);
            var temp = document.getElementById('page-' + Page);
            if (temp != null) {
                temp.style.backgroundColor = "#5dafb2";
                temp.style.color = "white"
            }
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}

$('#selectedEntity').on('change', function () {
    requestedData.totalEntity = $(this).val();
    requestedData.requestedPage = 1;
    $.ajax({
        url: '/Admin/SearchRecordTable',
        data: requestedData,
        success: function (response) {
            $('#dataTable').html(response);
            var temp = document.getElementById('page-1');
            if (temp != null) {
                temp.style.backgroundColor = "#5dafb2";
                temp.style.color = "white"
            }
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
})

$('#exportBtn').on('click', function () {
    debugger;
    var link = document.createElement('a');
    link.href = '/Admin/ExportSearchRecordData?selectedStatus=' + requestedData.selectedStatus + "&selectedType=" + requestedData.selectedType + "&searchedPatient=" + requestedData.searchedPatient + "&searchedProvider=" + requestedData.searchedProvider + "&searchedPhone=" + requestedData.searchedPhone + "&searchedEmail=" + requestedData.searchedEmail;
    //link.href = '/Admin/ExportData?obj=' + param;
    link.style.display = 'none';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
});

var PatientHistoryData = {
    firstName : "",
    lastName : "",
    email : "",
    mobile : ""
}

$('#searchBtnPatientHistory').on('click', function () {

    PatientHistoryData.firstName = $('#firstName').val();
    PatientHistoryData.lastName = $('#lastName').val();
    PatientHistoryData.email = $('#email').val();
    PatientHistoryData.mobile = $('#phone').val();

    $.ajax({
        url: '/Admin/GetPatientHistory',
        data: PatientHistoryData,
        success: function (response) {
            $('#dataTable').html(response);
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
})

$('#clearBtnPatientHistory').on('click', function () {
    $.ajax({
        url: '/Admin/GetPatientHistory',
        success: function (response) {
            $('#dataTable').html(response);
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
})


var requestedBlockData = {
    name: "",
    email: "",
    phone: "",
    requestedPage: 1,
    totalEntity: 3
}

$('#searchBlockedBtn').click(function () {
   
    requestedBlockData.name = $('#blockedPatient').val();
    requestedBlockData.email = $('#blockedEmail').val();
    requestedBlockData.phone = $('#blockedPhone').val();
    requestedData.requestedPage = 1;

    $.ajax({
        url: '/Admin/BlockHistoryData',
        data: requestedBlockData,
        success: function (response) {
            $('#dataTable').html(response);
            var temp = document.getElementById('page-1');
            if (temp != null) {
                temp.style.backgroundColor = "#5dafb2";
                temp.style.color = "white"
            }
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
})

$('#clearBlockedBtn').on('click', function () {
  
    requestedBlockData.name = "";
    requestedBlockData.email = "";
    requestedBlockData.phone = "";
    requestedBlockData.requestedPage = 1;
    requestedBlockData.totalEntity = 3;
    $.ajax({
        url: '/Admin/BlockHistoryData',
        data: requestedBlockData,
        success: function (response) {
            $('#dataTable').html(response);
            var temp = document.getElementById('page-1');
            if (temp != null) {
                temp.style.backgroundColor = "#5dafb2";
                temp.style.color = "white"
            }
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
})


function ChangeBlockDataPage(Page) {
    requestedBlockData.requestedPage = Page;
    var temp = document.getElementById('page-' + Page);
    if (temp != null) {
        temp.style.backgroundColor = "#5dafb2";
        temp.style.color = "white"
    }
    $.ajax({
        url: '/Admin/BlockHistoryData',
        data: requestedBlockData,
        success: function (response) {
            $('#dataTable').html(response);
            var temp = document.getElementById('page-' + Page);
            if (temp != null) {
                temp.style.backgroundColor = "#5dafb2";
                temp.style.color = "white"
            }
        },
        error: function (xhr, status, error) {
            console.error(error);
        }
    });
}