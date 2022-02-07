﻿var host = "https://localhost:5001";
//var host = "http://www.maahyarazad.ir"

var NotificationSettings = {
    "url": `${host}/api/Notification/Notification`,
    "method": "GET",
    "dataType": "json",
    "crossDomain": "true",
    "headers": {
        "Content-Type": "application/json",
        "Access-Control-Allow-Origin": "*"
    },
};
var NotificationCountSettings = {
    "url": `${host}/api/Notification/CountUnreadNotification`,
    "method": "GET",
    "dataType": "json",
    "crossDomain": "true",
    "headers": {
        "Access-Control-Allow-Origin": "*",
        "Content-Type": "application/json"
    },
};

var NotificationCountSettings = {
    "url": `${host}/api/Notification/CountUnreadNotification`,
    "method": "GET",
    "dataType": "json",
    "crossDomain": "true",
    "headers": {
        "Content-Type": "application/json",
        "Access-Control-Allow-Origin": "*"
    },
};

var NotificationAjax = () => {
    $.ajax(NotificationSettings).done(function (response) {
        const wrapper = $('#notification-wrapper');
        wrapper.empty();
        if (response.length !== 0) {
            response.forEach(item => {
                NotificationDom =
                    `<div data-notification-label="${item.recipientId}"
                    class="max-w-sm w-full bg-white shadow-lg rounded-lg pointer-events-auto ring-1 ring-black ring-opacity-5 overflow-hidden">
                    <div class="p-2">
                        <div class="flex items-start">
                            <div class="flex-shrink-0">
                                <!-- Heroicon name: outline/inbox -->
                                <svg class="h-6 w-6 text-gray-400 mt-1" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor" aria-hidden="true">
                                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20 13V6a2 2 0 00-2-2H6a2 2 0 00-2 2v7m16 0v5a2 2 0 01-2 2H6a2 2 0 01-2-2v-5m16 0h-2.586a1 1 0 00-.707.293l-2.414 2.414a1 1 0 01-.707.293h-3.172a1 1 0 01-.707-.293l-2.414-2.414A1 1 0 006.586 13H4" />
                                </svg>
                            </div>
                            <div class="ml-3 w-0 flex-1 pt-0.5">
                                <p class="text-sm font-medium text-gray-900">
                                   ${item.notificationTitle}
                                </p>
                                <p class="mt-1 font-medium text-sm text-gray-600">
                                    ${item.notificationBody}
                                </p>
                            </div>
                            <div class="ml-4 flex-shrink-0 flex">
                                <button type="button" id="notification-markread" data-notification-id="${item.recipientId}" onclick="handleNotificationRead(${item.recipientId})"
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
        } else {
            wrapper.append(
                `<div class="max-w-sm w-full bg-white shadow-lg rounded-lg pointer-events-auto ring-1 ring-black ring-opacity-5 overflow-hidden">
                    <div class="p-4">
                        <div class="flex items-start">
                            <div class="flex-shrink-0">
                                <!-- Heroicon name: outline/inbox -->
                                <svg class="h-6 w-6 text-gray-400" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor" aria-hidden="true">
                                    <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M20 13V6a2 2 0 00-2-2H6a2 2 0 00-2 2v7m16 0v5a2 2 0 01-2 2H6a2 2 0 01-2-2v-5m16 0h-2.586a1 1 0 00-.707.293l-2.414 2.414a1 1 0 01-.707.293h-3.172a1 1 0 01-.707-.293l-2.414-2.414A1 1 0 006.586 13H4"/>
                                </svg>
                            </div>
                            <div class="ml-3 w-0 flex-1 pt-0.5">
                                <p class="text-sm font-medium text-gray-900">
                                    System Message
                                </p>
                                <p class="mt-1 text-sm text-gray-500">
                                    There is no new notification
                                </p>
                            </div>
                            <div class="ml-4 flex-shrink-0 flex">
                            </div>
                        </div>
                    </div>
                </div>`);
        }
    });
}


$(document).ready(() => {
    NotificationAjax();
    $.ajax(NotificationCountSettings).done(function (response) {
        $("#notification-counter").text(response)
        $("#notification-counter-mobile").text(response)
    });
});

setInterval(() => {
    NotificationAjax();
    $.ajax(NotificationCountSettings).done(function (response) {
        $("#notification-counter").text(response)
        $("#notification-counter-mobile").text(response)
    });
},
    10000);


function handleNotificationRead(id) {
    var NotificationMarkReadSettings = {
        "url": `${host}/api/Notification/`,
        "method": "POST",
        "dataType": "json",
        "crossDomain": "true",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json",
            "Access-Control-Allow-Origin": "*"
        },
        "data": JSON.stringify({
            "Id": id
        }),
    };
    $.ajax(NotificationMarkReadSettings).done(function (response) {
        $("#notification-counter").text(response)
        $("#notification-counter-mobile").text(response)
        $(`div[data-notification-label='${id}']`).remove();
    });
    NotificationAjax();
}
