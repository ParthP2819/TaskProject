var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Category/GetAll"
        },
        "columns": [
            { "data": "name", "width": "25%" },
            { "data": "description", "width": "25%" },
            { "data": "isActive", "width": "25%" },
            {
                "data": "categoryId",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                        <a onclick="showInPopup('/Category/Edit/?id=${data}','Edit Category')"
                        class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                       <a onclick="showInPopup('/Category/Delete/?id=${data}','Delete Category')"
                        class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Delete</a>
                        <a onclick="showInPopup('/Category/AddChild/?id=${data}','Add Child Category')"
                        class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Add Child</a>
					</div>
                        `
                },
                "width": "25%"
            }
        ]
    });
}
function showInPopup(url, title) {
    $.ajax({
        type: "GET",
        url: url,
        success: function (res) {
            $('#modal-popup .modal-body').html(res);
            $('#modal-popup .modal-title').html(title);
            $('#modal-popup').modal('show');
        }
    })
}