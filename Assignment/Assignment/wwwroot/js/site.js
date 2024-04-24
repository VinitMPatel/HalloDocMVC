// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var requestedData = {
    searchKey: "",
    requestedPage: 1,
    totalEntity: 3,
}
function Search() {
    requestedData.searchKey = $('#search').val();
    CommonAjax();
}

function CommonAjax() {
    $.ajax({
        url: '/Home/StudentData',
        data: requestedData,
        success: function (response) {
            $('#studentTable').html(response);
            // var temp = document.getElementById('page-1');
            // if (temp != null) {
            //     temp.style.backgroundColor = "#5dafb2";
            //     temp.style.color = "white"
            // }
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    })
}

$('.addBtn').click(function () {
    $.ajax({
        url: "Home/StudentForm",
        dataType: "html",
        success: function (data) {
            $("#studentForm").html(data);
            const myModal = new bootstrap.Modal("#exampleModal", {});
            myModal.show();
        },
        error: function () {
            alert("No Projects Found");
            $("#studentForm").html('An error has occurred');
        }
    });
})

function EditStudentForm(studentId) {
    var studentId = studentId;
    $.ajax({
        url: "Home/EditStudentForm",
        data: { "studentId": studentId },
        dataType: "html",
        success: function (data) {
            $("#studentForm").html(data);
            const myModal = new bootstrap.Modal("#exampleModal", {});
            myModal.show();
        },
        error: function () {
            alert("No Projects Found");
            $("#studentForm").html('An error has occurred');
        }
    });
}

function DeleteStudent(studentId) {
    var studentId = studentId;
    swal.fire({
        title: "Are you sure?",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes",
        closeOnConfirm: false,
    }).then(function (result) {
        if (result.value) {
            console.log("Yes Btn");
            $.ajax({
                url: '/Home/DeleteStudent',
                data: { "studentId": studentId },
                success: function () {
                    new swal({
                        showCancelButton: false,
                        showConfirmButton: false,
                        title: "Deleted!",
                        text: "Student is deleted.",
                        type: "success",
                        timer: 2000
                    });
                    CommonAjax();
                }
            })
        }
    });
    console.log(studentId);
}


