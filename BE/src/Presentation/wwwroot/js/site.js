$(document).ready(function () {
    // Notifications Handler Timer
    function GetNotifications() {

        let lastNotification = $("#notifyContent > li")[0];

        let date = null;

        if (lastNotification) {
            date = lastNotification.getAttribute('date');
            $("#notifyModal").removeClass('no-notification-img');
            $('#clearAll').css('display', 'block');
        }
        else {
            $("#notifyModal").addClass('no-notification-img');
        }

        $.ajax({
            url: "/Notification/GetAll",
            type: "GET",
            data: { lastNotifyDate: date },
            async: true,
            success: function (data) {
                $("#notifyContent").prepend(data);
                let num = $("#notifyContent > li").length;
                if (num) {
                    $("#notifyBadgeCounter").text(num);
                    $("#notifyModal").removeClass('no-notification-img');
                    $('#notifyBadgeCounter').css('display', 'inline-block');
                    $('#clearAll').css('display', 'block');
                }
                else {
                    $('#notifyBadgeCounter').css('display', 'none');
                    $('#clearAll').css('display', 'none');
                }
                setTimeout(GetNotifications, 5000);
            },
            error: function (data) {
                setTimeout(GetNotifications, 5000);
            }
        });
    }

    GetNotifications();

    //Stay on selected tab on refresh (Fix)

    $(document).on("click", ".tabItem", function (e) {
        const url = new URL(window.location);
        url.searchParams.set('section', event.target.id);
        window.history.pushState({}, '', url);
    });

    var path_to_delete;
    var root = location.protocol + "//" + location.host;

    $("#deleteBookButton").click(function (e) {
        path_to_delete = $(this).data('path');
        $('#deleteForm').attr('action', root + path_to_delete);
    });
    $('[data-toggle="tooltip"]').tooltip({
        trigger: 'hover',
    })

    var wasAssigned = false;
    //Assign Book Button
    $('#assignBookBtn').click(function () {

        $.ajax({
            url: "/Book/AssignBook",
            type: "POST",
            data: { bookId: parseInt($('#assignBookBtn').attr('bookid')) },
            cache: false,
            dataType: 'json',
            async: true,
            success: function (data) {

                if (data.error) {
                    $("#assignBookModalHeader").removeClass("bg-success");
                    $("#assignBookModalHeader").addClass("bg-error");
                    $("#assignBookModalContent").html(data.message);
                }
                else
                {
                    $("#assignBookModalHeader").removeClass("bg-error");
                    $("#assignBookModalHeader").addClass("bg-success");
                    $("#assignBookModalContent").html("Book was successfully assigned to you.<br> Your assignment ends at:<br><b>" + data.date + "</b>");
                }
                wasAssigned = true;
                $("#assignBookModal").modal('show');
                
            }
        });
    });

    $('#addToQueueBtn').click(function () {
        if ($('#addToQueueBtn').hasClass('btn-disabled')) {
            return false;
        } else {
            $.ajax({
                url: "/Book/AddToQueue",
                type: "POST",
                dataType: "json",
                data: {
                    bookId: parseInt($('#addToQueueBtn').attr('bookId'))
                },
                success: function (data) {
                    $("#assignBookModalHeader").addClass("bg-success");
                    $("#assignBookModalContent").html("Book was successfully added to the queue. Your assignment starts at:<br><b>" + data.date + "</b>");
                    wasAssigned = true;
                    $("#assignBookModal").modal('show');
                }
            });
        }
        
    });

    //AssignBook Modal Close Event
    $('#assignBookModal').on('hidden.bs.modal', function () {
        if (wasAssigned)
            window.location.reload();
    })
    var userNameSearchTimeout = null;
    $('#userNameSearch').on('input', function (e) {
        if (userNameSearchTimeout != null) {
            clearTimeout(userNameSearchTimeout);
        }
        userNameSearchTimeout = setTimeout(function () {
            userNameSearchTimeout = null;
            $("#ManageRolesTable").empty();
            RolesInfinityScroll.AddTableLines(0);

        }, 1000);  
        
    });

    // UserManageModal Refresh on Close
    $('#userManageModal').on('hidden.bs.modal', function () {
            window.location.reload();
    })
});

function ManageRolesInfinityScroll(iTable, iAction, iParams) {
    this.table = iTable;
    this.action = iAction;
    this.params = iParams;
    this.loading = false;
    this.AddTableLines = function (firstItem) {
        this.loading = true;
        this.params.firstItem = firstItem;
        this.params.userNameSearch = $('#userNameSearch').val();
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
    window.removeEventListener('scroll', someFunction);
    window.addEventListener('scroll', someFunction);
    function someFunction() {
        if (((window.innerHeight + window.scrollY) >= document.body.offsetHeight - 1) && $('.active.tab-pane').is('#manageRoles')) {
            if (!self.loading) {
                var itemCount = $('#' + self.table + ' tr').length;
                self.AddTableLines(itemCount);
            }
        }
    }
    this.AddTableLines(0);
}

function toggleModalErrorHeader(modalId, isError) {
    $(modalId).removeClass(isError ? "bg-success" : "bg-danger");
    $(modalId).addClass(isError ? "bg-danger" : "bg-success");
}

function assignAdmin(id, username) {
    $("#userManageHeader").text("Admin Assignment");
    $.ajax({
        type: "POST",
        url: "/Account/AssignAdmin",
        data: {
            id: id
        },
        success: function () {
            toggleModalErrorHeader("#userManageModalHeader", false)
            $("#userManageModalContent").html("Admin role has been succesfully assigned to the user <b>" + username + "</b>");
           
        },
        error: function () {
            toggleModalErrorHeader("#userManageModalHeader", true)
            $("#userManageModalContent").html("Error adding admin role to the user <b>" + username + "</b>");
        }
    });
}

function removeAdmin(id, username) {
    $("#userManageHeader").text("Admin Removal");
    $("#userManageModalContent").html("Loading Response...");
    $.ajax({
        type: "POST",
        url: "/Account/RemoveAdmin",
        data: {
            id: id
        },
        success: function () {
            toggleModalErrorHeader("#userManageModalHeader", false)
            $("#userManageModalContent").html("Admin role has been removed from the user <b>" + username + "</b>");
          
        },
        error: function () {
            toggleModalErrorHeader("#userManageModalHeader", true)
            $("#userManageModalContent").html("Error removing admin role from the user <b>" + username + "</b>");
        }
    });
}

function AdminReviewsInfinityScroll(iTable, iAction, iParams) {
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
            })
            .always(function () {
                $("#footer").css("display", "none");
            });
    }

    var self = this;
    window.removeEventListener('scroll', someFunction);
    window.addEventListener('scroll', someFunction);
    function someFunction() {
        if (((window.innerHeight + window.scrollY) >= document.body.offsetHeight - 1) && $('.active.tab-pane').is('#deleteReviews')) {
            if (!self.loading) {
                var itemCount = $('#' + self.table + ' tr').length;
                self.AddTableLines(itemCount);
            }
        }
    }
    this.AddTableLines(0);
}

function PendingAuthorsTable(iTable, iAction, iParams) {
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
            })
            .always(function () {
                $("#footer").css("display", "none");
            });
    }

    var self = this;
    window.removeEventListener('scroll', someFunction);
    window.addEventListener('scroll', someFunction);
    function someFunction() {
        if (((window.innerHeight + window.scrollY) >= document.body.offsetHeight - 1) && $('.active.tab-pane').is('#addAuthors')) {
            if (!self.loading) {
                var itemCount = $('#' + self.table + ' tr').length;
                self.AddTableLines(itemCount);
            }
        }
    }
    this.AddTableLines(0);
}

// Read notification function
function ReadNotification(notificationId, path) {
    $.ajax({
        url: "/Notification/ReadNotification",
        type: "DELETE",
        data: { id: notificationId },
        async: true,
        success: function () {
            window.location.replace(path);
        },
        error: function () {
        }
    });
}

function ClearNotifications() {
    $.ajax({
        url: "/Notification/ClearNotifications",
        type: "POST",
        async: true,
        success: function () {
            $("#notifyContent").empty();
            $('#notifyBadgeCounter').css('display', 'none');
            $("#notifyModal").addClass('no-notification-img');
            $('#notifyBadgeCounter').text("0");
            $('#clearAll').css('display', 'none');
        },
        error: function () {
        }
    });
}