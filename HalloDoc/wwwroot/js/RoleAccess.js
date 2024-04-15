$('#accountType').on('change', function () {
    var type = $('#accountType').val();
    $.ajax({
        url: '/Admin/RolesList',
        data: { "accountType": type },
        success: function (response) {
            $('#rolesList').html(response);
        },
        error: function (xhr, status, error) {
            alert("Something went wrong");
        }
    })
})

$('#createRoleBtn').on('click', function () {

    var selectedMenu = [];
    $('input[type="checkbox"]:checked').each(function () {
        selectedMenu.push($(this).val());
    });

    if (selectedMenu.length > 0 && $('#nameError').text() == "" && $('#roleName').val() != "") {
        $.ajax({

            url: '/Admin/AddNewRole',
            data: { "menus": selectedMenu, "accountType": $('#accountType').val(), "roleName": $('#roleName').val() },
            type: 'POST',
            async : false,
            success: function (response) {
                var link = document.createElement('a');
                link.href = "/Admin/RoleAccess";
                link.click();
                toastr.success("Role added successfully.")
            },
            error: function (xhr, status, error) {
                alert("2");
            }
        })
    }
    else {
        $('#errorSpan').html("*Please enter credential");

    }
})

$('#cancelBtn').on('click', function () {
    var type = 0;
    $.ajax({
        url: '/Admin/RolesList',
        data: { "accountType": type },
        success: function (response) {
            $('#rolesList').html(response);
        },
        error: function (xhr, status, error) {
            alert("Something went wrong");
        }
    })
    $('#errorSpan').html("")
})

function validation() {
    if ($('input[type="checkbox"]:checked').length > 0) {
        $('#errorSpan').html('');
    }
    else if ($('input[type="checkbox"]:checked').length < 1) {
        $('#errorSpan').html('*Please select atleast one option');
    }
}

$('#roleName').focus(function () {
    $('#errorSpan').html('');
})

$('.editRoleBtn').click(function () {
    var type = 0;
    $.ajax({
        url: '/Admin/EditRole',
        data: { "roleId" : $(this).val()},
        success: function (response) {
            $('#mainContent').html(response);
        },
        error: function (xhr, status, error) {
            alert("1");
        }
    })
})
