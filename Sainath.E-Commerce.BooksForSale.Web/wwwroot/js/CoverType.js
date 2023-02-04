$(document).ready(function () {
    let coverTypesDataTable;
    loadCoverTypesDataTable();
});

function loadCoverTypesDataTable() {
    coverTypesDataTable = $('#tableCoverTypes').DataTable({
        "ajax": {
            "url": 'CoverType/GetAllCoverTypesApiEndPoint'
        },
        "columns": [
            { "data": "coverTypeName" },
            { "data": "createdDateTime" },
            { "data": "updatedDateTime" },
            {
                "data": "coverTypeId",
                "render": function (data) {
                    return `
                        <div>
                            <a class="btn btn-primary" href="CoverType/UpdateCoverType?coverTypeId=${data}"><i class="bi bi-pencil-square"></i></a>
                            <a class="btn btn-danger" onclick="removeCoverTypeAjax(${data})"><i class="bi bi-trash3-fill"></i></a>
                        </div>
                    `;
                }
            }
        ]
    });
}

function removeCoverTypeAjax(coverTypeId) {
    Swal.fire({
        title: 'Are you sure you want to remove this cover type?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `CoverType/RemoveCoverTypeApiEndPoint?coverTypeId=${coverTypeId}`,
                type: "DELETE",
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.success) {
                        Swal.fire({
                            title: data.message,
                            icon: 'success',
                            confirmButtonText: 'Okay'
                        });
                        coverTypesDataTable.ajax.reload();
                    } else {
                        Swal.fire({
                            title: data.message,
                            icon: 'warning',
                            confirmButtonText: 'Okay'
                        });
                    }
                }
            });
        }
    });
}