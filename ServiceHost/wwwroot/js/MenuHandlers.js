﻿
$('#mobile-menu-close').on('click', function () {
    $('#mobile-menu').css('display', 'none');
})

$('#main-menu').on('click', function () {
    $('#mobile-menu').css('display', 'block');
})

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
        $('#mobile-menu').css('z-index', '0');
    } else {
        $('#mobile-menu').css('display', 'block');
        $('#mobile-menu').css('z-index', '999');
    }
})

$('#notification-handler').on('click', function () {
    var status = $('#notification-panel').css("display");
    if (status === 'block') {
        $('#notification-panel').css('display', 'none');
        $('#notification-panel').removeClass('translate-y-0');
        $('#notification-panel').removeClass('opacity-100');
        $('#notification-panel').addClass('translate-y-2');
        $('#notification-panel').addClass('opacity-0');
    } else {
        $('#notification-panel').css('display', 'block');
        $('#notification-panel').css('z-index', '10');
        $('#notification-panel').removeClass('translate-y-2');
        $('#notification-panel').removeClass('opacity-0');
        $('#notification-panel').addClass('translate-y-0');
        $('#notification-panel').addClass('opacity-100');
        $('#notification-panel').addClass('duration-1000');
        $('#notification-panel').addClass('transition');
        $('#notification-panel').addClass('ease-out');
        $('#notification-panel').addClass('transform');
    }
})

$('#notification-handler-mobile').on('click', function () {
    var status = $('#notification-panel').css("display");
    if (status === 'block') {
        $('#notification-panel').css('display', 'none');
        $('#notification-panel').removeClass('translate-y-0');
        $('#notification-panel').removeClass('opacity-100');
        $('#notification-panel').addClass('translate-y-2');
        $('#notification-panel').addClass('opacity-0');
    } else {
        $('#notification-panel').css('display', 'block');
        $('#notification-panel').css('z-index', '10');
        $('#notification-panel').removeClass('translate-y-2');
        $('#notification-panel').removeClass('opacity-0');
        $('#notification-panel').addClass('translate-y-0');
        $('#notification-panel').addClass('opacity-100');
        $('#notification-panel').addClass('duration-1000');
        $('#notification-panel').addClass('transition');
        $('#notification-panel').addClass('ease-out');
        $('#notification-panel').addClass('transform');
    }
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


$("button[id='save']").on('click',
    function () {
        window.scrollTo({
            top: 0,
            behavior: 'smooth',
        })
    });

$("div[id='main-table_wrapper").scroll(function () {
    $("span[id='view-all").addClass("text-sky-800");
    $("span[id='view-all").removeClass("text-white");
});

function listingIsService() {
    var checkVal = $("input[id='is-service']").prop('checked');
    if (checkVal) {
        $("div[id='listing-amount']").hide();
    } else {
        $("div[id='listing-amount']").show();
    }
}

//$(document).on('click', function (e) {
//    if ($(e.target).closest(elem).length === 0) {
//        $("#modal").hide();
//    }
//});

$(document).on('keydown', function (e) {
    if (e.keyCode === 27) {
        // ESC
        window.location.hash = "##";
        $("#modal").hide();
    }
});





