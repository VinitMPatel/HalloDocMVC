
var professionType = 0;
var searchKey = "";
var i = 0;
$('#professions').on('change', function () {
    professionType = $(this).val();
    PartnerTableAjax(professionType, searchKey)
})

$('.editProfessionBtn').on('click', function () {
    
})

function Search() {
    searchKey = $('#searchKey').val()
    PartnerTableAjax(professionType, searchKey)
}


function PartnerTableAjax(professionType, searchKey) {
    console.log(" common" + i)
    $.ajax({
        url: '/Admin/GetPartnerData',
        data: { "professionType": professionType, "searchKey": searchKey },
        success: function (response) {
            $('#dataTable').html(response);
            i++;
        },
        error: function (xhr, status, error) {
            alert("common ajax");
        }
    })
}

$('.cancelData').on('click', function () {
    $('form span').css('display', 'none');
})

$('.saveData').on('click', function () {
    $('form span').css('display', 'block');
})

if (document.getElementById('phone') != null) {
const phoneInputField = document.querySelector("#phone");
const phoneInput = window.intlTelInput(phoneInputField, {
    utilsScript:
        "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
});
}
