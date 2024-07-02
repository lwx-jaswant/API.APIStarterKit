var Edit = function (id) {
    var url = "/IdentitySetting/Edit?id=" + id;
    $('#titleExtraBigModal').html("Edit Default Identity Option");
    loadExtraBigModal(url);
};


var UpdateDefaultIdentityOptions = function () {
    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');

    var _frmDefaultIdentityOptions = $("#frmDefaultIdentityOptions").serialize();
    $.ajax({
        type: "POST",
        url: "/IdentitySetting/Edit",
        data: _frmDefaultIdentityOptions,
        success: function (result) {
            Swal.fire({
                title: result.AlertMessage,
                icon: "success"
            }).then(function () {
                document.getElementById("btnClose").click();
                setTimeout(function () {
                    $("#tblDefaultIdentityOptions").load("/IdentitySetting/Index #tblDefaultIdentityOptions");
                }, 100);
            });
        },
        error: function (errormessage) {
            $("#btnSave").val("Save");
            $('#btnSave').removeAttr('disabled');
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}
