var Details = function (id) {
    var url = "/LoginHistory/Details?id=" + id;
    $('#titleBigModal').html("Login History Details");
    loadBigModal(url);
};

