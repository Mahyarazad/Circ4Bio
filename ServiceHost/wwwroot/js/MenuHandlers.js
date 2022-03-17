"use strict";
//var host = "https://localhost:5001";
var host = "http://www.circ4bio.com";

$('#mobile-home-menu-close').on('click', function () {
    $('#mobile-home-menu').css('visibility', 'hidden');
    $('#mobile-home-menu').css('opacity', '0');
    $('#mobile-home-menu').css('z-index', '-1');
    $('#mobile-home-menu').addClass('top-0');
    $('#mobile-home-menu').removeClass("top-16");
})

$('#mobile-home-menu-open').on('click', function () {
    $('#mobile-home-menu').css('visibility', 'visible');
    $('#mobile-home-menu').removeClass('top-0');
    $('#mobile-home-menu').addClass("top-16");
    $('#mobile-home-menu').css('opacity', '1');
    $('#mobile-home-menu').css('z-index', '10');
})


$("#desktop-user-menu-trigger").on('mouseover', function (e) {
    e.preventDefault;
    $("#desktop-user-menu").css('visibility', 'visible');
    $("#desktop-user-menu").css('opacity', '1');
    $("#desktop-user-menu").css('z-index', '50');
    $("#desktop-user-menu").removeClass('top-0');
    $("#desktop-user-menu").addClass("top-10");
    $('#notification-panel').css('z-index', '1');
});

$("#desktop-user-menu-trigger").on('mouseleave', function (e) {

    var offsetLeft = $("#desktop-user-menu-trigger").offset().left;
    var offsetTop = $("#desktop-user-menu-trigger").offset().top;
    var elementWidth = $("#desktop-user-menu-trigger").outerWidth();

    if (e.clientX > offsetLeft + elementWidth || e.clientX < offsetLeft || e.pageY < offsetTop) {
        $("#desktop-user-menu").css('visibility', 'hidden');
        $("#desktop-user-menu").css('opacity', '0');
        $("#desktop-user-menu").css('z-index', '-1');
        $("#desktop-user-menu").addClass('top-0');
        $("#desktop-user-menu").removeClass("top-10");
    }
});

$("#desktop-user-menu").on('mouseleave', function () {
    $("#desktop-user-menu").css('visibility', 'hidden');
    $("#desktop-user-menu").css('opacity', '0');
    $("#desktop-user-menu").css('z-index', '-1');
    $("#desktop-user-menu").addClass('top-0');
    $("#desktop-user-menu").removeClass("top-10");
});


// Menu opens from top
$('#mobile-menu-trigger').on('click', function () {
    var status = $('#mobile-menu').css("visibility");
    if (status === 'hidden') {
        $('#mobile-menu').css('visibility', 'visible');
        //        $('#mobile-menu').removeClass('translate-y-full');
        $('#mobile-menu').removeClass('top-0');
        $('#mobile-menu').addClass("top-16");
        $('#mobile-menu').css('opacity', '1');
        $('#mobile-menu').css('z-index', '50');
        $('#notification-panel').css('z-index', '1');
    } else {
        $('#mobile-menu').css('visibility', 'hidden');
        $('#mobile-menu').css('opacity', '0');
        $('#mobile-menu').css('z-index', '-1');
        $('#mobile-menu').addClass('top-0');
        $('#mobile-menu').removeClass("top-16");
        //        $('#mobile-menu').addClass("translate-y-full");
    }
})


//$("#notification-handler").on('mouseover', function (e) {
//    e.preventDefault;
//    $('#notification-panel').css('visibility', 'visible');
//    $('#notification-panel').removeClass('-translate-y-[20rem]');
//    $('#notification-panel').css('opacity', '1');
//    $('#notification-panel').css('z-index', '10');
//});
//
//$("#notification-handler").on('mouseleave', function (e) {
//
//    var offsetLeft = $("#notification-handler").offset().left;
//    var offsetTop = $("#notification-handler").offset().top;
//    var elementWidth = $("#notification-handler").outerWidth();
//
//    if (e.clientX > offsetLeft + elementWidth || e.clientX < offsetLeft || e.pageY < offsetTop) {
//        $('#notification-panel').css('visibility', 'hidden');
//        $('#notification-panel').addClass('-translate-y-[20rem]');
//        $('#notification-panel').css('opacity', '0');
//        $('#notification-panel').css('z-index', '-1');
//    }
//});

$("#notification-handler").on('click',
    function (e) {
        var status = $('#notification-panel').css('visibility');
        if (status === 'hidden') {
            $('#notification-panel').css('visibility', 'visible');
            $('#notification-panel').removeClass('-translate-y-[20rem]');
            $('#notification-panel').css('opacity', '1');
            $('#notification-panel').css('z-index', '10');
        } else {
            $('#notification-panel').css('visibility', 'hidden');
            $('#notification-panel').addClass('-translate-y-[20rem]');
            $('#notification-panel').css('opacity', '0');
            $('#notification-panel').css('z-index', '-1');
        }
    });

//$("#notification-wrapper").on('mouseout', function (e) {
//    $('#notification-panel').css('visibility', 'hidden');
//    $('#notification-panel').addClass('-translate-y-[20rem]');
//    $('#notification-panel').css('opacity', '0');
//    $('#notification-panel').css('z-index', '-1');
//});


$('#notification-handler-mobile').on('click', function () {
    var status = $('#notification-panel').css("visibility");
    if (status === 'hidden') {
        $('#notification-panel').css('visibility', 'visible');
        $('#notification-panel').removeClass('-translate-y-[20rem]');
        $('#notification-panel').css('opacity', '1');
        $('#notification-panel').css('z-index', '10');
    } else {
        $('#notification-panel').css('visibility', 'hidden');
        $('#notification-panel').addClass('-translate-y-[20rem]');
        $('#notification-panel').css('opacity', '0');
        $('#notification-panel').css('z-index', '-1');
    }
})

$(document).ready(() => {
    $('#form').submit(function (e) {
        if ($(this).valid()) {
            $("#overlay").removeClass('invisible');
            $("#overlay").removeClass('hidden');
            $("#overlay").show();
        } else {
        }
    });
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
        //        $("#modal").hide();
        hideModal();
        //        $("#user-menu").css('visibility', 'none');
        $('#notification-panel').css('visibility', 'hidden');
        $('#notification-panel').addClass('-translate-y-[20rem]');
        $('#notification-panel').css('opacity', '0');
        $('#notification-panel').css('z-index', '-1');
    }
});

function gototop() {
    hideModal();
    loading();
    document.body.scrollTop = 0; // For Safari
    document.documentElement.scrollTop = 0;
    // For Chrome, Firefox, IE and Opera
}

function gotoTop() {
    hideModal();
    var element = document.getElementById('operation-result');
    element.scrollIntoView({ behavior: "smooth" });
}


function MarketSelect() {
    $('#market').addClass("bg-black");
}
function DashboardSelect() {
    $('#dashboard').addClass("bg-black");
}

function handleFilter() {
    var stat = $("#filter-section-0").css('visibility');
    if (stat === 'hidden') {
        $("#shrink-div").removeClass('h-10');
        $("#svg-plus").addClass('invisible');
        $("#svg-minus").removeClass('invisible');
        $("#filter-section-0").removeClass('invisible');
        $('#filter-section-0').removeClass('-translate-y-[5rem]');
        $('#filter-section-0').css('opacity', '1');
    } else {
        $("#shrink-div").addClass('h-10');
        $("#svg-plus").removeClass('invisible');
        $("#svg-minus").addClass('invisible');
        $("#filter-section-0").addClass('invisible');
        $('#filter-section-0').addClass('-translate-y-[5rem]');
        $('#filter-section-0').css('opacity', '0');
    }
};

$("#grid-size-handler").on('click', () => {
    var stat = $("#grid-size-container").css('visibility');
    if (stat === 'hidden') {
        $("#grid-size-container").removeClass('invisible');
        $("#grid-size-container").removeClass('scale-95');
        $('#grid-size-container').css('opacity', '1');
    } else {
        $("#grid-size-container").addClass('invisible');
        $("#grid-size-container").addClass('scale-95');
        $('#grid-size-container').css('opacity', '0');
    }
})

$(document).on('keydown', function (e) {
    if (e.keyCode === 27) {
        // ESC
        $("#grid-size-container").addClass('invisible');
        $("#grid-size-container").addClass('scale-95');
        $('#grid-size-container').css('opacity', '0');
        $("#grid-size-container-unlogged").addClass('invisible');
        $("#grid-size-container-unlogged").addClass('scale-95');
        $('#grid-size-container-unlogged').css('opacity', '0');
    }
});

$("#grid-size-handler-unlogged").on('click', () => {
    var stat = $("#grid-size-container-unlogged").css('visibility');
    if (stat === 'hidden') {
        $("#grid-size-container-unlogged").removeClass('invisible');
        $("#grid-size-container-unlogged").removeClass('scale-95');
        $('#grid-size-container-unlogged').css('opacity', '1');
    } else {
        $("#grid-size-container-unlogged").addClass('invisible');
        $("#grid-size-container-unlogged").addClass('scale-95');
        $('#grid-size-container-unlogged').css('opacity', '0');
    }
})
