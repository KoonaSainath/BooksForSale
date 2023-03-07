$(document).ready(function () {
    let processing = 'processing', pendingPayment = 'approvedfordelayedpayment', approved = 'approved', shipped = 'shipped', all = 'all';
    let ordersDataTable;
    let url = window.location.search.toLowerCase();
    if (url.includes(processing)) {
        loadOrdersDataTable(processing);
    } else if (url.includes(pendingPayment)) {
        loadOrdersDataTable(pendingPayment);
    } else if (url.includes(approved)) {
        loadOrdersDataTable(approved);
    } else if (url.includes(shipped)) {
        loadOrdersDataTable(shipped);
    } else {
        loadOrdersDataTable(all);
    }
});

function loadOrdersDataTable(status) {
    ordersDataTable = $('#tableOrders').DataTable({
        "ajax": {
            "url": `ManageOrders/GetAllOrders/?status=${status}`
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
                        <a class="btn btn-info"><i class="bi bi-info-circle"></i></a>
                    `;
                }
            }
        ],
        "order": []
    });
}