$(document).ready(function () {
    $.ajax({
        type: "GET",
        url: '/Customer/Portfolio/GetAllProjects',
        datatype: "json",
        success: function (data) {
            debugger;
            console.log(data);
            if (data != null) {
                $('#footer-projects').html(data);
            }
            else {
                alert(data.status);
            }

        },
        error: function () {
            alert("Content load failed.");
        }
    });
});