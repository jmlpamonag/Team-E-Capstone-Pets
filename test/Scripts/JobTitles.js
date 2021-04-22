var $dialog;

$(document).ready(function () {

    // populate Job Titles
    LoadJobTitles();

    // Open Pop Up
    $('body').on("click", "a.popup", function (e) {
        e.preventDefault();
        var page = $(this).attr('href');
        OpenPopup(page);
    });

    //Save Job Titles
    $("body").on('submit', '#saveForm', function (e) {
        e.preventDefault();
        SaveJobTitle();
    });

    //Update Job Titles
    $("body").on('submit', '#updateForm', function (e) {
        e.preventDefault();
        UpdateJobTitle();
    });

    //Delete Job Titles
    $('body').on('submit', '#deleteForm', function (e) {
        e.preventDefault();
        DeleteJobTitle();
    });
});




// populate Job Titles
function LoadJobTitles() {
    $('#update_panel').html('Loading Data...');

    $.ajax({
        type: 'GET',
        url: '/TJobTitles/GetJobTitles',
        dataType: 'json',
        success: function (d) {
            if (d.length > 0) {

                var $data = $('<table></table>').addClass('table table-responsive table-striped');
                var header = "<thead><tr><th style='background-color: darkgray; color: black; font: bold; font - size: large; font - weight: bold;'>Job Title</th><th style='background-color: darkgray; color: black; font: bold; font - size: large; font - weight: bold;'>Action</th></tr></thead>";
                $data.append(header);
                $.each(d, function (i, row) {
                    var $row = $('<tr/>');
                    $row.append($('<td/>').html(row.strJobTitle));
                    $row.append($('<td/>').html("<a href='/TJobTitles/Update/" + row.intJobTitletID + "' class='popup'><i class='fas fa-pencil-alt'></i></a> | <a style='color: red;' href='/TJobTitles/Delete/" + row.intJobTitleID + "' class='popup'><i class='fas fa-trash-alt'></i></a>"));
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


//Save Job Titles
function SaveJobTitle() {
    //Validation  
    if ($('#strJobTitleDesc').val().trim() == '') {
        $('#msg').html('<div class="failed">All fields are required.</div>');
        return false;
    }

    var jobTitle = {
        intJobTitleID: $('#intJobTitleID').val() == '' ? '0' : $('#intJobTitleID').val(),
        strJobTitleDesc: $('#strJobTitleDesc').val().trim(),
    };


    //Add validation token  
    JobTitle.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();


    //Save Job Titles
    $.ajax({
        url: '/TJobTitles/Save',
        type: 'POST',
        data: JobTitle,
        dataType: 'json',
        success: function (data) {
            alert(data.message);
            if (data.status) {
                $('#intJobTitleID').val('');
                $('#strJobTitleDesc').val('');

                LoadJobTitles();
                $dialog.dialog('close');
            }
        },
        error: function () {
            $('#msg').html('<div class="failed">Error! Please try again.</div>');
        }
    });
}

//Update Job Titles
function UpdateJobTitle() {
    //Validation  
    if ($('#strJobTitleDesc').val().trim() == '') {
        $('#msg').html('<div class="failed">All fields are required.</div>');
        return false;
    }

    var jobTitle = {

        intJobTitleID: $('#intJobTitleID').val() == '' ? '0' : $('#intJobTitleID').val(),
        strJobTitle: $('#strJobTitleDesc').val().trim()

    };


    //Add validation token  
    jobTitle.__RequestVerificationToken = $('input[name=__RequestVerificationToken]').val();


    //Update Job Titles
    $.ajax({
        url: '/TJobTitles/Update',
        type: 'POST',
        data: jobTitle,
        dataType: 'json',
        success: function (data) {
            alert(data.message);
            if (data.status) {
                $('#intJobTitleID').val('');
                $('#strJobTitleDesc').val('');

                LoadJobTitles();
                $dialog.dialog('close');
            }
        },
        error: function () {
            $('#msg').html('<div class="failed">Error! Please try again.</div>');
        }
    });
}

//Delete Job Titles  
function DeleteJobTitle() {
    $.ajax({
        url: '/TJobTitles/Delete',
        type: 'POST',
        dataType: 'json',
        data: {
            'id': $('#intJobTitleID').val(),
            '__RequestVerificationToken': $('input[name=__RequestVerificationToken]').val()
        },
        success: function (data) {
            alert(data.message);
            if (data.status) {
                $dialog.dialog('close');
                LoadJobTitles();
            }
        },
        error: function () {
            $('#msg').html('<div class="failed">Error ! Please try again.</div>');
        }
    });
}  
