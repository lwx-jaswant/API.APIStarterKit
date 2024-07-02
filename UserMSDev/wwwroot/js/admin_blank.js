$(document).ready(function () {
    GetAppInitInfo();
});

var GetAppInitInfo = function () {
    $.ajax({
        type: "POST",
        url: "/Account/GetAppInitInfo",
        data: null,
        success: function (result) {
            sessionStorage.setItem("APIBaseURL", result[0]);
            sessionStorage.setItem("ClientBaseURL", result[1]);
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var getFormDataAsJSONObj = function (form) {
    var unindexed_array = form.serializeArray();
    var indexed_array = {};
    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });
    return indexed_array;
}


var SwalSimpleAlert = function (Message, icontype) {
    Swal.fire({
        title: Message,
        icon: icontype
    });
}

var FieldValidationAlert = function (FieldName, Message, icontype) {
    Swal.fire({
        title: Message,
        icon: icontype,
        onAfterClose: () => {
            $(FieldName).focus();
        }
    });
}