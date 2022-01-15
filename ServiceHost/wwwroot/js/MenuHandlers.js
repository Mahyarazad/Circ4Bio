
$('#menu-trigger').on('click', function () {
    var status = $('#user-menu').css("display");
    if (status === 'block') {
        $('#user-menu').css('display', 'none');
    } else {
        $('#user-menu').css('display', 'block');
    }
})

$('#main-menu').on('click', function () {
    $('#mobile-menu').css('display', 'block');
    $('#mobile-menu').css('z-index', '999');
})
$('#mobile-menu-close').on('click', function () {
    $('#mobile-menu').css('display', 'none');
})


function loading() {
    $(".overlay").show();
}

$('#login-button').on('click', function () {
    setInterval(function () {
        if ($('#password-error').length == 1 || $('#user-email-error').length == 1) {
            $(".overlay").hide();
        };
    }, 100)
})
$('#register-button').on('click', function () {
    setInterval(function () {
        if ($('#password-error').length == 1 || $('#user-email-error').length == 1
            || $('#repassword-error').length == 1 || $('#user-type-error').length == 1) {
            $(".overlay").hide();
        };
    }, 100)
})

$('#contact-us-button').on('click', function () {
    setInterval(function () {
        if ($('#Command_FullName-error').length == 1 || $('#Command_Email-error').length == 1
            || $('#Command_Subject-error').length == 1 || $('#Command_Body-error').length == 1) {
            $(".overlay").hide();
        };
    }, 100)
})



