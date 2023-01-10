var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#companyTbl').DataTable({
        "ajax": {
            "url": "/Admin/Company/GetAll"
        },
        "columns": [
            {"data": "name", "Width": "15%"},
            {"data": "streetAddress", "Width": "15%"},
            {"data": "city", "Width": "15%"},
            {"data": "state", "Width": "15%"},
            {"data": "phoneNumber", "Width": "15%"},
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a class="btn btn-primary mx-2" href="/Admin/Company/Upsert?id=${data}">
                                <i class="bi bi-pencil-square"></i>Edit
                            </a>
                            <a class="btn btn-danger mx-2" onclick=Delete('/Admin/Company/Delete/'+${data})>
                                <i class="bi bi-trash-fill"></i>Delete
                            </a>
                        </div> `
                },
                "Width": "10%"
            },
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
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    } else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}