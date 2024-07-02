var DetailsEmailConfig = function (id) {
    var url = "/EmailConfig/Details?id=" + id;
    $('#titleBigModal').html("Email Config Details");
    loadBigModal(url);
};

var AddEditEmailConfig = function (id) {
    var url = "/EmailConfig/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleBigModal').html("Edit Email Config");
    } else {
        $('#titleBigModal').html("Add Email Config");
    }
    loadBigModal(url);
};

var SaveEmailConfig = function () {
    if (!$("#frmEmailConfig").valid()) {
        return;
    }

    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');

    var _frmEmailConfig = $("#frmEmailConfig").serialize();
    $.ajax({
        type: "POST",
        url: "/EmailConfig/SaveEmailConfig",
        data: _frmEmailConfig,
        success: function (result) {
            $("#btnSave").val("Save");
            $('#btnSave').removeAttr('disabled');
            
            if (result.IsSuccess) {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "success"
                }).then(function () {
                    document.getElementById("btnClose").click();
                    $('#tblEmailConfig').DataTable().ajax.reload();
                });
            }
            else {
                FieldValidationAlert('#Email', result.AlertMessage, "warning");
            }
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}


var SaveEmailConfigOLD = function () {
    if ($('#SSLEnabled').is(':checked')) {
        $("#SSLEnabled").val(true)
    }
    else {
        $("#SSLEnabled").val(false)
    }

    if ($('#IsDefault').is(':checked')) {
        $("#IsDefault").val(true)
    }
    else {
        $("#IsDefault").val(false)
    }
    var _SSLEnabled = $("#SSLEnabled").val();
    var _IsDefault = $("#IsDefault").val();
    console.log(_SSLEnabled);
    console.log(_IsDefault);

    var APIControllerName = 'EmailConfigAPI';
    var formName = '#frmEmailConfig';
    var jQueryDataTableName = '#tblEmailConfig';
    SaveBase(APIControllerName, formName, jQueryDataTableName);
}

var DeleteEmailConfig = function (id) {
    var APIControllerName = 'EmailConfigAPI';
    var jQueryDataTableName = '#tblEmailConfig';
    DeleteBase(id, APIControllerName, jQueryDataTableName);
}