$(document).ready(function () {
    isAddedAuthorValid = false;
    isFirstNameValid = false;
    isLastNameValid = false;
    isEditedAuthorValid = true;
    isTitleValid = false;
    isLanguageValid = false;
    isGenreValid = false;
    isExistingAuthorValid = false;
    isCoverValid = true;
    isDateValid = false;
    isUploaded = false;
    isWishBookUploaded = false;
    SetMaxDate("setDate");
    $(".caret").remove();
    $(".DisabledInputs").css("cursor", "default");
    var userTabContent = document.getElementById("userTabContent");
    if (userTabContent != null)
        userTabContent.style.display = "block";
    $('#exampleModal2').on('hidden.bs.modal', function () {
        if (isUploaded)
            window.location = "?section=myBooksSection";
        $('#insertNewAuthor').removeAttr('readonly');
        $('button[data-id = selectAuthor]').removeAttr('disabled');
        $('#title').removeAttr('readonly');
        $('#addAuthorButton').removeAttr('disabled');
    })

    $('#extendReason').bind('input propertychange', function (e) {
        e.target.value = e.target.value.replace(/(\r\n|\n|\r)/gm, "");
        document.getElementById('err-extendReason').innerText = isExtendReasonValid(e.target) || e.target.value == "" ? "": "Extend reason can have minimum 5 text characters";
    });

    $('#extendSubmitButton').on('click', function () {
        if (isExtendReasonValid(document.getElementById('extendReason')))
        {
            var extendAssignmentDTO = {
                AssignmentId: document.getElementById("extendIdInput").value,
                EndDate: document.getElementById("extendEndDateInput").value,
                Reason: document.getElementById("extendReason").value
            }

            $.ajax({
                type: "POST",
                url: "/Book/ExtendAssignment",
                data: extendAssignmentDTO,
                success: function () {
                    location.reload();
                },
                error: function () {
                    document.getElementById('err-extendReason').innerText = "Bad request. Enter details correctly";
                }
            });
        }
    });

})
$(".caret").remove()
//Check if extend reason is valid
function isExtendReasonValid(el) {
    return (/[\ -z]{5,100}$/.test(el.value.replace(/\s+/g, ' ').trimEnd()));
}

function SetToTrue() {
    isGenreValid = true;
    isLanguageValid = true;
    isExistingAuthorValid = true;
    isTitleValid = true;
    isDateValid = true;
}
//Title Validation
function validateTitle(txt) {
    let regAdd = /^([\s.]*([^\s.][\s.]*){3,100})$/
    if (regAdd.test(txt) === false) {
        document.getElementById("err-title").innerHTML = "<span class='warning'>Invalid Game Title!</span>";
        document.getElementById("err-title").style.color = "#FF0000";
        document.getElementById("title").style.border = "none";
        isTitleValid = false;
    } else {
        document.getElementById("err-title").innerHTML = "";
        isTitleValid = true;
    }
}

//First Name Validation
function validateFirstName(firstN) {
    let regAdd = /^[a-zA-Z]{2,15}$/
    if (regAdd.test(firstN) === false) {
        document.getElementById("err-firstName").innerHTML = "<span class='warning'>Invalid First Name!</span>";
        document.getElementById("err-firstName").style.color = "#FF0000";
        document.getElementById("firstName").style.border = "none";
        isFirstNameValid = false;
    } else {
        document.getElementById("err-firstName").innerHTML = "";
        isFirstNameValid = true;
    }
}

//Last Name Validation
function validateLastName(lastN) {
    let regAdd = /^[a-zA-Z]{2,15}$/
    if (regAdd.test(lastN) === false) {
        document.getElementById("err-lastName").innerHTML = "<span class='warning'>Invalid Last Name!</span>";
        document.getElementById("err-lastName").style.color = "#FF0000";
        document.getElementById("lastName").style.border = "none";
        isLastNameValid = false;
    } else {
        document.getElementById("err-lastName").innerHTML = "";
        isLastNameValid = true;
    }
}

//Language Validation
function validateLanguage(language) {
    if (!language) {
        document.getElementById("err-language").innerHTML = "<span class='warning'>Select the book language!</span>";
        document.getElementById("err-language").style.color = "#FF0000";
        document.getElementById("selectLanguage").style.border = "none";
        isLanguageValid = false;
    } else {
        document.getElementById("err-language").innerHTML = "";
        isLanguageValid = true;
    }
}
//Genre Validation
function ValidateGenre(genre) {
    if (!genre) {
        document.getElementById("err-genre").innerHTML = "<span class='warning'>Select the book genres!</span>";
        document.getElementById("err-genre").style.color = "#FF0000";
        document.getElementById("selectGenre").style.border = "none"
        isGenreValid = false;
    } else {
        document.getElementById("err-genre").innerHTML = "";
        isGenreValid = true;
    }
}

function SetMaxDate(inputId) {
    var input = document.getElementById(inputId);
    if (input != null)
        input.setAttribute("max", formatDate(Date()));
}

function GetAllSelectedValues(selectId) {
    var selected = [];
    for (var option of document.getElementById(selectId).options)
    {
        if (option.selected) {
            selected.push(option.value);
        }
    }
    return selected;
}
//Author Validation (either it is new author or an existing one)
function ValidateFullName(author) {
    let regAdd = /^[a-zA-Z]{2,15}(?: [a-zA-Z]{2,15})$/
    if (regAdd.test(author) === false) {
        document.getElementById("err-addedAuthor").innerHTML = "<span class='warning'>Invalid Author Full Name!</span>";
        document.getElementById("err-addedAuthor").style.color = "#FF0000";
        document.getElementById("insertNewAuthor").style.border = "none";
        isAddedAuthorValid = false;
    } else {
        document.getElementById("err-addedAuthor").innerHTML = "";
        isAddedAuthorValid = true;
    }
}

function ValidateEditedAuthorName(author, id) {
    let regAdd = /^[a-zA-Z]{2,15}(?: [a-zA-Z]{2,15})$/
    if (regAdd.test(author) === false) {
        document.getElementById("err-addedAuthor"+"-"+id).innerHTML = "<span class='warning'>Invalid Author Full Name!</span>";
        document.getElementById("err-addedAuthor"+"-"+id).style.color = "#FF0000";
        document.getElementById(id).style.border = "none";
        isEditedAuthorValid = false;
    } else {
        isEditedAuthorValid = true;
        document.getElementById("err-addedAuthor"+"-"+id).innerHTML = "";
    }
}

function ValidateExistingAuthor(existingAuthor) {
    if (!existingAuthor) {
        document.getElementById("err-existingAuthor").innerHTML = "<span class='warning'>Please select the book author!</span>";
        document.getElementById("err-existingAuthor").style.color = "#FF0000";
        document.getElementById("selectAuthor").style.border = "none";
        isExistingAuthorValid = false;
    } else {
        document.getElementById("err-existingAuthor").innerHTML = "";
        isExistingAuthorValid = true;
    }
}

function AddNewAuthorToggle() {
    var input = document.getElementById("insertNewAuthor");
    var addButton = document.getElementById("addAuthorButton");
    var dragdownAuthors = document.getElementById("selectAuthordiv");
    document.getElementById("err-existingAuthor").innerHTML = "";
    if (input.style.display === "none") {
        input.style.display = "block";
        addButton.textContent = "Choose existing";
        dragdownAuthors.style.display = "none";
        isExistingAuthorValid = false;
        isAddedAuthorValid = false;
    }
    else {
        input.style.display = "none";
        addButton.textContent = "Add new company";
        dragdownAuthors.style.display = "block";
        document.getElementById("err-addedAuthor").innerHTML = "";
        document.getElementById("insertNewAuthor").value = "";
        isAddedAuthorValid = false;
        if (($('#selectAuthor option').filter(':selected').text()) != "Select the author") {
            isExistingAuthorValid = true;
        }
    }
}

function GetBookAuthor() {
    if (document.getElementById("insertNewAuthor").style.display === "none") {
        return document.getElementById("selectAuthor").options[document.getElementById("selectAuthor").selectedIndex].text;
    }
    else {
        return $("#insertNewAuthor").val();
    }
}

function GetUpdatedBookAuthor() {
    return document.getElementById("selectAuthor").options[document.getElementById("selectAuthor").selectedIndex].text;
}
//Book Cover Validation
var _validFileExtensions = [".jpg", ".jpeg", ".png"];
function ValidateBookCover(oInput) {
    if (oInput.type == "file") {
        var sFileName = oInput.value;
        if (sFileName.length > 0) {
            var blnValid = false;
            for (var j = 0; j < _validFileExtensions.length; j++) {
                var sCurExtension = _validFileExtensions[j];
                if (sFileName.substr(sFileName.length - sCurExtension.length, sCurExtension.length).toLowerCase() == sCurExtension.toLowerCase()) {
                    blnValid = true;
                    break;
                }
            }
            if (!blnValid) {
                document.getElementById("err-cover").innerHTML = "<span class='warning'>Please select a valid image!</span>";
                document.getElementById("err-cover").style.color = "#FF0000";
                document.getElementById("imageFile").style.border = "none";
                oInput.value = "";
                isCoverValid = false;
            }
        }
    }
    if (document.getElementById('imageFile').files[0].size > 1024000) {
        document.getElementById("err-cover").innerHTML = "<span class='warning'>The file must not be larger than 1MB!</span>";
        document.getElementById("err-cover").style.color = "#FF0000";
        document.getElementById("imageFile").style.border = "none";
        oInput.value = "";
        isCoverValid = false;
    } else {
        document.getElementById("err-cover").innerHTML = "";
        isCoverValid = true;
    }
}

function fileToByteArray(file) {
    return new Promise((resolve, reject) => {
        try {
            let reader = new FileReader();
            let fileByteArray = [];
            reader.readAsArrayBuffer(file);
            reader.onloadend = (evt) => {
                if (evt.target.readyState == FileReader.DONE) {
                    let arrayBuffer = evt.target.result,
                        array = new Uint8Array(arrayBuffer);
                    for (byte of array) {
                        fileByteArray.push(byte);
                    }
                }
                resolve(fileByteArray);
            }
        }
        catch (e) {
            reject(e);
        }
    })
}

//Validate User Profile Avatar
async function ValidateUserAvatar(oInput) {
    document.getElementById("err-avatar").innerHTML = "";

    if (oInput.type == "file") {
        var sFileName = oInput.value;
        if (sFileName.length > 0) {
            var blnValid = false;
            for (var j = 0; j < _validFileExtensions.length; j++) {
                var sCurExtension = _validFileExtensions[j];
                if (sFileName.substr(sFileName.length - sCurExtension.length, sCurExtension.length).toLowerCase() == sCurExtension.toLowerCase()) {
                    blnValid = true;
                    break;
                }
            }
            if (!blnValid) {
                document.getElementById("err-avatar").innerHTML = "<span class='warning' style='color:red'>Please select a valid image!</span>";
                document.getElementById("err-avatar").style.color = "#FF0000";
                document.getElementById("file").style.border = "none";
                oInput.value = "";
            }
        }
    }
    if (document.getElementById('file').files[0].size > 1024000) {
        document.getElementById("err-avatar").innerHTML = "<span class='warning' style='color:red'>The file must not be larger than 1MB!</span>";
        document.getElementById("err-avatar").style.color = "#FF0000";
        document.getElementById("file").style.border = "none";
        oInput.value = "";
    } else {
        var fdata = new FormData();
        var fileInput = $('#file')[0];
        var file = fileInput.files[0];
        fdata.append("image", file);

       $.ajax({
            type: "PATCH",
            url: "/Account/AddAccountAvatar",
            processData: false,
            contentType: false,
            data: fdata,
            success: function (data) {
                document.getElementById("err-avatar").innerHTML = "<span class='success' style='color:green'>Your Avatar was updated successfully!</span>";
            },
            error: function (data) {
                document.getElementById("err-avatar").innerHTML = "<span class='warning' style='color:red'>Something went wrong</span>";
            }
        });
    }
}
//Calendar
function formatDate(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();
    if (month.length < 2)
        month = '0' + month;
    if (day.length < 2)
        day = '0' + day;
    return [year, month, day].join('-');
}

function ValidateDate(date) {
    if (formatDate(Date()) < date) {
        document.getElementById("err-publish").innerHTML = "<span class='warning'>Please select a valid publish Date</span>";
        document.getElementById("err-publish").style.color = "#FF0000";
        document.getElementById("setDate").style.border = "none";
        isDateValid = false;
    } else {
        document.getElementById("err-publish").innerHTML = "";
        isDateValid = true;
    }
}

function DisplayFullNameError() {
    if (isEditedAuthorValid == false) {
        alert("Please specify a valid author full name!")
    }
}

function DisplayAuthorErrors() {
    if (isFirstNameValid === false && isLastNameValid == true) {
        alert("Please specify a valid first name!")
        document.getElementById("err-firstName").innerHTML = "<span class='warning'>Invalid First Name!</span>";
        document.getElementById("err-firstName").style.color = "#FF0000";
        document.getElementById("firstName").style.border = "none"
    } else {
        alert("Please specify a valid last name!")
        document.getElementById("err-lastName").innerHTML = "<span class='warning'>Invalid Last Name!</span>";
        document.getElementById("err-lastName").style.color = "#FF0000";
        document.getElementById("lastName").style.border = "none";
    }
}
//Display functions
function DisplayErrors() {
    if (isTitleValid === false) {
        document.getElementById("err-title").innerHTML = "<span class='warning'>Invalid Book Title!</span>";
        document.getElementById("err-title").style.color = "#FF0000";
        document.getElementById("title").style.border = "none"
    }
    if (isLanguageValid === false) {
        document.getElementById("err-language").innerHTML = "<span class='warning'>Select the book language!</span>";
        document.getElementById("err-language").style.color = "#FF0000";
        document.getElementById("selectLanguage").style.border = "none";
    }
    if (isGenreValid === false) {
        document.getElementById("err-genre").innerHTML = "<span class='warning'>Select the book genres!</span>";
        document.getElementById("err-genre").style.color = "#FF0000";
        document.getElementById("selectGenre").style.border = "none";
    }
    if (isAddedAuthorValid === false && isExistingAuthorValid === false) {
        if (document.getElementById("insertNewAuthor").style.display === "none") {
            document.getElementById("err-existingAuthor").innerHTML = "<span class='warning'>Please specify the book author!</span>";
            document.getElementById("err-existingAuthor").style.color = "#FF0000";
            document.getElementById("selectAuthor").style.border = "none";
        } else {
            document.getElementById("err-addedAuthor").innerHTML = "<span class='warning'>Invalid Author Full Name!</span>";
            document.getElementById("err-addedAuthor").style.color = "#FF0000";
            document.getElementById("insertNewAuthor").style.border = "none";
        }
    }
    if (isDateValid === false) {
        document.getElementById("err-publish").innerHTML = "<span class='warning'>Please select a valid publish Date</span>";
        document.getElementById("err-publish").style.color = "#FF0000";
        document.getElementById("setDate").style.border = "none";
    }
}

function ShowSuccessMessage() {
    var errorElements = document.getElementsByClassName("error-message");
    for (i = 0; i < errorElements.length; i++) {
        errorElements[i].innerHTML = "";
    }
    $(".modal-body input").val("");
    document.getElementById("MessageBoxArea").innerHTML = "";
    $('<br /><div class="alert alert-success" role="alert" style="display: inline-block">Your book was added Successfully!</div >').appendTo("#MessageBoxArea");
    isTitleValid = false;
    isDateValid = false;
    isAddedAuthorValid = false;
}

function ShowAuthorSuccessMessage() {
    var errorElements = document.getElementsByClassName("error-message");
    for (i = 0; i < errorElements.length; i++) {
        errorElements[i].innerHTML = "";
    }
    $(".modal-body input").val("");
    document.getElementById("MessageBoxArea").innerHTML = "";
    $('<br /><div class="alert alert-success" role="alert" style="display: inline-block">Author added successfully!</div >').appendTo("#MessageBoxArea");
    isTitleValid = false;
    isDateValid = false;
    isAddedAuthorValid = false;
}

function ShowAuthorAlreadyExistsMessage() {
    var errorElements = document.getElementsByClassName("error-message");
    for (i = 0; i < errorElements.length; i++) {
        errorElements[i].innerHTML = "";
    }
    $(".modal-body input").val("");
    document.getElementById("MessageBoxArea").innerHTML = "";
    $('<br /><div class="alert alert-success" role="alert" style="display: inline-block">Such author already exists.</div >').appendTo("#MessageBoxArea");
    isTitleValid = false;
    isDateValid = false;
    isAddedAuthorValid = false;
}

function CheckAuthorFields() {
    var errorElements = document.getElementsByClassName("error-message");
    for (i = 0; i < errorElements.length; i++) {
        errorElements[i].innerHTML = "";
    }
    $(".modal-body input").val("");
    document.getElementById("MessageBoxArea").innerHTML = "";
    $('<br /><div class="alert alert-danger" role="alert" style="display: inline-block">Check the fields and try again.</div >').appendTo("#MessageBoxArea");
    isTitleValid = false;
    isDateValid = false;
    isAddedAuthorValid = false;
}

function ShowErrorMessage() {
    var errorElements = document.getElementsByClassName("error-message");
    for (i = 0; i < errorElements.length; i++) {
        errorElements[i].innerHTML = "";
    }
    $(".modal-body input").val("");
    document.getElementById("MessageBoxArea").innerHTML = "";
    $('<br /><div class="alert alert-danger" role="alert" style="display: inline-block">Please check the fields and try again!</div >').appendTo("#MessageBoxArea")
    isTitleValid = false;
    isDateValid = false;
    isAddedAuthorValid = false;
}
//Check all validations and perform an ajax call to the controller.
async function AddBook() {
    document.getElementById("MessageBoxArea").innerHTML = "";
    if (((isAddedAuthorValid === true && isExistingAuthorValid === false) || (isAddedAuthorValid === false && isExistingAuthorValid === true))
        && isLanguageValid === true && isTitleValid === true && isGenreValid === true && isDateValid === true) {
        $("#addBook").prop('disabled', true);

        var fdata = new FormData();
        var genres = GetAllSelectedValues("selectGenre");
        var fileInput = $('#imageFile')[0];
        var file = fileInput.files[0];
        fdata.append("Title", $("#title").val());
        fdata.append("PublishDate", $("#setDate").val());
        fdata.append("Language", document.getElementById("selectLanguage").options[document.getElementById("selectLanguage").selectedIndex].text);
        genres.forEach(item => {
            fdata.append(
                "Genres", item
            );
        });
        fdata.append("Author", GetBookAuthor());
        fdata.append("Image", file);

        $.ajax({
            type: "POST",
            url: "/book/AddNewBook",
            processData: false,
            contentType: false,
            data: fdata,
            success: function (data) {
                ShowSuccessMessage();
                isUploaded = true;
                $("#addBook").prop('disabled', false);
            },
            error: function (data) {
                ShowErrorMessage();
                $("#addBook").prop('disabled', false);
            }
        });
    } else {
        DisplayErrors();
    }
}

async function AddAuthorNotPending() {
    if (isFirstNameValid == true && isLastNameValid == true) {
        $.ajax({
            type: "POST",
            url: "/Author/UploadAuthor",
            data: {
                data: document.getElementById("firstName").value + " " + document.getElementById("lastName").value
            },
            dataType: 'json',
            success: function (response) {
                if (response.name != null) {
                    ShowAuthorSuccessMessage();
                    isUploaded = true;
                }
                else
                    ShowAuthorAlreadyExistsMessage();
            }
        });
    }
    else {
        CheckAuthorFields();
    }       
}

async function EditPendingAuthor(id) {
    if (isEditedAuthorValid == true) {
        var responsePendingRequestDTO = {
            id: document.getElementById(id).id,
            author: document.getElementById(id).value
        }
        $.ajax({
            type: "POST",
            url: "/Book/UpdatePendingBook",
            data: responsePendingRequestDTO,
            dataType: 'json',
            success: function (response) {
                isAuthorEdited = true;
            }
        });
    }
    else {
        DisplayFullNameError();
    }
}

async function AddPendingAuthor(id) {
    document.getElementById("addAuthorNameInput" + "-" + id).value = document.getElementById(id).value;
}

function ClearMessageBox() {
    document.getElementById("MessageBoxArea").innerHTML = "";
}

/*Profile Image Part*/
const imgDiv = document.querySelector('.profile-pic-div');
const img = document.querySelector('#photo');
const file = document.querySelector('#file');
const uploadBtn = document.querySelector('#uploadBtn');

if (imgDiv != null) {
    imgDiv.addEventListener('mouseenter', function () {
        uploadBtn.style.display = "block";
    });
    imgDiv.addEventListener('mouseleave', function () {
        uploadBtn.style.display = "none";
    });
}

if (file != null) {
    file.addEventListener('change', function () {
        const choosedFile = this.files[0];

        if (choosedFile) {

            const reader = new FileReader();

            reader.addEventListener('load', function () {
                img.setAttribute('src', reader.result);
            });

            reader.readAsDataURL(choosedFile);
        }
    });
}

/*Wish Book List*/
function WantedBooksInfinitiySroll(iTable, iAction, iParams) {
    this.table = iTable;        
    this.action = iAction;      
    this.params = iParams;
    this.params.userChecked = localStorage.userChecked;
    this.loading = false;       
    this.AddTableLines = function (firstItem) {
        this.loading = true;
        this.params.firstItem = firstItem;
        $.ajax({
            type: 'GET',
            url: self.action,
            data: self.params,
            dataType: "html"
        })
            .done(function (result) {
                if (result) {
                    $("#" + self.table).append(result);
                    self.loading = false;
                    $("p").tooltip()
                }
            })
            .fail(function (xhr, ajaxOptions, thrownError) {
            })
            .always(function () {
                $("#footer").css("display", "none");
            });
    }

    var self = this;
    window.onscroll = function (ev) {
        if (((window.innerHeight + window.scrollY) >= document.body.offsetHeight - 1) && $('.active.tab-pane').is('#wantedBooks')) {
            if (!self.loading) {
                var itemCount = $('#' + self.table + ' tr').length;
                self.AddTableLines(itemCount);
            }
        }
    };
    this.AddTableLines(0);
}

function initExtendAssignmentModal(id, startDate, endDate) {
    document.getElementById("extendIdInput").value = id;
    document.getElementById("extendStartDateInput").value = startDate;
    var endDateInput = document.getElementById("extendEndDateInput");
    endDateInput.value = endDate;
    endDateInput.min = new Date().toISOString().split('T')[0];
    var maxEndDate = new Date(endDate);
    maxEndDate.setDate(maxEndDate.getDate() + maxExtendDays)
    endDateInput.max = maxEndDate.toISOString().split('T')[0];
    document.getElementById('extendReason').value = "";
    document.getElementById('err-extendReason').innerText = "";
}



function ShowDeleteReviewModal(id) {
    document.getElementById("deleteReviewInput").value = id;
    $("#deleteReviewModal").modal("show");
}


function initExtendAssignmentModal(id, startDate, endDate) {
    document.getElementById("extendIdInput").value = id;
    document.getElementById("extendStartDateInput").value = startDate;
    var endDateInput = document.getElementById("extendEndDateInput");
    endDateInput.value = endDate;
    endDateInput.min = new Date().toISOString().split('T')[0];
    var maxEndDate = new Date(endDate);
    maxEndDate.setDate(maxEndDate.getDate() + maxExtendDays)
    endDateInput.max = maxEndDate.toISOString().split('T')[0];
    document.getElementById('extendReason').value = "";
    document.getElementById('err-extendReason').innerText = "";
}

async function EditBook(id) {
    document.getElementById("MessageBoxArea").innerHTML = "";
    if (((isAddedAuthorValid === true && isExistingAuthorValid === false) || (isAddedAuthorValid === false && isExistingAuthorValid === true))
        && isLanguageValid === true && isTitleValid === true && isGenreValid === true && isDateValid === true) {
        $("#editBook").prop('disabled', true);

        var fdata = new FormData();
        var genres = GetAllSelectedValues("selectGenre");
        var fileInput = $('#imageFile')[0];
        var file = fileInput.files[0];
        fdata.append("Title", $("#title").val());
        fdata.append("PublishDate", $("#setDate").val());
        fdata.append("Language", document.getElementById("selectLanguage").options[document.getElementById("selectLanguage").selectedIndex].text);
        genres.forEach(item => {
            fdata.append(
                "Genres", item
            );
        });
        fdata.append("Author", GetUpdatedBookAuthor());
        fdata.append("Image", file);
        fdata.append("Id", id)

        $.ajax({
            type: "POST",
            url: "/book/Update",
            processData: false,
            contentType: false,
            data: fdata,
            success: function (data) {
/*                ShowSuccessMessage();
*/                isUploaded = true;
                location.reload();
                $("#editBook").prop('disabled', false);
            },
            error: function (data) {
                ShowErrorMessage();
                $("#editBook").prop('disabled', false);
            }
        });
    } else {
        DisplayErrors();
    }
}