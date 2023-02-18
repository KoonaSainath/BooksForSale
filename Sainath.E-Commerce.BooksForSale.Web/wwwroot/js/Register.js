$(document).ready(function () {
    RoleChange();
});

function RoleChange() {
    let companyDiv = $('#companyDiv');
    companyDiv.hide();
    $('#roleDropDown').on('change', function () {
        if ($('#roleDropDown option:selected').val().toUpperCase() == 'COMPANY CUSTOMER') {
            companyDiv.show();
        } else {
            companyDiv.hide();
        }
    });
}