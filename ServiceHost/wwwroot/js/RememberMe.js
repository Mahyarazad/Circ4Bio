$(document).ready(function () {
    const userToken = $.cookie("user-token");
    const user = JSON.parse(userToken);
    $("#user-email").val(user.email);
    $("#password").val(user.password);
    $("#remember-me").prop('checked', true);

});