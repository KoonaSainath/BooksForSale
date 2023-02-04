$(document).ready(function () {
    CKEDITOR.replace('textAreaDescription');
    let booksDataTable;
    loadBooksDataTable();
});

function validateFileExtensionAndTextArea() {
    let errorText = '';
    let titleText = '';
    let extensionRegex = /\.png$/;
    let imageFile = document.querySelector('#imageFile').value;
    let textAreaDescription = document.querySelector('#textAreaDescription');
    CKEDITOR.instances.textAreaDescription.updateElement();
    let description = textAreaDescription.value;
    if (imageFile != '' && !extensionRegex.test(imageFile)) {
        titleText = 'File extension error!';
        errorText = 'Please upload an image with ".png" extension only';
    } else if (description == '' || typeof description == undefined) {
        titleText = 'Book description error!';
        errorText = 'Please enter book description';
    }
    if (errorText != '') {
        Swal.fire({
            title: titleText,
            text: errorText,
            icon: 'error',
            confirmButtonText: 'Okay'
        });
        return false;
    }
    return true;
}

function validateFileAndTextArea() {
    let imageFile = document.querySelector('#imageFile').value;
    let errorText = '';
    let titleText = '';
    let textAreaDescription = document.querySelector('#textAreaDescription');
    CKEDITOR.instances.textAreaDescription.updateElement();
    let description = textAreaDescription.value;
    let extensionRegex = /\.png$/;
    if (imageFile == '') {
        errorText = 'Please upload an image to create a book';
        titleText = 'File error!';
    } else if (!extensionRegex.test(imageFile)) {
        errorText = 'Please upload an image with ".png" extension only';
        titleText = 'File extension error!';
    } else if (description == '' || typeof description == undefined) {
        errorText = 'Please enter book description';
        titleText = 'Book description error!';
    }
    if (errorText != '') {
        Swal.fire({
            title: titleText,
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
            { "data": "createdDateTimeString" },
            { "data": "updatedDateTimeString" },
            {
                "data": "bookId",
                "render": function (data) {
                    return `
                        <div>
                            <a class="btn btn-primary mx-4" href="/Admin/Book/UpsertBook?bookId=${data}"><i class="bi bi-pencil-square"></i></a>
                            <a class="btn btn-danger" onclick="removeBookAjax(${data})"><i class="bi bi-trash3-fill"></i></a>
                        </div>
                    `;
                }
            }
        ]
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
                        Swal.fire({
                            title: 'Deleted!',
                            text: data.message,
                            icon: 'success'
                        });

                        booksDataTable.ajax.reload();
                    } else {
                        Swal.fire({
                            title: 'Error occured!',
                            text: data.message,
                            icon: 'error'
                        });
                    }
                }
            });
        }
    });
}