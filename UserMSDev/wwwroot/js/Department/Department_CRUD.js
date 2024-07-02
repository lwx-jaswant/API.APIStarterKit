var Details = function (id) {
    var url = "/Department/Details?id=" + id;
    $('#titleBigModal').html("Department Details");
    loadBigModal(url);
};

var AddEdit = function (id) {
    var url = "/Department/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleBigModal').html("Edit Department");
    }
    else {
        $('#titleBigModal').html("Add Department");
    }
    loadBigModal(url);
};

var Save = function () {
    if (!$("#frmDepartment").valid()) {
        return;
    }
    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');

    var _frmDepartment = getFormDataAsJSONObj($("#frmDepartment"));
    var _GetAPIComonInfo = GetAPIComonInfo();

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: _GetAPIComonInfo.BaseURL + "DepartmentAPI/AddEdit",
        headers: { Authorization: 'Bearer ' + _GetAPIComonInfo.AccessToken },
        data: JSON.stringify(_frmDepartment),
        dataType: "json",
        success: function (result) {
            if (result.isSuccess) {
                Swal.fire({
                    title: result.alertMessage,
                    icon: "success"
                }).then(function () {
                    document.getElementById("btnClose").click();
                    $("#btnSave").val("Save");
                    $('#btnSave').removeAttr('disabled');
                    $('#tblDepartment').DataTable().ajax.reload();
                });
            }
            else {
                SwalSimpleAlert(result.alertMessage, "warning");
            }
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var Delete = function (id) {
    var APIControllerName = 'DepartmentAPI';
    var jQueryDataTableName = '#tblDepartment';
    DeleteBase(id, APIControllerName, jQueryDataTableName);
}
