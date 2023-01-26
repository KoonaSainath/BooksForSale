let btnCreateBook = document.querySelector('#btnCreateBook');
btnCreateBook.addEventListener('click', ValidateFile);

function ValidateFile() {
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