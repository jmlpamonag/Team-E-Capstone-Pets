var $dialog;

$(document).ready(function () {

    // populate Roles
    LoadRoles();

    // Open Pop Up
    $('body').on("click", "a.popup", function (e) {
        e.preventDefault();
        var page = $(this).attr('href');
        OpenPopup(page);
    });

    //Save Roles
    $("body").on('submit', '#saveForm', function (e) {
        e.preventDefault();
        SaveRole();
    });

    //Update Roles
    $("body").on('submit', '#updateForm', function (e) {
        e.preventDefault();
        UpdateRole();
    });

    //Delete Roles
    $('body').on('submit', '#deleteForm', function (e) {
        e.preventDefault();
        DeleteRole();
    });
});




// populate Roles
function LoadRoles() {
    $('#update_panel').html('Loading Data...');

    $.ajax({
        type: 'GET',
        url: '/TRoles/GetRoles',
        dataType: 'json',
        success: function (d) {
            if (d.length > 0) {

                var $data = $('<table></table>').addClass('table table-responsive table-striped');
                var header = "<thead><tr><th style='background-color: darkgray; color: black; font: bold; font - size: large; font - weight: bold;'>User Role</th><th style='background-color: darkgray; color: black; font: bold; font - size: large; font - weight: bold;'>Action</th></tr></thead>";
                $data.append(header);
                $.each(d, function (i, row) {
                    var $row = $('<tr/>');
                    $row.append($('<td/>').html(row.strRoleName));
                    $row.append($('<td/>').html("<a href='/TRoles/Update/" + row.intRoletID + "' class='popup'><i class='fas fa-pencil-alt'></i></a> | <a style='color: red;' href='/TRoles/Delete/" + row.intRoleID + "' class='popup'><i class='fas fa-trash-alt'></i></a>"));
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


//Save Roles
function SaveRole() {
    //Validation  
    if ($('#strRoleName').val().trim() == '') {
        $('#msg').html('<div class="failed">All fields are required.</div>');
        return false;
    }

    var role = {
        intRoleID: $('#intRoleID').val() == '' ? '0' : $('#intRoleID').val(),
        strRoleName: $('#strRoleName').val().trim(),
    };


    //Add validation token  
    role.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();


    //Save Roles
    $.ajax({
        url: '/TRoles/Save',
        type: 'POST',
        data: role,
        dataType: 'json',
        success: function (data) {
            alert(data.message);
            if (data.status) {
                $('#intRoleID').val('');
                $('#strRoleName').val('');

                LoadRoles();
                $dialog.dialog('close');
            }
        },
        error: function () {
            $('#msg').html('<div class="failed">Error! Please try again.</div>');
        }
    });
}

//Update Roles
function UpdateRole() {
    //Validation  
    if ($('#strRoleName').val().trim() == '') {
        $('#msg').html('<div class="failed">All fields are required.</div>');
        return false;
    }

    var role = {

        intRoleID: $('#intRoleID').val() == '' ? '0' : $('#intRoleID').val(),
        strRoleName: $('#strRoleName').val().trim()

    };


    //Add validation token  
    role.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();


    //Update Roles
    $.ajax({
        url: '/TRoles/Update',
        type: 'POST',
        data: role,
        dataType: 'json',
        success: function (data) {
            alert(data.message);
            if (data.status) {
                $('#intRoleID').val('');
                $('#strRoleName').val('');

                LoadRoles();
                $dialog.dialog('close');
            }
        },
        error: function () {
            $('#msg').html('<div class="failed">Error! Please try again.</div>');
        }
    });
}

//Delete Roles  
function DeleteRole() {
    $.ajax({
        url: '/TRoles/Delete',
        type: 'POST',
        dataType: 'json',
        data: {
            'id': $('#intRoleID').val(),
            '__RequestVerificationToken': $('input[name=__RequestVerificationToken]').val()
        },
        success: function (data) {
            alert(data.message);
            if (data.status) {
                $dialog.dialog('close');
                LoadRoles();
            }
        },
        error: function () {
            $('#msg').html('<div class="failed">Error ! Please try again.</div>');
        }
    });
}  
