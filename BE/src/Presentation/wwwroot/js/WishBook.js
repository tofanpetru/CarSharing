$(document).ready(function () {
    isUploadWishBookButtonClicked = true;
    
    //delete wish book
    $(document).on('click', '.deleteWishBookBtn', function (e) {
        e.preventDefault();
        $('#deleteWishBookModal').modal('show');
        var id = $(this).data('id');
        $('#wishBookId').val(id);
    })

    $('#confirmDeleteWishBook').click(function () {
        var wishBookId = $('#wishBookId').val();

        $.ajax({
            type: "DELETE",
            url: "/book/DeleteWishBook",
            data: { id: wishBookId },
            success: function () {
                $('#deleteWishBookModal').modal('hide');
                window.location.reload();
            }
        })
    })
})


$(function () {
    var test = localStorage.userChecked === 'true' ? true : false;
    $('#check-only-my').prop('checked', test);
});

$('#check-only-my').on('change', function () {
    localStorage.userChecked = $(this).is(':checked');
    window.location.reload();
});

function AddNewAuthorWishBookToggle() {
    var input = document.getElementById("insertNewAuthorWishBook");
    var addButton = document.getElementById("addWishBookAuthorButton");
    var dragdownAuthors = document.getElementById("selectAuthorWishBookdiv");
    document.getElementById("err-existingWishBookAuthor").innerHTML = "";
    if (input.style.display === "none") {
        input.style.display = "block";
        addButton.textContent = "Choose existing";
        dragdownAuthors.style.display = "none";
        isExistingAuthorValid = false;
        isAddedAuthorValid = false;
    }
    else {
        input.style.display = "none";
        addButton.textContent = "Add new author";
        dragdownAuthors.style.display = "block";
        document.getElementById("err-addedWishBookAuthor").innerHTML = "";
        document.getElementById("insertNewAuthorWishBook").value = "";
        isAddedAuthorValid = false;
        if (($('#selectAuthorWishBook option').filter(':selected').text()) != "Select the author") {
            isExistingAuthorValid = true;
        }
    }
}
function ValidateExistingWishBookAuthor(existingAuthor) {
    if (!existingAuthor) {
        document.getElementById("err-existingWishBookAuthor").innerHTML = "<span class='warning'>Please select the book author!</span>";
        document.getElementById("err-existingWishBookAuthor").style.color = "#FF0000";
        document.getElementById("selectAuthorWishBook").style.border = "none";
        isExistingAuthorValid = false;
    } else {
        document.getElementById("err-existingWishBookAuthor").innerHTML = "";
        isExistingAuthorValid = true;
    }
}

function validateWishBookTitle(txt) {
    let regAdd = /^([\s.]*([^\s.][\s.]*){3,100})$/
    if (regAdd.test(txt) === false) {
        document.getElementById("err-wishBookTitle").innerHTML = "<span class='warning'>Invalid Book Title!</span>";
        document.getElementById("err-wishBookTitle").style.color = "#FF0000";
        document.getElementById("wishBookTitle").style.border = "none";
        isTitleValid = false;
    } else {
        document.getElementById("err-wishBookTitle").innerHTML = "";
        isTitleValid = true;
    }
}

function ValidateWishBookFullName(author) {
    let regAdd = /^[a-zA-Z]{2,15}(?: [a-zA-Z]{2,15})$/
    if (regAdd.test(author) === false) {
        document.getElementById("err-addedWishBookAuthor").innerHTML = "<span class='warning'>Invalid Author Full Name!</span>";
        document.getElementById("err-addedWishBookAuthor").style.color = "#FF0000";
        document.getElementById("insertNewAuthorWishBook").style.border = "none";
        isAddedAuthorValid = false;
    } else {
        document.getElementById("err-addedWishBookAuthor").innerHTML = "";
        isAddedAuthorValid = true;
    }
}

function GetWishBookAuthor() {
    if (document.getElementById("insertNewAuthorWishBook").style.display === "none") {
        return document.getElementById("selectAuthorWishBook").options[document.getElementById("selectAuthorWishBook").selectedIndex].text;
    }
    else {
        return $("#insertNewAuthorWishBook").val();
    }
}
function DisplayWishBookErrors() {
    if (isTitleValid === false) {
        document.getElementById("err-wishBookTitle").innerHTML = "<span class='warning'>Invalid Book Title!</span>";
        document.getElementById("err-wishBookTitle").style.color = "#FF0000";
        document.getElementById("wishBookTitle").style.border = "none"
    }
    if (isAddedAuthorValid === false && isExistingAuthorValid === false) {
        if (document.getElementById("insertNewAuthorWishBook").style.display === "none") {
            document.getElementById("err-existingWishBookAuthor").innerHTML = "<span class='warning'>Please specify the book author!</span>";
            document.getElementById("err-existingWishBookAuthor").style.color = "#FF0000";
            document.getElementById("selectAuthorWishBook").style.border = "none";
        } else {
            document.getElementById("err-addedWishBookAuthor").innerHTML = "<span class='warning'>Invalid Author Full Name!</span>";
            document.getElementById("err-addedWishBookAuthor").style.color = "#FF0000";
            document.getElementById("insertNewAuthorWishBook").style.border = "none";
        }
    }
}
var wishBookId = 0;
$('#addWishBookBtn').click(function () {
    if (isUploadWishBookButtonClicked) {
        if (((isAddedAuthorValid === true && isExistingAuthorValid === false) ||
            (isAddedAuthorValid === false && isExistingAuthorValid === true)) &&
            isTitleValid === true) {
            $.ajax({
                url: "/Book/AddWishBook",
                type: "POST",
                data: {
                    BookTitle: $('#wishBookTitle').val(),
                    BookAuthor: GetWishBookAuthor()
                },
                success: function (data) {
                    if (data.error === true) {
                        ShowWishBookErrorMessage(data.message);
                    } else {
                        $('#addWishBookModal').modal('hide');
                        window.location.reload();
                    }
                }
            });
        } else {
            DisplayWishBookErrors();
        };
    } else {
        if (((isAddedAuthorValid === true && isExistingAuthorValid === false) ||
            (isAddedAuthorValid === false && isExistingAuthorValid === true)) &&
            isTitleValid === true) {
            $.ajax({
                url: "/Book/EditWishBook",
                type: "PUT",
                data: {
                    Id: wishBookId,
                    BookTitle: $('#wishBookTitle').val(),
                    BookAuthor: GetWishBookAuthor()
                },
                success: function (data) {
                    isWishBookUploaded = true;
                    console.log(data);
                    if (data.error === true) {
                        ShowWishBookErrorMessage(data.message);
                    } else {
                        $('#addWishBookModal').modal('hide');
                        window.location.reload();
                    }
                }
            })
        } else {
            DisplayWishBookErrors();
        }
    }
})

function resetWishBookModal() {
    $('#selectAuthorWishBook').selectpicker('val', '');
    $('#insertNewAuthorWishBook').val('');
    $('#wishBookTitle').val('');
}

function EditBook(author, title, id) {
    isUploadWishBookButtonClicked = false;
    console.log(id);
    wishBookId = id;
    var exist = 0 != $('#selectAuthorWishBook option').filter(function () {
        return $(this).text() === author;
    }).length;
    $('#wishBookTitle').val(title);
    console.log(author);
    validateWishBookTitle(title);
    console.log($('#selectAuthorWishBook option').filter(function () {
        return $(this).text() === author;
    }).val());


    if (!exist) {
        AddNewAuthorWishBookToggle();
        $('#insertNewAuthorWishBook').val(author);
        isAddedAuthorValid = true;
    } else {
        $('#selectAuthorWishBook').selectpicker('val', author);
        isExistingAuthorValid = true;
    }

}
$('#addWishBookModal').on('hidden.bs.modal', function () {
    resetWishBookModal();
    document.getElementById('insertNewAuthorWishBook').style.display = 'none';
    document.getElementById('addWishBookAuthorButton').textContent = "Add new author";
    document.getElementById('selectAuthorWishBookdiv').style.display = "block";
    document.getElementById("err-addedWishBookAuthor").innerHTML = "";
    document.getElementById("WishBookMessageBoxArea").innerHTML = "";
    document.getElementById("err-existingWishBookAuthor").innerHTML = "";
    document.getElementById("err-wishBookTitle").innerHTML = "";
    isExistingAuthorValid = false;
    isAddedAuthorValid = false;
    isTitleValid = false;
});

$('#UploadWishedBook').click(function () {
    isUploadWishBookButtonClicked = true;
})

function ShowWishBookSuccessMessage(message) {
    var errorElements = document.getElementsByClassName("error-message");
    for (i = 0; i < errorElements.length; i++) {
        errorElements[i].innerHTML = "";
    }
    $(".modal-body input").val("");
    document.getElementById("WishBookMessageBoxArea").innerHTML = "";
    $('<br /><div class="alert alert-success" role="alert" style="display: inline-block">' + message + ' </div >').appendTo("#WishBookMessageBoxArea");
    isTitleValid = false;
    isDateValid = false;
    isAddedAuthorValid = false;
}
function ShowWishBookErrorMessage(message) {
    var errorElements = document.getElementsByClassName("error-message");
    for (i = 0; i < errorElements.length; i++) {
        errorElements[i].innerHTML = "";
    }
    document.getElementById("WishBookMessageBoxArea").innerHTML = "";
    $('<br /><div class="alert alert-danger" role="alert" style="display: inline-block">' + message + '</div >').appendTo("#WishBookMessageBoxArea")
    isTitleValid = false;
    isDateValid = false;
    isAddedAuthorValid = false;
}

function UploadThisBook(author, title) {
    $('#title').val(title);
    $('#title').attr('readonly', true);
    validateWishBookTitle(title);
    var exist = 0 != $('#selectAuthor option').filter(function () {
        return $(this).text() === author;
    }).length;
    if (!exist) {
        document.getElementById("insertNewAuthor").style.display = "block";
        document.getElementById("addAuthorButton").textContent = "Choose existing";
        $('#addAuthorButton').attr('disabled', true);
        document.getElementById("selectAuthordiv").style.display = "none";
        isExistingAuthorValid = false;
        isAddedAuthorValid = false;
        $('#insertNewAuthor').val(author);
        $('#insertNewAuthor').attr('readonly', true);
        ValidateWishBookFullName(author);
    } else {
        document.getElementById("insertNewAuthor").style.display = "none";
        document.getElementById("addAuthorButton").textContent = "Add new author";
        $('#addAuthorButton').attr('disabled', true);
        document.getElementById("selectAuthordiv").style.display = "block";
        document.getElementById("err-addedAuthor").innerHTML = "";
        document.getElementById("insertNewAuthor").value = "";
        isAddedAuthorValid = false;
        if (($('#selectAuthor option').filter(':selected').text()) != "Select the author") {
            isExistingAuthorValid = true;
        }
        $('#selectAuthor').selectpicker('val', author);
        $('button[data-id = selectAuthor]').attr('disabled',true);
    }
}