$(document).ready(function () {
    document.title = 'User Account';

    $("#tblUserManagement").DataTable({
        paging: true,
        select: true,
        "order": [[0, "desc"]],
        dom: 'Bfrtip',


        buttons: [
            'pageLength',
        ],


        "processing": true,
        "serverSide": true,
        "filter": true, //Search Box
        "orderMulti": false,
        "stateSave": true,

        "ajax": {
            "url": "/UserManagement/GetDataTabelData",
            "type": "POST",
            "datatype": "json"
        },

        "columns": [
            {
                data: "Id", "name": "Id", render: function (data, type, row) {
                    return "<a href='#' class='fa fa-eye' onclick=ViewUserDetails('" + row.Id + "');>" + row.Id + "</a>";
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='#' class='d-block' onclick=ViewImage('" + row.ProfilePicture + "','User_Image');><div class='image'><img src='" + row.ProfilePicture + "' class='img-circle elevation-2 imgCustom50px' alt='Asset Image'></div></a>";
                }
            },
            {
                data: "Id", "name": "Id", render: function (data, type, row) {
                    return "<a href='#' onclick=ViewUserDetails('" + row.Id + "');>" + row.FirstName + "</a>";
                }
            },
            { "data": "LastName", "name": "LastName", "autoWidth": true },
            { "data": "PhoneNumber", "name": "PhoneNumber", "autoWidth": true },
            { "data": "Email", "name": "Email", "autoWidth": true },
            {
                "data": "CreatedDate",
                "name": "CreatedDate",
                "autoWidth": true,
                "render": function (data) {
                    var date = new Date(data);
                    var month = date.getMonth() + 1;
                    return (month.length > 1 ? month : month) + "/" + date.getDate() + "/" + date.getFullYear();
                }
            },

            {
                data: null, render: function (data, type, row) {
                    return "<select id='" + row.Id + "' onchange=funAction('" + row.Id + "'); class='btn-sm' style='width: 80px;'>" +
                        "<option value='0'></option>" +
                        "<option value='1'>Edit</option>" +
                        "<option value='2'>Reset Password</option>" +
                        "<option value='3'>Manage Page Access</option>" +
                        "<option value='4'>Delete</option>" +
                        "</select>";
                }
            },
        ],

        'columnDefs': [
            {
                'targets': [1, 7],
                'orderable': false,
            },
            {
                "width": "10px",
                "targets": 7
            },
        ],

        "lengthMenu": [[20, 10, 15, 25, 50, 100, 200], [20, 10, 15, 25, 50, 100, 200]]
    });

});
