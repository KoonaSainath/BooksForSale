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
            { "data": "createdDateTime" },
            { "data": "updatedDateTime" },
            {
                "data": "categoryId",
                "render": function (data) {
                    return `
                        <div>
                            <a class="btn btn-primary mx-4" href="/Admin/Category/UpdateCategory?categoryId=${data
                        }"><i class="bi bi-pencil-square"></i></a>
                            <a class="btn btn-danger"><i class="bi bi-trash3-fill"></i></a>
                        </div>
                    `;
                }
            }
        ]
    });
}