$(document).ready(function () {
    //let btnCreateBook = document.querySelector('#btnCreateBook');
    //if (btnCreateBook != null) {
    //    btnCreateBook.addEventListener('click', validateFile);
    //}

    loadBooksDataTable();
})

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
        })
        return false;
    }
    return true;
}

function loadBooksDataTable() {
    $('#tableBooks').DataTable({
        "ajax": {
            "url": "Book/GetAllBooksApiEndPoint"
        },
        "columns": [
            { "data": "title" },
            { "data": "author" },
            { "data": "price" }
        ]
    });
}