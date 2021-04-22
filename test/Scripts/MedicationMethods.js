var $dialog;

$(document).ready(function () {

    // populate Methods
    LoadMethods();

    // Open Pop Up
    $('body').on("click", "a.popup", function (e) {
        e.preventDefault();
        var page = $(this).attr('href');
        OpenPopup(page);
    });

    //Save Methods
    $("body").on('submit', '#saveForm', function (e) {
        e.preventDefault();
        SaveMethod();
    });

    //Update Methods
    $("body").on('submit', '#updateForm', function (e) {
        e.preventDefault();
        UpdateMethod();
    });

    //Delete Methods
    $('body').on('submit', '#deleteForm', function (e) {
        e.preventDefault();
        DeleteMethod();
    });
});




// populate Methods
function LoadMethods() {
    $('#update_panel').html('Loading Data...');

    $.ajax({
        type: 'GET',
        url: '/TMethods/GetMethods',
        dataType: 'json',
        success: function (d) {
            if (d.length > 0) {

                var $data = $('<table></table>').addClass('table table-responsive table-striped');
                var header = "<thead><tr><th style='background-color: darkgray; color: black; font: bold; font - size: large; font - weight: bold;'>Medication Method</th><th style='background-color: darkgray; color: black; font: bold; font - size: large; font - weight: bold;'>Action</th></tr></thead>";
                $data.append(header);
                $.each(d, function (i, row) {
                    var $row = $('<tr/>');
                    $row.append($('<td/>').html(row.strMethod));
                    $row.append($('<td/>').html("<a href='/TMethods/Update/" + row.intMethodtID + "' class='popup'><i class='fas fa-pencil-alt'></i></a> | <a style='color: red;' href='/TMethods/Delete/" + row.intMethodID + "' class='popup'><i class='fas fa-trash-alt'></i></a>"));
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


//Save Methods
function SaveMethod() {
    //Validation  
    if ($('#strMethod').val().trim() == '') {
        $('#msg').html('<div class="failed">All fields are required.</div>');
        return false;
    }

    var method = {
        intMethodID: $('#intMethodID').val() == '' ? '0' : $('#intMethodID').val(),
        strMethod: $('#strMethod').val().trim(),
    };


    //Add validation token  
    method.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();


    //Save Methods
    $.ajax({
        url: '/TMethods/Save',
        type: 'POST',
        data: method,
        dataType: 'json',
        success: function (data) {
            alert(data.message);
            if (data.status) {
                $('#intMethodID').val('');
                $('#strMethod').val('');

                LoadMethods();
                $dialog.dialog('close');
            }
        },
        error: function () {
            $('#msg').html('<div class="failed">Error! Please try again.</div>');
        }
    });
}

//Update Methods
function UpdateMethod() {
    //Validation  
    if ($('#strMethod').val().trim() == '') {
        $('#msg').html('<div class="failed">All fields are required.</div>');
        return false;
    }

    var method = {

        intMethodID: $('#intMethodID').val() == '' ? '0' : $('#intMethodID').val(),
        strMethod: $('#strMethod').val().trim()

    };


    //Add validation token  
    method.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();


    //Update Methods
    $.ajax({
        url: '/TMethods/Update',
        type: 'POST',
        data: method,
        dataType: 'json',
        success: function (data) {
            alert(data.message);
            if (data.status) {
                $('#intMethodID').val('');
                $('#strMethod').val('');

                LoadMethods();
                $dialog.dialog('close');
            }
        },
        error: function () {
            $('#msg').html('<div class="failed">Error! Please try again.</div>');
        }
    });
}

//Delete Methods  
function DeleteMethod() {
    $.ajax({
        url: '/TMethods/Delete',
        type: 'POST',
        dataType: 'json',
        data: {
            'id': $('#intMethodID').val(),
            '__RequestVerificationToken': $('input[name=__RequestVerificationToken]').val()
        },
        success: function (data) {
            alert(data.message);
            if (data.status) {
                $dialog.dialog('close');
                LoadMethods();
            }
        },
        error: function () {
            $('#msg').html('<div class="failed">Error ! Please try again.</div>');
        }
    });
}  
