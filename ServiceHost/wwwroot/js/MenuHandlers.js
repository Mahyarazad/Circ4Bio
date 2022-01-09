$(document).ready(function () {
});


$('#menu-trigger').on('click', function () {
    var status = $('#user-menu').css("display");
    if (status === 'block') {
        $('#user-menu').css('display', 'none');
    } else {
        $('#user-menu').css('display', 'block');
    }
})

$('#mobile-menu-trigger').on('click', function () {
    var status = $('#mobile-menu').css("display");
    if (status === 'block') {
        $('#mobile-menu').css('display', 'none');
    } else {
        $('#mobile-menu').css('display', 'block');
    }
})