"use strict";

function loading() {
    $("#overlay").removeClass('hidden');
    $("#overlay").show();
}

function loaded() {
    $("#overlay").addClass('hidden');
    $("#overlay").css('display', 'none');
}

$("#operation-result-message").change(function () {
    $("#overlay").addClass('hidden');
});

$("#operation-result-failed-message").change(function () {
    $("#overlay").addClass('hidden');
});

function closeLandingMobileMenu() {
    $('#mobile-home-menu').css('visibility', 'hidden');
    $('#mobile-home-menu').css('opacity', '0');
    $('#mobile-home-menu').css('z-index', '-1');
    $('#mobile-home-menu').addClass('top-0');
    $('#mobile-home-menu').removeClass("top-16");
}

function openLandingMobileMenu() {
    $('#mobile-home-menu').css('visibility', 'visible');
    $('#mobile-home-menu').removeClass('top-0');
    $('#mobile-home-menu').addClass("top-16");
    $('#mobile-home-menu').css('opacity', '1');
    $('#mobile-home-menu').css('z-index', '10');
}


//$('#mobile-home-menu-open').on('click', function () {
//    openLandingMobileMenu();
//});
//
//
//$('#mobile-home-menu-close').on('click', function () {
//    closeLandingMobileMenu();
//});

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


$(document).ready(() => {
    $("#overlay").addClass('hidden');
    $("#overlay").hide();
    $('#form').submit(function (e) {
        if ($(this).valid()) {
            $("#overlay").removeClass('hidden');
            $("#overlay").show();
        } else {
            $("#overlay").addClass('hidden');
            $("#overlay").hide();
        }
    });
});


$("button[id='save']").on('click',
    function () {
        window.scrollTo({
            top: 0,
            behavior: 'smooth',
        });
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

$(document).on('keydown', function (e) {
    var stat = $('#modal').css('visibility', 'visible');
    if (e.keyCode === 27 && stat !== 'hidden') {
        // ESC
        hideModal();
        closeNotificationPanel();
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

function scrolltotop() {
    $(window).scrollTop(0);
}

function MarketSelect() {
    $('#market').addClass("bg-black");
}
function DashboardSelect() {
    $('#dashboard').addClass("bg-black");
}

function handleFilter(id) {
    var stat = $(`#filter-section-${id}`).css('visibility');
    if (stat === 'hidden') {
        $(`#shrink-div-${id}`).removeClass('h-10');
        $(`#svg-plus-${id}`).addClass('invisible');
        $(`#svg-minus-${id}`).removeClass('invisible');
        $(`#filter-section-${id}`).removeClass('invisible');
        $(`#filter-section-${id}`).removeClass('-translate-y-[5rem]');
        $(`#filter-section-${id}`).css('opacity', '1');
    } else {
        $(`#shrink-div-${id}`).addClass('h-10');
        $(`#svg-plus-${id}`).removeClass('invisible');
        $(`#svg-minus-${id}`).addClass('invisible');
        $(`#filter-section-${id}`).css('opacity', '0');
        $(`#filter-section-${id}`).addClass('invisible');
        $(`#filter-section-${id}`).addClass('-translate-y-[5rem]');

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

$("#grid-size-handler-unlogged").on('click',
    () => {
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
    });


$("#IsFiltered").on("click", function () {
    $("#filter-deals").submit();
});

$("#IsFiltered").on("click", function () {
    $("#filter-paid-negotiation").submit();
});

$("#IsCanceled").on("click", function () {
    $("#show-canceled-negotiations").submit();
});

$("#IsReed").on("click", function () {
    $("#show-reed-messages").submit();
});

$("#IsDeleted").on("click", function () {
    $("#show-deleted-blogs").submit();
});

$("#IsDeleted").on("click", function () {
    $("#show-deleted-naces").submit();
});

var openSideMenu = function () {
    $('#side-menu').removeClass("invisible");
    $('#side-menu').removeClass('-translate-x-[10rem]');
    $('#side-menu').removeClass('left-0');
    $('#side-menu').css('opacity', '1');
    $('#side-menu').css('z-index', '10');
};
var closeSideMenu = function () {
    $('#side-menu').addClass("invisible");
    $('#side-menu').addClass('-translate-x-[10rem]');
    $('#side-menu').addClass('left-0');
    $('#side-menu').css('opacity', '0');
    $('#side-menu').css('z-index', '-1');
};

var openNotificationPanel = function () {
    $('#notification-panel').css('visibility', 'visible');
    $('#notification-panel').removeClass('-translate-y-[20rem]');
    $('#notification-panel').css('opacity', '1');
    $('#notification-panel').css('z-index', '10');
};

var closeNotificationPanel = function () {
    $('#notification-panel').css('visibility', 'hidden');
    $('#notification-panel').addClass('-translate-y-[20rem]');
    $('#notification-panel').css('opacity', '0');
    $('#notification-panel').css('z-index', '-1');
};

var openProfileMenu = function () {
    $('#mobile-menu').css('visibility', 'visible');
    $('#mobile-menu').removeClass('top-0');
    $('#mobile-menu').addClass("top-16");
    $('#mobile-menu').css('opacity', '1');
    $('#mobile-menu').css('z-index', '50');
    $('#notification-panel').css('z-index', '1');
}

var closeProfileMenu = function () {
    $('#mobile-menu').css('visibility', 'hidden');
    $('#mobile-menu').css('opacity', '0');
    $('#mobile-menu').css('z-index', '-1');
    $('#mobile-menu').addClass('top-0');
    $('#mobile-menu').removeClass("top-16");
}

$("#dashboard-menu").on("click", function () {
    if (window.screen.width < 1024) {
        var stat = $("#side-menu").css('visibility');
        if (stat === 'hidden') {
            openSideMenu();
            closeNotificationPanel();
            closeProfileMenu();
        } else {
            closeSideMenu();
        }
    }
});


$('#notification-handler-mobile').on('click',
    function () {
        var status = $('#notification-panel').css("visibility");
        if (status === 'hidden') {
            openNotificationPanel();
            closeSideMenu();
            closeProfileMenu();
        } else {
            closeNotificationPanel();
        }
    });

$('#mobile-menu-trigger').on('click',
    function () {
        var status = $('#mobile-menu').css("visibility");
        if (status === 'hidden') {
            openProfileMenu();
            closeNotificationPanel();
            closeSideMenu();
        } else {
            closeProfileMenu();
        }
    });

$("#mobile-home-menu-close").on('click', function () {
    closeLandingMobileMenu();
});

var closeFilterMenu = function () {
    $('#available-filters').addClass("invisible");
    $('#available-filters').addClass('translate-x-[30rem]');
    $('#available-filters').css('opacity', '0');
};

$('html').click(function (e) {
    if (e.target.id === "notification-markread" |
        e.target.id === "notification-markread-svg" |
        e.target.id === "notification-markread-path")
        return;

    if ($('#grid-size-container-unlogged').css('visibility') !== 'hidden') {
        $("#grid-size-container-unlogged").addClass('invisible');
        $("#grid-size-container-unlogged").addClass('scale-95');
        $('#grid-size-container-unlogged').css('opacity', '0');
    }


    if (window.screen.width < 1024) {
        var element = $(e.target)[0]
        if (element.id === "mobile-home-menu-open") {
            openLandingMobileMenu();
        }

        if (element.id !== "mobile-menu-trigger" &&
            element.id !== "notification-handler-mobile" &&
            element.id !== "dashboard-menu" &&
            element.id !== "mobile-home-menu-open"
        ) {
            closeProfileMenu();
            closeNotificationPanel();
            closeSideMenu();
            closeLandingMobileMenu();
        }


    } else {
        var element = $(e.target)[0]
        if (element.id !== "notification-handler") {
            var status = $('#notification-panel').css('visibility');
            if (status !== 'hidden') {
                closeNotificationPanel();
            }
        }
    }
});



$('#filter-menu-handler').on('click',
    function () {
        var status = $('#available-filters').css('visibility');
        if (status === 'hidden') {
            $('#available-filters').removeClass('invisible');
            $('#available-filters').removeClass("invisible");
            $('#available-filters').removeClass('translate-x-[30rem]');
            $('#available-filters').css('opacity', '1');
        } else {
            closeFilterMenu();
        }
    });
