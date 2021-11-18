$(document).ready(function () {
    $("div.rate input:radio").hide();
    try {
        document.getElementById("deleteBookButton").setAttribute("data-path", "/Book/DeleteBook/@Model.Id?referrer=" + GetReferrer());
    } catch{}
    isValidReviewTitle = false;
    isValidReviewContent = true;
    IsValidReviewStar = false;
    //Fill The book Rate
    var star_rating_width = $('.fill-ratings span').width();
    $('.star-ratings').width(star_rating_width);
});

function GoBack() {
    window.location.replace(GetReferrer());
}

function GetReferrer() {
    if (document.referrer.toLowerCase().includes("account/userprofile")) {
        var referrer = document.referrer;
        if (!document.referrer.toLowerCase().includes("section")) {
            referrer += "?section=myBooksSection"
        }
        return referrer;
    }   
    else
        return "/";
}

//Review Title Validation
function ValidateReviewTitle(txt) {
    let regAdd = /^([\s]*([^\s][\s]*){5,100})$/;
    if (regAdd.test(txt) === false) {
        document.getElementById("err-reviewTitle").innerHTML = "<span class='warning'>Title should have from 5 to 100 characters!</span>";
        document.getElementById("err-reviewTitle").style.color = "#FF0000";
        document.getElementById("reviewTitle").style.border = "none";
        isValidReviewTitle = false;
    } else {
        document.getElementById("err-reviewTitle").innerHTML = "";
        isValidReviewTitle = true;
    }
}

function ValidateReviewContent(txt) {
    let regAdd = /^([\s]*([^\s][\s]*){10,500})$/;
    if (regAdd.test(txt) === false) {
        document.getElementById("err-reviewContent").innerHTML = "<span class='warning'>Review Should have from 10 to 500 characters!</span>";
        document.getElementById("err-reviewContent").style.color = "#FF0000";
        document.getElementById("reviewContent").style.border = "none";
        isValidReviewContent = false;
    } else {
        document.getElementById("err-reviewContent").innerHTML = "";
        isValidReviewContent = true;
    }
    if (txt == "") {
        document.getElementById("err-reviewContent").innerHTML = "";
        isValidReviewContent = true;
    }
}

function DisplayErrors() {
    if (isValidReviewTitle === false) {
        document.getElementById("err-reviewTitle").innerHTML = "<span class='warning'>Title should have from 5 to 100 characters!</span>";
        document.getElementById("err-reviewTitle").style.color = "#FF0000";
        document.getElementById("reviewTitle").style.border = "none";
    }
    if (isValidReviewContent === false) {
        document.getElementById("err-reviewContent").innerHTML = "<span class='warning'>Review Should have from 10 to 500 characters!</span>";
        document.getElementById("err-reviewContent").style.color = "#FF0000";
        document.getElementById("reviewContent").style.border = "none";
    }
    if (IsValidReviewStar === false) {
        document.getElementById("err-reviewStar").innerHTML = "<br /><br /><span class='warning'>Please select a book rating!</span>";
        document.getElementById("err-reviewStar").style.color = "#FF0000";
    }
}

$('input[type=radio][name=rate]').change(function () {
    IsValidReviewStar = ($("input[name=rate]").filter(":checked").val() >= 1 && $("input[name=rate]").filter(":checked").val() <= 5)
    if (IsValidReviewStar === false) {
        document.getElementById("err-reviewStar").innerHTML = "<br /><br /><span class='warning'>Please select a book rating!</span>";
        document.getElementById("err-reviewStar").style.color = "#FF0000";
    } else {
        document.getElementById("err-reviewStar").innerHTML = "";
        IsValidReviewStar = true;
    }
});

$("div.rate input:radio").hide();

function AddReview(bookId) {
    document.getElementById("err-addReview").innerHTML = "";
    if (IsValidReviewStar === true && isValidReviewTitle === true && isValidReviewContent === true) {
        var bookReview = (JSON.stringify({
            Title: $("#reviewTitle").val(),
            Content: $("#reviewContent").val(),
            //PublishDate: new Date(),
            Rating: $("input[name=rate]").filter(":checked").val(),
            //ReviewerUserName: "",
            BookId: bookId,
        }))
        $.ajax({
            type: "POST",
            url: "/Review/AddBookReview",
            contentType: "application/json; charset=utf-8",
            data: bookReview,
            success: function (data) {
                location.reload();
            },
            error: function (data) {
                document.getElementById("err-addReview").innerHTML = "<br /><br /><span class='warning'>Something went wrong!</span>";
                document.getElementById("err-addReview").style.color = "#FF0000";
                console.log(bookReview);
            }
        });
    } else {
        DisplayErrors();
    }
}

function BookReviewPagedScroll(iTable, iAction, iParams) {
    this.table = iTable;
    this.action = iAction;
    this.params = iParams;
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
                console.log("Error in AddTableLines:", thrownError);
            })
            .always(function () {
                $("#footer").css("display", "none");
            });
    }

    var self = this;
    jQuery(function ($) {
        $('#reviewsDiv').on('scroll', function () {
            if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight - 1) {
                if (!self.loading) {
                    var itemCount = $('#' + self.table + ' tr').length;
                    if ($("#AddReviewDiv").length === 0) {
                        self.AddTableLines(itemCount - 1);
                    } else {
                        self.AddTableLines(itemCount);
                    }
                }
            }
        })
    });
    this.AddTableLines(0);
}