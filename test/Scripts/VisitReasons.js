var $dialog;

$(document).ready(function () {

    // populate VisitReasons
    LoadVisitReasons();

    // Open Pop Up
    $('body').on("click", "a.popup", function (e) {
        e.preventDefault();
        var page = $(this).attr('href');
        OpenPopup(page);
    });

    //Save VisitReasons
    $("body").on('submit', '#saveForm', function (e) {
        e.preventDefault();
        SaveVisitReason();
    });

    //Update VisitReasons
    $("body").on('submit', '#updateForm', function (e) {
        e.preventDefault();
        UpdateVisitReason();
    });

    //Delete VisitReasons
    $('body').on('submit', '#deleteForm', function (e) {
        e.preventDefault();
        DeleteVisitReason();
    });
});




// populate VisitReasons
function LoadVisitReasons() {
    $('#update_panel').html('Loading Data...');

    $.ajax({
        type: 'GET',
        url: '/TVisitReasons/GetVisitReasons',
        dataType: 'json',
        success: function (d) {
            if (d.length > 0) {

                var $data = $('<table></table>').addClass('table table-responsive table-striped');
                var header = "<thead><tr><th style='background-color: darkgray; color: black; font: bold; font - size: large; font - weight: bold;'>Visit Reason</th><th style='background-color: darkgray; color: black; font: bold; font - size: large; font - weight: bold;'>Action</th></tr></thead>";
                $data.append(header);
                $.each(d, function (i, row) {
                    var $row = $('<tr/>');
                    $row.append($('<td/>').html(row.strVisitReason));
                    $row.append($('<td/>').html("<a href='/TVisitReasons/Update/" + row.intVisitReasonID + "' class='popup'><i class='fas fa-pencil-alt'></i></a> | <a style='color: red;' href='/TVisitReasons/Delete/" + row.intVisitReasonID + "' class='popup'><i class='fas fa-trash-alt'></i></a>"));
                    $data.append($row);
                });

                $('#update_panel').html($data);
            }
            else {
                var $noData = $('<div/>').html('No Data Found!');
                $('#update_panel').html($noData);
            }
        },
        error: function () {
            alert('Error! AJAX is broken.');
        }
    });

}

//open popup  
function OpenPopup(Page) {

    var $pageContent = $('<div/>');
    $pageContent.load(Page);
    $dialog = $('<div class="popupWindow" style="overflow:hidden"></div>')
        .html($pageContent)
        .dialog({
            title: "Capstone Pets",
            draggable: true,
            autoOpen: false,
            resizable: true,
            model: true,
            height: 450,
            width: 500,
            closeText: "",
            close: function () {
                $dialog.dialog('destroy').remove();
            }
        })
    $dialog.dialog('open');
}


//Save VisitReasons
function SaveVisitReason() {
    //Validation  
    if ($('#strVisitReason').val().trim() == '') {
        $('#msg').html('<div class="failed">All fields are required.</div>');
        return false;
    }

    var visitReason = {
        intVisitReasonID: $('#intVisitReasonID').val() == '' ? '0' : $('#intVisitReasonID').val(),
        strVisitReason: $('#strVisitReason').val().trim()
    };


    //Add validation token  
    visitReason.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();


    //Save VisitReasons
    $.ajax({
        url: '/TVisitReasons/Save',
        type: 'POST',
        data: visitReason,
        dataType: 'json',
        success: function (data) {
            alert(data.message);
            if (data.status) {
                $('#intVisitReasonID').val('');
                $('#strVisitReason').val('');

                LoadVisitReasons();
                $dialog.dialog('close');
            }
        },
        error: function () {
            $('#msg').html('<div class="failed">Error! Please try again.</div>');
        }
    });
}

//Update VisitReasons
function UpdateVisitReason() {
    //Validation  
    if ($('#strVisitReason').val().trim() == '') {
        $('#msg').html('<div class="failed">All fields are required.</div>');
        return false;
    }

    var visitReason = {

        intVisitReasonID: $('#intVisitReasonID').val() == '' ? '0' : $('#intVisitReasonID').val(),
        strVisitReason: $('#strVisitReason').val().trim()
    };


    //Add validation token  
    visitReason.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();


    //Update VisitReasons
    $.ajax({
        url: '/TVisitReasons/Update',
        type: 'POST',
        data: visitReason,
        dataType: 'json',
        success: function (data) {
            alert(data.message);
            if (data.status) {
                $('#intVisitReasonID').val('');
                $('#strVisitReason').val('');
                LoadVisitReasons();
                $dialog.dialog('close');
            }
        },
        error: function () {
            $('#msg').html('<div class="failed">Error! Please try again.</div>');
        }
    });
}

//Delete VisitReasons  
function DeleteVisitReason() {
    $.ajax({
        url: '/TVisitReasons/Delete',
        type: 'POST',
        dataType: 'json',
        data: {
            'id': $('#intVisitReasonID').val(),
            '__RequestVerificationToken': $('input[name=__RequestVerificationToken]').val()
        },
        success: function (data) {
            alert(data.message);
            if (data.status) {
                $dialog.dialog('close');
                LoadVisitReasons();
            }
        },
        error: function () {
            $('#msg').html('<div class="failed">Error ! Please try again.</div>');
        }
    });
}  
