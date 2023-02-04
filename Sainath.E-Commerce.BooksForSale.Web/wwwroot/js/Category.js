$(document).ready(function () {
    let categoriesDataTable;
    console.log('test');
    loadCategoriesDataTable();
});

function loadCategoriesDataTable() {
    categoriesDataTable = $('#tableCategories').DataTable({
        "ajax": {
            "url": "Category/GetAllCategoriesApiEndPoint"
        },
        "columns": [
            { "data": "categoryName" },
            { "data": "displayOrder" },
            { "data": "createdDateTimeString" },
            { "data": "updatedDateTimeString" },
            {
                "data": "categoryId",
                "render": function (data) {
                    return `
                        <div>
                            <a class="btn btn-primary" href="/Admin/Category/UpdateCategory?categoryId=${data
                        }"><i class="bi bi-pencil-square"></i></a>
                            <a class="btn btn-danger" onclick="removeCategoryAjax(${data})"><i class="bi bi-trash3-fill"></i></a>
                        </div>
                    `;
                }
            }
        ]
    });
}

function removeCategoryAjax(categoryId) {
    Swal.fire({
        title: 'Are you sure to remove the category?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: `Category/RemoveCategoryApiEndPoint?categoryId=${categoryId}`,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.success) {
                        categoriesDataTable.ajax.reload();
                        Swal.fire({
                            title: 'Category deleted successfully',
                            icon: 'success',
                            confirmButtonText: 'Okay'
                        });
                    } else {
                        Swal.fire({
                            title: 'Are you sure to remove the category?',
                            icon: 'error',
                            confirmButtonText: 'Okay'
                        });
                    }
                }
            })
        }
    });
}