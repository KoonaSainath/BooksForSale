$(document).ready(() => {
    let comapniesDataTable;
    loadCompaniesDataTable();
});

function loadCompaniesDataTable() {
    companiesDataTable = $('#tableCompanies').DataTable({
        "ajax": {
            "url": "Company/GetAllCompanies"
        },
        "columns": [
            { "data": "companyName" },
            { "data": "phoneNumber" },
            { "data": "streetAddress" },
            { "data": "city" },
            { "data": "state" },
            { "data": "postalCode" },
            { "data": "createdDateTimeString" },
            { "data": "updatedDateTimeString" },
            {
                "data": "companyId",
                "render": function (data) {
                    return `
                        <a class="btn btn-primary" href="Company/UpsertCompany?companyId=${data}"><i class="bi bi-pencil-square"></i></a>
                        <a class="btn btn-danger"><i class="bi bi-trash3-fill"></i></a>
                    `;
                }
            }
        ],
        "order": []
    });
}