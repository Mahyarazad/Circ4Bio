
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


$(document).ready(() => {
    var NotificationSettings = {
        "url": "https://localhost:5001/api/Notification/Notification",
        "method": "GET",
        "dataType": "json",
        "headers": {
            "Content-Type": "application/json"
        },
    };
    var NotificationCountSettings = {
        "url": "https://localhost:5001/api/Notification/CountUnreadNotification",
        "method": "GET",
        "dataType": "json",
        "headers": {
            "Content-Type": "application/json"
        },
    };

    $.ajax(NotificationSettings).done(function (response) {
        const wrapper = $('#notification-wrapper');
        response.forEach(item => {
            NotificationDom =
                `<div data-notification-label="${item.id}" class="mt-10 max-w-sm w-full bg-white shadow-lg rounded-lg pointer-events-auto ring-1 ring-black ring-opacity-5 overflow-hidden">
                    <div class="p-2">
                        <div class="flex items-start">
                            <div class="flex-shrink-0">
                                <!-- Heroicon name: outline/inbox -->
                                <svg class="h-6 w-6 text-gray-400" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor" aria-hidden="true">
                                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20 13V6a2 2 0 00-2-2H6a2 2 0 00-2 2v7m16 0v5a2 2 0 01-2 2H6a2 2 0 01-2-2v-5m16 0h-2.586a1 1 0 00-.707.293l-2.414 2.414a1 1 0 01-.707.293h-3.172a1 1 0 01-.707-.293l-2.414-2.414A1 1 0 006.586 13H4" />
                                </svg>
                            </div>
                            <div class="ml-3 w-0 flex-1 pt-0.5">
                                <p class="text-sm font-medium text-gray-900">
                                   ${item.notificationTitle}
                                </p>
                                <p class="mt-1 text-sm text-gray-500">
                                    ${item.notificationBody}
                                </p>
                            </div>
                            <div class="ml-4 flex-shrink-0 flex" >
                                <button type="button" id="notification-markread" data-notification-id="${item.id}" onclick="handleNotificationRead(${item.id})"
                                        class="bg-white rounded-md inline-flex text-gray-400 hover:text-gray-500 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500">
                                    <span class="sr-only">Close</span>
                                    <!-- Heroicon name: solid/x -->
                                    <svg class="h-5 w-5" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor" aria-hidden="true">
                                        <path fill-rule="evenodd" d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z" clip-rule="evenodd" />
                                    </svg>
                                </button>
                            </div>
                        </div>
                    </div>
                </div>`;
            wrapper.append(NotificationDom);
        });

    });
    $.ajax(NotificationCountSettings).done(function (response) {
        $("#notification-counter").text(response)
    });


});

function handleNotificationRead(id) {
    var NotificationCountSettings = {
        "url": "https://localhost:5001/api/Notification/CountUnreadNotification",
        "method": "GET",
        "dataType": "json",
        "headers": {
            "Content-Type": "application/json"
        },
    };
    var NotificationMarkReadSettings = {
        "url": "https://localhost:5001/api/Notification/",
        "method": "POST",
        "dataType": "json",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json"
        },
        "data": JSON.stringify({
            "Id": id
        }),
    };
    $.ajax(NotificationMarkReadSettings).done(function (response) {
        $("#notification-counter").text(response)
        $(`div[data-notification-label='${id}']`).remove();
    });

}




