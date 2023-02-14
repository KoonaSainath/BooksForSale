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
            { "data": "updatedDateTimeString" }
        ],
        "order": []
    });
}