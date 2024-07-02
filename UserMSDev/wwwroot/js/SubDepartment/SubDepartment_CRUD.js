var Details = function (id) {
    var url = "/SubDepartment/Details?id=" + id;
    $('#titleBigModal').html("Sub Department Details");
    loadBigModal(url);
};

var AddEdit = function (id) {
    var url = "/SubDepartment/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleBigModal').html("Edit Sub Department");
    }
    else {
        $('#titleBigModal').html("Add Sub Department");
    }
    loadBigModal(url);
};

var Save = function () {
    var baseString = 'SubDepartment';
    var APIControllerName = baseString + 'API';
    var formName = '#frm' + baseString;
    var jQueryDataTableName = '#tbl' + baseString;
    SaveBase(APIControllerName, formName, jQueryDataTableName);
}

var Delete = function (id) {
    var baseString = 'SubDepartment';
    var APIControllerName = baseString + 'API';
    var jQueryDataTableName = '#tbl' + baseString;
    DeleteBase(id, APIControllerName, jQueryDataTableName);
}