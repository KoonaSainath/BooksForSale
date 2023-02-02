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
                        LINKS
                    `;
                }
            }
        ]
    });
}