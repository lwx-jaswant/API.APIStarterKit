var ResetPassword = function (ApplicationUserId) {
    if (DemoUserAccountLockAll() == 1) return;
    $('#titleMediumModal').html("<h4>Reset Password</h4>");
    var url = "/UserProfile/ResetPassword?ApplicationUserId=" + ApplicationUserId;
    loadMediumModal(url);
};

var SaveResetPassword = function () {
    if (!$("#frmResetPassword").valid()) {
        return;
    }

    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');

    var _formData = getFormDataAsJSONObj($("#frmResetPassword"));
    var _GetAPIComonInfo = GetAPIComonInfo();

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: _GetAPIComonInfo.BaseURL + "UserProfileAPI/SaveResetPassword",
        headers: { Authorization: 'Bearer ' + _GetAPIComonInfo.AccessToken },
        data: JSON.stringify(_formData),
        dataType: "json",
        success: function (result) {
            var _error = result.substring(0, 5);
            if (_error == 'error') {
                var _result = result.slice(5);
                Swal.fire({
                    title: _result,
                    icon: "warning"
                }).then(function () {
                    setTimeout(function () {
                        $('#OldPassword').focus();
                        $("#btnSave").val("Save");
                        $('#btnSave').removeAttr('disabled');
                    }, 400);
                });
            }
            else {
                Swal.fire({
                    title: result,
                    icon: "success"
                }).then(function () {
                    document.getElementById("btnClose").click();
                });
            }
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var EditUserProfile = function (id) {
    if (DemoUserAccountLockAll() == 1) return;
    var url = "/UserManagement/AddEditUserAccount?id=" + id;
    $('#titleExtraBigModal').html("Add User");
    loadExtraBigModal(url);

    setTimeout(function () {
        $('#FirstName').focus();
    }, 200);
};