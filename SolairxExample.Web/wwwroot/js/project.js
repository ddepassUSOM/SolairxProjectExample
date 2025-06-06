var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/project/getall'},
        "columns": [
            { data: 'project.projectName', "width": "25%" },
            /*  { data: 'employee.lastName' + ', ' + 'employee.firstName', "width": "25%" },*/
            { data: 'projectManager', "width": "25%" },
            {
                data: 'project.projectId',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                     <a href="/admin/project/upsert?id=${data}" class="btn btn-outline-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>               
                     <a onClick=Delete('/admin/project/delete/${data}') class="btn btn-outline-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
                    </div>`
                },
                "width": "25%"
            }
        ]
    });
}
function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
}
