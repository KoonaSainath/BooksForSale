$(document).ready(function () {
    let textAreaDescription = document.getElementById('Description');
    if (textAreaDescription != null) {
        CKEDITOR.replace(textAreaDescription);
    }
    let booksDataTable;
    loadBooksDataTable();
});

function validateFileExtensionAndTextArea() {
    let errorText = '';
    let titleText = '';
    let imageFile = document.querySelector('#imageFile').value;
    CKEDITOR.instances.Description.updateElement();
    if (imageFile != '' && !isFilePng(imageFile)) {
        titleText = 'File extension error!';
        errorText = 'Please upload an image with ".png" extension only';
    } else if (!isDescriptionValid(document.getElementById('Description').value)) {
        titleText = 'Book description error!';
        errorText = 'Please enter book description';
    }
    if (errorText != '') {
        SweetAlert(titleText, errorText, 'error', 'Okay');
        return false;
    }
    return true;
}

function validateFileAndTextArea() {
    let imageFile = document.querySelector('#imageFile').value;
    let errorText = '';
    let titleText = '';
    CKEDITOR.instances.Description.updateElement();
    if (imageFile == '') {
        errorText = 'Please upload an image to create a book';
        titleText = 'File error!';
    } else if (!isFilePng(imageFile)) {
        errorText = 'Please upload an image with ".png" extension only';
        titleText = 'File extension error!';
    } else if (!isDescriptionValid(document.getElementById('Description').value)) {
        errorText = 'Please enter book description';
        titleText = 'Book description error!';
    }
    if (errorText != '') {
        SweetAlert(titleText, errorText, 'error', 'Okay');
        return false;
    }
    return true;
}

function loadBooksDataTable() {
    booksDataTable = $('#tableBooks').DataTable({
        "ajax": {
            "url": "Book/GetAllBooksApiEndPoint"
        },
        "columns": [
            { "data": "title" },
            { "data": "author" },
            { "data": "price" },
            { "data": "category.categoryName" },
            { "data": "coverType.coverTypeName" },
            { "data": "createdDateTimeString" },
            { "data": "updatedDateTimeString" },
            {
                "data": "bookId",
                "render": function (data) {
                    return `
                        <div>
                            <a class="btn btn-primary" href="/Admin/Book/UpsertBook?bookId=${data}"><i class="bi bi-pencil-square"></i></a>
                            <a class="btn btn-danger" onclick="removeBookAjax(${data})"><i class="bi bi-trash3-fill"></i></a>
                        </div>
                    `;
                }
            }
        ],
        "order": []
    });
}

function removeBookAjax(bookId) {
    Swal.fire({
        title: 'Are you sure you want to delete this book?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `/Admin/Book/RemoveBookApiEndPoint?bookId=${bookId}`,
                type: "DELETE",
                dataType: 'json',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    if (data.success) {
                        SweetAlert('Deleted!', data.message, 'success', 'Okay');
                        booksDataTable.ajax.reload();
                    } else {
                        SweetAlert('Error occured!', data.message, 'error', 'Okay');
                    }
                }
            });
        }
    });
}

function isDescriptionValid(description) {
    return !(description == '' || typeof description == undefined)
}

function isFilePng(fileName) {
    let regex = /\.png$/;
    return regex.test(fileName);
}

function SweetAlert(title, text, iconType, confirmButtonText) {
    Swal.fire({
        title: title,
        text: text,
        icon: iconType,
        confirmButtonText: confirmButtonText
    });
}