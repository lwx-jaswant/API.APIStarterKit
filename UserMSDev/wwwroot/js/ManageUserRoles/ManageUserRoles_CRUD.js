var Details = function (id) {
    var url = "/ManageUserRoles/Details?id=" + id;
    $('#titleExtraBigModal').html("Role Details");
    loadExtraBigModal(url);
};

var AddEdit = function (id) {
    if (DemoUserAccountLockAll() == 1) return;
    var url = "/ManageUserRoles/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleExtraBigModal').html("Edit Role");
    }
    else {
        $('#titleExtraBigModal').html("Add Role");
    }
    loadExtraBigModal(url);
};

var Save = function () {
    if (!$("#frmUserRoles").valid()) {
        return;
    }

    var _frmUserRoles = $("#frmUserRoles").serialize();
    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');
    $.ajax({
        type: "POST",
        url: "/ManageUserRoles/AddEdit",
        data: _frmUserRoles,
        success: function (result) {
            console.log(result);
            if (result.IsSuccess) {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "success"
                }).then(function () {
                    document.getElementById("btnClose").click();
                    $("#btnSave").val("Save");
                    $('#btnSave').removeAttr('disabled');
                    $('#tblManageUserRoles').DataTable().ajax.reload();
                });
            }
            else {
                SwalSimpleAlert(result.AlertMessage, "warning");
            }
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}


var Delete = function (id) {
    if (DemoUserAccountLockAll() == 1) return;
    var baseString = 'ManageUserRoles';
    var APIControllerName = baseString + 'API';
    var jQueryDataTableName = '#tbl' + baseString;
    DeleteBase(id, APIControllerName, jQueryDataTableName);
}