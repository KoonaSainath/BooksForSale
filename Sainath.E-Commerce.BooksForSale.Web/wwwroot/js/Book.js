$(document).ready(function () {
    let booksDataTable;
    loadBooksDataTable();
});

function validateFileExtension() {
    let extensionRegex = /\.png$/;
    let imageFile = document.querySelector('#imageFile').value;
    if (imageFile != '' && !extensionRegex.test(imageFile)) {
        Swal.fire({
            title: 'File upload error!',
            text: 'Please upload an image with ".png" extension only',
            icon: 'error',
            confirmButtonText: 'Okay'
        });
        return false;
    }
    return true;
}

function validateFile() {
    let imageFile = document.querySelector('#imageFile').value;
    let errorText = '';
    let extensionRegex = /\.png$/;
    if (imageFile == '') {
        errorText = 'Please upload an image to create a book';
    } else if (!extensionRegex.test(imageFile)) {
        errorText = 'Please upload an image with ".png" extension only';
    }
    if (errorText != '') {
        Swal.fire({
            title: 'File upload error!',
            text: errorText,
            icon: 'error',
            confirmButtonText: 'Okay'
        });
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
            {
                "data": "bookId",
                "render": function (data) {
                    return `
                        <div class="btn-group">
                            <a class="btn btn-info" href="/Admin/Book/UpsertBook?bookId=${data}">Update</a>
                        </div>
                    `;
                }
            }
        ]
    });
}