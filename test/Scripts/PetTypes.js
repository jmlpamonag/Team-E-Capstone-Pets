var $dialog;

$(document).ready(function () {

    // populate Pet Types
    LoadPetTypes();

    // Open Pop Up
    $('body').on("click", "a.popup", function (e) {
        e.preventDefault();
        var page = $(this).attr('href');
        OpenPopup(page);
    });

    //Save Pet Types
    $("body").on('submit', '#saveForm', function (e) {
        e.preventDefault();
        SavePetType();
    });

    //Update Pet Types
    $("body").on('submit', '#updateForm', function (e) {
        e.preventDefault();
        UpdatePetType();
    });

    //Delete Pet Types
    $('body').on('submit', '#deleteForm', function (e) {
        e.preventDefault();
        DeletePetType();
    });
});




// populate Pet Types
function LoadPetTypes() {
    $('#update_panel').html('Loading Data...');

    $.ajax({
        type: 'GET',
        url: '/TPetTypes/GetPetTypes',
        dataType: 'json',
        success: function (d) {
            if (d.length > 0) {

                var $data = $('<table></table>').addClass('table table-responsive table-striped');
                var header = "<thead><tr><th style='background-color: darkgray; color: black; font: bold; font - size: large; font - weight: bold;'>Pet Type</th><th style='background-color: darkgray; color: black; font: bold; font - size: large; font - weight: bold;'>Action</th></tr></thead>";
                $data.append(header);
                $.each(d, function (i, row) {
                    var $row = $('<tr/>');
                    $row.append($('<td/>').html(row.strPetType));
                    $row.append($('<td/>').html("<a href='/TPetTypes/Update/" + row.intPetTypetID + "' class='popup'><i class='fas fa-pencil-alt'></i></a> | <a style='color: red;' href='/TPetTypes/Delete/" + row.intPetTypeID + "' class='popup'><i class='fas fa-trash-alt'></i></a>"));
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


//Save Pet Types
function SavePetType() {
    //Validation  
    if ($('#strPetType').val().trim() == '') {
        $('#msg').html('<div class="failed">All fields are required.</div>');
        return false;
    }

    var petType = {
        intPetTypeID: $('#intPetTypeID').val() == '' ? '0' : $('#intPetTypeID').val(),
        strPetType: $('#strPetType').val().trim(),
    };


    //Add validation token  
    petType.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();


    //Save Pet Types
    $.ajax({
        url: '/TPetTypes/Save',
        type: 'POST',
        data: petType,
        dataType: 'json',
        success: function (data) {
            alert(data.message);
            if (data.status) {
                $('#intPetTypeID').val('');
                $('#strPetType').val('');

                LoadPetTypes();
                $dialog.dialog('close');
            }
        },
        error: function () {
            $('#msg').html('<div class="failed">Error! Please try again.</div>');
        }
    });
}

//Update Pet Types
function UpdatePetType() {
    //Validation  
    if ($('#strPetType').val().trim() == '') {
        $('#msg').html('<div class="failed">All fields are required.</div>');
        return false;
    }

    var petType = {

        intPetTypeID: $('#intPetTypeID').val() == '' ? '0' : $('#intPetTypeID').val(),
        strPetType: $('#strPetType').val().trim()

    };


    //Add validation token  
    petType.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();


    //Update Pet Types
    $.ajax({
        url: '/TPetTypes/Update',
        type: 'POST',
        data: petType,
        dataType: 'json',
        success: function (data) {
            alert(data.message);
            if (data.status) {
                $('#intPetTypeID').val('');
                $('#strPetType').val('');

                LoadPetTypes();
                $dialog.dialog('close');
            }
        },
        error: function () {
            $('#msg').html('<div class="failed">Error! Please try again.</div>');
        }
    });
}

//Delete Pet Types  
function DeletePetType() {
    $.ajax({
        url: '/TPetTypes/Delete',
        type: 'POST',
        dataType: 'json',
        data: {
            'id': $('#intPetTypeID').val(),
            '__RequestVerificationToken': $('input[name=__RequestVerificationToken]').val()
        },
        success: function (data) {
            alert(data.message);
            if (data.status) {
                $dialog.dialog('close');
                LoadPetTypes();
            }
        },
        error: function () {
            $('#msg').html('<div class="failed">Error ! Please try again.</div>');
        }
    });
}  
