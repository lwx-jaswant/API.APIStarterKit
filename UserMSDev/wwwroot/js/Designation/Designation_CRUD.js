var Details = function (id) {
    var url = "/Designation/Details?id=" + id;
    $('#titleBigModal').html("Designation Details");
    loadBigModal(url);
};

var AddEdit = function (id) {
    var url = "/Designation/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleBigModal').html("Edit Designation");
    }
    else {
        $('#titleBigModal').html("Add Designation");
    }
    loadBigModal(url);
};

var Save = function () {
    var APIControllerName = 'DesignationAPI';
    var formName = '#frmDesignation';
    var jQueryDataTableName = '#tblDesignation';
    SaveBase(APIControllerName, formName, jQueryDataTableName);
}

var Delete = function (id) {
    var APIControllerName = 'DesignationAPI';
    var jQueryDataTableName = '#tblDesignation';
    DeleteBase(id, APIControllerName, jQueryDataTableName);
}
