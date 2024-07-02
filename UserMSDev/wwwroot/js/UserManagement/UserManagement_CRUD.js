var funAction = function (Id) {
    if (DemoUserAccountLockAll() == 1) return;
    var _Action = $("#" + Id).val();
    if (_Action == 1)
        AddEditUserAccount(Id);
    else if (_Action == 2)
        ResetPasswordAdmin(Id);
    else if (_Action == 3)
        UpdateUserRole(Id);
    else if (_Action == 4)
        DeleteUserAccount(Id);
    $("#" + Id).prop('selectedIndex', 0);
};

var ViewUserDetails = function (Id) {
    var url = "/UserManagement/ViewUserDetails?Id=" + Id;
    $('#titleExtraBigModal').html("User Details ");
    loadExtraBigModal(url);
};

var AddEditUserAccount = function (id) {
    if (DemoUserAccountLockAll() == 1) return;
    var url = "/UserManagement/AddEditUserAccount?id=" + id;
    if (id > 0) {
        $('#titleExtraBigModal').html("Edit User");
    }
    else {
        $('#titleExtraBigModal').html("Add User");
    }
    loadExtraBigModal(url);

    setTimeout(function () {
        $('#FirstName').focus();
    }, 200);
};

var SaveUser = function () {
    if (!$("#ApplicationUserForm").valid()) {
        return;
    }

    var _UserProfileId = $("#Id").val();
    if (_UserProfileId > 0) {
        $("#btnSave").prop('value', 'Updating User');
    }
    else {
        $("#btnSave").prop('value', 'Creating User');
    }
    $('#btnSave').prop('disabled', true);

    var _PreparedFormObj = PreparedFormObj();

    $.ajax({
        type: "POST",
        url: "/UserManagement/SaveAddEditUserAccount",
        data: _PreparedFormObj,
        processData: false,
        contentType: false,
        success: function (result) {
            $('#btnSave').prop('disabled', false);
            $("#btnSave").prop('value', 'Save');
            if (result.IsSuccess) {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "success"
                }).then(function () {
                    document.getElementById("btnAddEditUserAccountClose").click();
                    if (result.CurrentURL == "/") {
                        setTimeout(function () {
                            $("#tblRecentRegisteredUser").load("/ #tblRecentRegisteredUser");
                        }, 1000);
                    }
                    else if (result.CurrentURL == "/UserProfile/Index") {
                        $("#divUserProfile").load("/UserProfile/Index #divUserProfile");
                    }
                    else {
                        $('#tblUserManagement').DataTable().ajax.reload();
                    }
                });
            }
            else {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "warning"
                }).then(function () {
                    setTimeout(function () {
                        $('#Email').focus();
                    }, 400);
                });
            }
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var DeleteUserAccount = function (id) {
    if (DemoUserAccountLockAll() == 1) return;
    var baseString = 'UserManagement';
    var APIControllerName = baseString + 'API';
    var jQueryDataTableName = '#tbl' + baseString;
    DeleteBase(id, APIControllerName, jQueryDataTableName);
}


var UpdateUserRole = function (id) {
    if (DemoUserAccountLockAll() == 1) return;
    
    $('#titleExtraBigModal').html("<h4>Manage Page Access</h4>");
    var url = "/ManageUserRoles/UpdateUserRoleInUM?id=" + id;
    loadExtraBigModal(url);
};

var SaveUpdateUserRole = function () {
    $("#btnUpdateRole").val("Please Wait");
    $('#btnUpdateRole').attr('disabled', 'disabled');

    var _frmManageRole = $("#frmManageRole").serialize();

    $.ajax({
        type: "POST",
        url: "/ManageUserRoles/SaveUpdateUserRoleInUM",
        data: _frmManageRole,
        success: function (result) {
            $("#btnUpdateRole").val("Save");
            $('#btnUpdateRole').removeAttr('disabled');
            if (result.IsSuccess) {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "success"
                }).then(function () {
                    document.getElementById("btnClose").click();
                    $('#tblUserManagement').DataTable().ajax.reload();
                });
            }
            else {
                Swal.fire({
                    title: result.AlertMessage,
                    icon: "warning"
                }).then(function () {
                });
            }
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var ResetPasswordAdmin = function (id) {
    if (DemoUserAccountLockAll() == 1) return;
    $('#titleMediumModal').html("<h4>Reset Password</h4>");
    var url = "/UserManagement/ResetPasswordAdmin?id=" + id;
    loadMediumModal(url);
};

var SaveResetPasswordAdmin = function () {
    if (!FieldValidation('#NewPassword')) {
        FieldValidationAlert('#NewPassword', 'New Password is Required.', "warning");
        return;
    }
    if (!FieldValidation('#ConfirmPassword')) {
        FieldValidationAlert('#ConfirmPassword', 'Confirm Password is Required.', "warning");
        return;
    }

    if (!$("#frmResetPassword").valid()) {
        FieldValidationAlert('#ConfirmPassword', 'Please fill up all input properly.', "warning");
        return;
    }

    $("#btnResetPasswordAdmin").val("Please Wait");
    $('#btnResetPasswordAdmin').attr('disabled', 'disabled');

    var _formData = getFormDataAsJSONObj($("#frmResetPassword"));
    var _GetAPIComonInfo = GetAPIComonInfo();
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: _GetAPIComonInfo.BaseURL + "UserManagementAPI/ResetPasswordAdmin",
        headers: { Authorization: 'Bearer ' + _GetAPIComonInfo.AccessToken },
        data: JSON.stringify(_formData),
        dataType: "json",
        success: function (result) {
            $("#btnResetPasswordAdmin").val("Save");
            $('#btnResetPasswordAdmin').removeAttr('disabled');

            var _error = result.substring(0, 5);
            if (_error == 'error') {
                var _result = result.slice(5);
                Swal.fire({
                    title: _result,
                    icon: "warning"
                }).then(function () {
                    setTimeout(function () {
                        $('#NewPassword').focus();
                    }, 400);
                });
            }
            else {
                Swal.fire({
                    title: result,
                    icon: "success"
                }).then(function () {
                    document.getElementById("btnResetPasswordClose").click();
                });
            }
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var PreparedFormObj = function () {
    var _FormData = new FormData()
    _FormData.append('Id', $("#Id").val())
    _FormData.append('Id', $("#Id").val())
    _FormData.append('ApplicationUserId', $("#ApplicationUserId").val())
    _FormData.append('ProfilePictureDetails', $('#ProfilePictureDetails')[0].files[0])

    _FormData.append('FirstName', $("#FirstName").val())
    _FormData.append('LastName', $("#LastName").val())
    _FormData.append('EmployeeTypeId', $("#EmployeeTypeId").val())
    _FormData.append('PhoneNumber', $("#PhoneNumber").val())
    _FormData.append('Email', $("#Email").val())
    _FormData.append('PasswordHash', $("#PasswordHash").val())
    _FormData.append('ConfirmPassword', $("#ConfirmPassword").val())
    _FormData.append('Address', $("#Address").val())
    _FormData.append('Country', $("#Country").val())
    _FormData.append('RoleId', $("#RoleId").val())
    _FormData.append('IsApprover', $("#IsApprover").val())

    _FormData.append('EmployeeId', $("#EmployeeId").val())
    _FormData.append('DateOfBirth', $("#DateOfBirth").val())
    _FormData.append('Designation', $("#Designation").val())
    _FormData.append('Department', $("#Department").val())
    _FormData.append('SubDepartment', $("#SubDepartment").val())
    _FormData.append('JoiningDate', $("#JoiningDate").val())
    _FormData.append('LeavingDate', $("#LeavingDate").val())

    _FormData.append('CurrentURL', $("#CurrentURL").val())
    return _FormData;
}