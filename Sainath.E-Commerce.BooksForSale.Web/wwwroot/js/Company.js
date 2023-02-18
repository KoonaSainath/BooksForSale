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
                        <a class="btn btn-danger" onclick="deleteCompanyAJAX(${data})"><i class="bi bi-trash3-fill"></i></a>
                    `;
                }
            }
        ],
        "order": []
    });
}

function deleteCompanyAJAX(companyId) {
    Swal.fire({
        title: 'Are you sure to delete this company?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                "url": `Company/DeleteCompanyApiEndPoint?companyId=${companyId}`,
                "type": "DELETE",
                "success": function (response) {
                    if (response.success) {
                        Swal.fire(
                            'Success',
                            response.message,
                            'success'
                        )
                        companiesDataTable.ajax.reload();
                    } else {
                        Swal.fire(
                            'Failure',
                            response.message,
                            'warning'
                        )
                    }
                }
            })
        }
    })
}