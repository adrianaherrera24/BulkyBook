var dataTable;

$(document).ready(function () {

    var urlParams = new URLSearchParams(location.search);
    var status = urlParams.get('status');

    if (status == "inprocess") {
        loadDataTable(status);
    } else if (status == "completed") {
        loadDataTable(status);
    } else if (status == "pending") {
        loadDataTable(status);
    } else if (status == "approved") {
        loadDataTable(status);
    } else {
        loadDataTable(status);
    }
    
});

function loadDataTable(status) {
    dataTable = $('#orderTbl').DataTable({
        "ajax": {
            "url": "/Admin/Order/GetAll?status=" + status
        },
        "columns": [
            {"data": "id", "Width": "15%"},
            {"data": "name", "Width": "15%"},
            {"data": "phoneNumber", "Width": "15%"},
            {"data": "applicationUser.email", "Width": "15%"},
            {"data": "orderStatus", "Width": "15%"},
            {"data": "orderTotal", "Width": "15%"},
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a class="btn btn-primary mx-2" href="/Admin/Order/Details?orderId=${data}">
                                <i class="bi bi-pencil-square"></i></a>
                        </div> `
                },
                "Width": "10%"
            },
        ]
    });
}