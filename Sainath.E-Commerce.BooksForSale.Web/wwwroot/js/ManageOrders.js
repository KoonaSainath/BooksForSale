$(document).ready(function () {
    let ordersDataTable;
    loadOrdersDataTable();
});

function loadOrdersDataTable() {
    ordersDataTable = $('#tableOrders').DataTable({
        "ajax": {
            "url": "Customer/ManageOrders/GetAllOrders"
        }
    });
}