var $dialog;

$(document).ready(function () {

    // populate Breeds
    LoadBreeds();

    // Open Pop Up
    $('body').on("click", "a.popup", function (e) {
        e.preventDefault();
        var page = $(this).attr('href');
        OpenPopup(page);
    });

    //Save Breeds
    $("body").on('submit', '#saveForm', function (e) {
        e.preventDefault();
        SaveBreed();
    });

    //Update Breeds
    $("body").on('submit', '#updateForm', function (e) {
        e.preventDefault();
        UpdateBreed();
    });

    //Delete Breeds
    $('body').on('submit', '#deleteForm', function (e) {
        e.preventDefault();
        DeleteBreed();
    });
});




// populate Breeds
function LoadBreeds() {
    $('#update_panel').html('Loading Data...');

    $.ajax({
        type: 'GET',
        url: '/TBreeds/GetBreeds',
        dataType: 'json',
        success: function (d) {
            if (d.length > 0) {

                var $data = $('<table></table>').addClass('table table-responsive table-striped');
                var header = "<thead><tr><th style='background-color: darkgray; color: black; font: bold; font - size: large; font - weight: bold;'>Breeds</th><th style='background-color: darkgray; color: black; font: bold; font - size: large; font - weight: bold;'>Action</th></tr></thead>";
                $data.append(header);
                $.each(d, function (i, row) {
                    var $row = $('<tr/>');
                    $row.append($('<td/>').html(row.strBreedName));
                    $row.append($('<td/>').html("<a href='/TBreeds/Update/" + row.intBreedID + "' class='popup'><i class='fas fa-pencil-alt'></i></a> | <a style='color: red;' href='/TBreeds/Delete/" + row.intBreedID + "' class='popup'><i class='fas fa-trash-alt'></i></a>"));
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


//Save Breed
function SaveBreed() {
    //Validation  
    if ($('#strBreedName').val().trim() == '') {
        $('#msg').html('<div class="failed">All fields are required.</div>');
        return false;
    }

    var strbreed = {
        intBreedID: $('#intBreedID').val() == '' ? '0' : $('#intBreedID').val(),
        strBreedName: $('#strBreedName').val().trim(),
    };


    //Add validation token  
    strbreed.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();


    //Save Breed
    $.ajax({
        url: '/TBreeds/Save',
        type: 'POST',
        data: strbreed,
        dataType: 'json',
        success: function (data) {
            alert(data.message);
            if (data.status) {
                $('#intBreedID').val('');
                $('#strBreedName').val('');

                LoadBreeds();
                $dialog.dialog('close');
            }
        },
        error: function () {
            $('#msg').html('<div class="failed">Error! Please try again.</div>');
        }
    });
}

//Update Breed
function UpdateBreed() {
    //Validation  
    if ($('#strBreedName').val().trim() == '') {
        $('#msg').html('<div class="failed">All fields are required.</div>');
        return false;
    }

    var strbreed = {

        intBreedID: $('#intBreedID').val() == '' ? '0' : $('#intBreedID').val(),
        strBreedName: $('#strBreedName').val().trim()

    };


    //Add validation token  
    strbreed.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();


    //Update Breed
    $.ajax({
        url: '/TBreeds/Update',
        type: 'POST',
        data: strbreed,
        dataType: 'json',
        success: function (data) {
            alert(data.message);
            if (data.status) {
                $('#intBreedID').val('');
                $('#strBreedName').val('');

                LoadBreeds();
                $dialog.dialog('close');
            }
        },
        error: function () {
            $('#msg').html('<div class="failed">Error! Please try again.</div>');
        }
    });
}

//Delete Breed  
function DeleteBreed() {
    $.ajax({
        url: '/TBreeds/Delete',
        type: 'POST',
        dataType: 'json',
        data: {
            'id': $('#intBreedID').val(),
            '__RequestVerificationToken': $('input[name=__RequestVerificationToken]').val()
        },
        success: function (data) {
            alert(data.message);
            if (data.status) {
                $dialog.dialog('close');
                LoadBreeds();
            }
        },
        error: function () {
            $('#msg').html('<div class="failed">Error ! Please try again.</div>');
        }
    });
}  
