

$('#selectProfession').on('change', function () {
    var professionType = $(this).val();
    $.ajax({
        url: '/Admin/GetPartnerData',
        data: { "professionType": professionType },
        success: function (response) {
            $('#dataTable').html(response);
        },
        error: function (xhr, status, error) {
            alert("Something went wrong");
        }
    })
})

$('.editProfessionBtn').on('click', function () {
    
})