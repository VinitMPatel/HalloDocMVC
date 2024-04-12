﻿var requestedData = {
    selectedStatus: 0,
    selectedType: 0,
    searchedPatient: "",
    searchedProvider: "",
    searchedEmail: "",
    searchedPhone: "",
    requestedPage: 1,
    totalEntity: 3
}

$('#searchBtn').on('click', function () {

    requestedData.selectedStatus = $('#selectedStatus').val();
    requestedData.selectedType = $('#selectedType').val();
    requestedData.searchedPatient = $('#searchedPatient').val();
    requestedData.searchedProvider = $('#searchedProvider').val();
    requestedData.searchedEmail = $('#searchedEmail').val();
    requestedData.searchedPhone = $('#searchedPhone').val();
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

$('#clearBtn').on('click', function () {
    requestedData.selectedStatus = 0;
    requestedData.selectedType = 0;
    requestedData.searchedPatient = "";
    requestedData.searchedProvider = "";
    requestedData.searchedEmail = "";
    requestedData.searchedPhone = "";
    requestedData.requestedPage = 1;
    requestedData.totalEntity = 3;
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
    requestedData.requestedPage = Page;
    var temp = document.getElementById('page-' + Page);
    if (temp != null) {
        temp.style.backgroundColor = "#5dafb2";
        temp.style.color = "white"
    }
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
