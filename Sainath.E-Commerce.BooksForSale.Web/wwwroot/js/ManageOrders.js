$(document).ready(function () {
    let ordersDataTable;
    loadOrdersDataTable();
});

function loadOrdersDataTable() {
    ordersDataTable = $('#tableOrders').DataTable({
        "ajax": {
            "url": "ManageOrders/GetAllOrders"
        },
        "columns": [
            { "data": "orderHeaderId", "width": "15%" },
            { "data": "name", "width": "20%" },
            { "data": "booksForSaleUser.email", "width": "25%" },
            { "data": "phoneNumber", "width": "20%" },
            { "data": "orderStatus", "width": "15%" },
            {
                "data": "orderHeaderId",
                "width": "5%",
                "render": function () {
                    return `
                        <a class="btn btn-primary"><i class="bi bi-info-circle"></i></a>
                    `;
                }
            }
        ],
        "order": []
    });
}