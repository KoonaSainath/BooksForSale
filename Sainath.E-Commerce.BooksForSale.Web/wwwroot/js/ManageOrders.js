let processing = 'Processing', pendingPayment = 'ApprovedForDelayedPayment', approved = 'Approved', shipped = 'Shipped', all = 'All';
$(document).ready(function () {
    let ordersDataTable;
    let url = window.location.search.toLowerCase();
    if (url.toLowerCase().includes(processing.toLowerCase())) {
        loadOrdersDataTable(processing);
    } else if (url.toLowerCase().includes(pendingPayment.toLowerCase())) {
        loadOrdersDataTable(pendingPayment);
    } else if (url.toLowerCase().includes(approved.toLowerCase())) {
        loadOrdersDataTable(approved);
    } else if (url.toLowerCase().includes(shipped.toLowerCase())) {
        loadOrdersDataTable(shipped);
    } else {
        loadOrdersDataTable(all);
    }
});

function loadOrdersDataTable(status) {
    if (status == pendingPayment) {
        ordersDataTable = $('#tableOrders').DataTable({
            "ajax": {
                "url": `ManageOrders/GetAllOrders/?status=${status}`
            },
            "columns": [
                { "data": "orderHeaderId", "width": "15%" },
                { "data": "name", "width": "20%" },
                { "data": "booksForSaleUser.email", "width": "25%" },
                { "data": "phoneNumber", "width": "20%" },
                { "data": "paymentStatus", "width": "15%" },
                {
                    "data": "orderHeaderId",
                    "width": "5%",
                    "render": function (data) {
                        return `
                        <a class="btn btn-info" href="ManageOrders/GetOrder?orderHeaderId=${data}"><i class="bi bi-info-circle"></i></a>
                    `;
                    }
                }
            ],
            "order": []
        });
    } else {
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
                    "render": function (data) {
                        return `
                        <a class="btn btn-info" href="ManageOrders/GetOrder?orderHeaderId=${data}"><i class="bi bi-info-circle"></i></a>
                    `;
                    }
                }
            ],
            "order": []
        });
    }

}

function ValidateCarrierAndTrackingNumber() {
    if ($('#carrier').val() == '' || typeof $('#carrier').val() == undefined || typeof $('#carrier').val() == null) {
        Swal.fire({
            icon: 'error',
            title: 'Alert',
            text: 'Please provide carrier details to ship this order'
        });
        return false;
    } else if ($('#trackingNumber').val() == '' || typeof $('#trackingNumber').val() == undefined || typeof $('#trackingNumber').val() == null) {
        Swal.fire({
            icon: 'error',
            title: 'Alert',
            text: 'Please provide tracking number to ship this order'
        });
        return false;
    } else {
        return true;
    }
}