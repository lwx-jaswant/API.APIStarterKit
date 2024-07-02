var Details = function (id) {
    var url = "/EmployeeType/Details?id=" + id;
    $('#titleBigModal').html("Employee Type Details");
    loadBigModal(url);
};


var AddEdit = function (id) {
    var url = "/EmployeeType/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleBigModal').html("Edit Employee Type");
    }
    else {
        $('#titleBigModal').html("Add Employee Type");
    }
    loadBigModal(url);
};

var Save = function () {
    var baseString = 'EmployeeType';
    var APIControllerName = baseString + 'API';
    var formName = '#frm' + baseString;
    var jQueryDataTableName = '#tbl' + baseString;
    SaveBase(APIControllerName, formName, jQueryDataTableName);
}

var Delete = function (id) {
    var baseString = 'EmployeeType';
    var APIControllerName = baseString + 'API';
    var jQueryDataTableName = '#tbl' + baseString;
    DeleteBase(id, APIControllerName, jQueryDataTableName);
}
