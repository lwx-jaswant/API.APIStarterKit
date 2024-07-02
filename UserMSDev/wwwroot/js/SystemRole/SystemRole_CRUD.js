var AddNewRole = function () {
    if (DemoUserAccountLockAll() == 1) return;
    var url = "/SystemRole/AddNewRole";
    $('#titleBigModal').html("Add New Role");
    loadBigModal(url);
};

var SaveAddNewRole = function () {
    if (!$("#frmAddNewRole").valid()) {
        return;
    }

    var _frmAddNewRole = $("#frmAddNewRole").serialize();
    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');
    $.ajax({
        type: "POST",
        url: "/SystemRole/SaveAddNewRole",
        data: _frmAddNewRole,
        success: function (result) {
            if (result.IsSuccess) {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "success"
                }).then(function () {
                    document.getElementById("btnClose").click();
                    $("#btnSave").val("Save");
                    $('#btnSave').removeAttr('disabled');
                    $('#tblSystemRole').DataTable().ajax.reload();
                });
            }
            else {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "warning"
                }).then(function () {
                    setTimeout(function () {
                        $('#RoleName').focus();
                        $("#btnSave").val("Save");
                        $('#btnSave').removeAttr('disabled');
                    }, 400);
                });
            }
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var DeleteRole = function (RoleId) {
    if (DemoUserAccountLockAll() == 1) return;
    var baseString = 'SystemRole';
    var APIControllerName = baseString + 'API';
    var jQueryDataTableName = '#tbl' + baseString;
    DeleteBase(RoleId, APIControllerName, jQueryDataTableName);
}
