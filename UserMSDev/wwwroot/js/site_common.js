var GetAPIComonInfo = function () {
    var _AccessToken = sessionStorage.getItem("AccessToken");
    var _BaseURL = sessionStorage.getItem("APIBaseURL");
    if (_AccessToken == null || _AccessToken == '' || _BaseURL == null || _BaseURL == '' || _BaseURL == 'undefined') {
        location.href = "/Account/Login";
    }

    var APIProjectData = {};
    APIProjectData.BaseURL = _BaseURL;
    APIProjectData.AccessToken = _AccessToken;
    return APIProjectData;
}

//Need to Update it
var GetToken = function () {
    var _AccessToken = sessionStorage.getItem("AccessToken");
    if (_AccessToken == null) {

    }
}

var RedirectToLoginPage = function () {
    location.href = "/Account/Login";
}

var getFormDataAsJSONObj = function (form) {
    var unindexed_array = form.serializeArray();
    var indexed_array = {};
    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });
    return indexed_array;
}

var SaveBase = function (APIControllerName, formName, jQueryDataTableName) {
    if (!$(formName).valid()) {
        return;
    }
    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');

    var _formData = getFormDataAsJSONObj($(formName));
    var _GetAPIComonInfo = GetAPIComonInfo();

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: _GetAPIComonInfo.BaseURL + APIControllerName + "/AddEdit",
        headers: { Authorization: 'Bearer ' + _GetAPIComonInfo.AccessToken },
        data: JSON.stringify(_formData),
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
                    $(jQueryDataTableName).DataTable().ajax.reload();
                });
            }
            else {
                $("#btnSave").val("Save");
                $('#btnSave').removeAttr('disabled');
                SwalSimpleAlert(result.alertMessage, "warning");
            }
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var DeleteBase = function (id, APIControllerName, jQueryDataTableName) {
    Swal.fire({
        title: 'Do you want to delete this item?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.value) {
            var _GetAPIComonInfo = GetAPIComonInfo();
            console.log(_GetAPIComonInfo.BaseURL + APIControllerName + "/Delete/" + id);
            $.ajax({
                type: "DELETE",
                url: _GetAPIComonInfo.BaseURL + APIControllerName + "/Delete/" + id,
                headers: { Authorization: 'Bearer ' + _GetAPIComonInfo.AccessToken },
                success: function (result) {
                    if (result.isSuccess) {
                        Swal.fire({
                            title: result.alertMessage,
                            icon: 'info',
                            onAfterClose: () => {
                                $(jQueryDataTableName).DataTable().ajax.reload();
                            }
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
    });
};


var GetDropDownDataBase = function (ItemName, APIMethodName) {
    var APIControllerName = 'CommonDataAPI';
    var _GetAPIComonInfo = GetAPIComonInfo();
    $.ajax({
        type: "GET",
        url: _GetAPIComonInfo.BaseURL + APIControllerName + APIMethodName,
        headers: { Authorization: 'Bearer ' + _GetAPIComonInfo.AccessToken },
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                var _SemesterId = document.getElementById(ItemName);
                var _option = document.createElement("option");
                _option.text = data[i].name;
                _option.value = data[i].id;
                _SemesterId.add(_option);
            }
        },
        error: function (response) {
            SwalSimpleAlert(response.responseText, "warning");
        }
    });
};