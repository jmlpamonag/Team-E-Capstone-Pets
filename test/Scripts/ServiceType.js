var $dialog;

$(document).ready(function ()
{

	// populate Service Types
	LoadServiceTypes();

	// Open Pop Up
	$('body').on("click", "a.popup", function (e) {
        e.preventDefault();
        var page = $(this).attr('href');
        OpenPopup(page);
    });

    //Save Service Types
    $("body").on('submit', '#saveForm', function (e) {
        e.preventDefault();
        SaveServiceType();
    });

    //Update Service Types
    $("body").on('submit', '#updateForm', function (e) {
        e.preventDefault();
        UpdateServiceType();
    });

    //Delete Service Types
    $('body').on('submit', '#deleteForm', function (e) {
        e.preventDefault();
        DeleteServiceType();
    });
});




// populate Service Types
function LoadServiceTypes()
{
    $('#update_panel').html('Loading Data...');

    $.ajax({
        type: 'GET',
        url: '/TServiceTypes/GetServiceTypes',
        dataType: 'json',
        success: function (d) {
            if (d.length > 0) {

                var $data = $('<table></table>').addClass('table table-responsive table-striped');
                var header = "<thead><tr><th style='background-color: darkgray; color: black; font: bold; font - size: large; font - weight: bold;'>Service Type</th><th style='background-color: darkgray; color: black; font: bold; font - size: large; font - weight: bold;'>Action</th></tr></thead>";
                $data.append(header);
                $.each(d, function (i, row) {
                    var $row = $('<tr/>');
                    $row.append($('<td/>').html(row.strServiceType));
                    $row.append($('<td/>').html("<a href='/TServiceTypes/Update/" + row.intServiceTypetID + "' class='popup'><i class='fas fa-pencil-alt'></i></a> | <a style='color: red;' href='/TServiceTypes/Delete/" + row.intServiceTypeID + "' class='popup'><i class='fas fa-trash-alt'></i></a>"));
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
function OpenPopup(Page)
{

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


//Save Service Type
function SaveServiceType()
{
    //Validation  
    if ($('#strServiceType').val().trim() == '')
    {
        $('#msg').html('<div class="failed">All fields are required.</div>');
        return false;
    }

    var serviceType = {
        intServiceTypeID: $('#intServiceTypeID').val() == '' ? '0' : $('#intServiceTypeID').val(),
        strServiceType: $('#strServiceType').val().trim(),
};


    //Add validation token  
serviceType.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();


    //Save Service Type
    $.ajax({
        url: '/TServiceTypes/Save',
        type: 'POST',
        data: serviceType,
        dataType: 'json',
        success: function (data) {
            alert(data.message);
            if (data.status) {
                $('#intServiceTypeID').val('');
                $('#strServiceType').val('');

                LoadServiceTypes();
                $dialog.dialog('close');
            }
        },
        error: function () {
            $('#msg').html('<div class="failed">Error! Please try again.</div>');
        }
    });
}

//Update Service Type
function UpdateServiceType()
{
    //Validation  
    if ($('#strServiceType').val().trim() == '')
    {
        $('#msg').html('<div class="failed">All fields are required.</div>');
        return false;
    }

    var serviceType = {

        intServiceTypeID: $('#intServiceTypeID').val() == '' ? '0' : $('#intServiceTypeID').val(),
        strServiceType: $('#strServiceType').val().trim()

    };


    //Add validation token  
    serviceType.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();


    //Update Service Type
    $.ajax({
        url: '/TServiceTypes/Update',
        type: 'POST',
        data: serviceType,
        dataType: 'json',
        success: function (data) {
            alert(data.message);
            if (data.status) {
                $('#intServiceTypeID').val('');
                $('#strServiceType').val('');

                LoadServiceTypes();
                $dialog.dialog('close');
            }
        },
        error: function () {
            $('#msg').html('<div class="failed">Error! Please try again.</div>');
        }
    });
}

//Delete Service Type  
function DeleteServiceType()
{
    $.ajax({
        url: '/TServiceTypes/Delete',
        type: 'POST',
        dataType: 'json',
        data: {
            'id': $('#intServiceTypeID').val(),
            '__RequestVerificationToken': $('input[name=__RequestVerificationToken]').val()
        },
        success: function (data) {
            alert(data.message);
            if (data.status) {
                $dialog.dialog('close');
                LoadServiceTypes();
            }
        },
        error: function () {
            $('#msg').html('<div class="failed">Error ! Please try again.</div>');
        }
    });
}  
