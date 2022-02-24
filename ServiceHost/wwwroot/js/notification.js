"use strict";
var host = "https://localhost:5001";
//var host = "http://www.maahyarazad.ir";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Debug)
    .build();

var invokeNotification = function () {
    var NotificationDom = document.createElement('template');;
    var replacer = new DocumentFragment();
    var wrapper = $('#notification-wrapper');
    connection.invoke("Notification").then(function (response) {
        if (response.length !== 0) {
            response.forEach(item => {
                NotificationDom.innerHTML =
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
                replacer.append(NotificationDom.content);
            });
            wrapper.html(replacer);
        } else {
            wrapper.html(
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
    })
};
var invokeCountUnreadNotifications = function () {
    connection.invoke("CountUnreadNotification").then(response => {
        $("#notification-counter").text(response);
        $("#notification-counter-mobile").text(response);
    });
}
var messaging = function () {
    document.getElementById("message-send-button").disabled = false;
    const loggedUser = parseInt($("#messaging-container").attr("data-logged-user-id"));
    const negotiateId = $('#Command_NegotiateId').val();
    var MessageDom;
    connection.invoke("GetAllMessages", parseInt(negotiateId)).then(function (response) {
        if (response) {
            const wrapper = $("#messaging-container");
            response.forEach(item => {
                MessageDom = `<div class="col-start-1 col-end-13 px-2 rounded-lg">
                                <div class="flex items-center justify-start ${item.userId === loggedUser
                        ? "flex-row-reverse"
                        : ""}">
                                    <div class="relative flex items-center h-10 w-10 rounded-full flex-shrink-0">
                                        <img class="h-10 w-10 rounded-full"
                                             src="${host}/Site Files/Profile_Images/${item.userId === item.buyerId
                        ? item.buyyerImageString === "default-avatar.png"
                            ? "default-avatar.png"
                            : item.buyyerImageString
                        : item.sellerImageString === "default-avatar.png"
                            ? "default-avatar.png"
                            : item.sellerImageString}"
                                             alt=""/>
                                        <div class="absolute text-gray-600 inset-y-7 inset-x-8 text-xs">
                                            ${item.userId === item.buyerId
                        ? item.buyyerLetter.toUpperCase()
                        : item.sellerLetter.toUpperCase()}
                                        </div>
                                    </div>
                                    <div class="relative mr-3 ml-3 text-sm ${item.userId === loggedUser
                        ? "bg-sky-100"
                        : ""} py-2 px-4 shadow rounded-xl">
                                        ${item.fileString === null
                        ? `<div>${item.messageBody}</div>`
                        : `<a href="${host}/Site%20Files/Deal%20Documents/${item.negotiateId}/${item.fileString}">${item
                            .messageBody}</a>`}
                                    </div>
                                </div>
                            </div>`;

                wrapper.append(MessageDom);
            });
            wrapper.append(`<div id="end-of-messages"></div>`);
            setTimeout(() => {
                var element = document.getElementById('end-of-messages');
                element.scrollIntoView({ block: "start" });
                $('#message-input').val('');
            }, 50);
        }
    })
}
var messagingLoader = window.location.pathname.split('/');

connection.start().then(() => {
    invokeNotification();
    invokeCountUnreadNotifications();
    setInterval(() => {
        try {
            invokeNotification();
        } catch (e) {
            console.log(e);
        }
        try {
            invokeCountUnreadNotifications();
        } catch (e) {
            console.log(e);
        }
    }, 5000);

    if (messagingLoader[messagingLoader.length - 2] === 'messages') {
        try {
            messaging();
        } catch (e) {
            console.log(e);
        }
    }
}).catch(e => console.log(e));

function handleNotificationRead(notificationId) {
    connection.invoke("MarkRead", notificationId).then(() => {
        $(`div[data-notification-label='${notificationId}']`).remove();
        invokeNotification();
        invokeCountUnreadNotifications();
    }).catch(function (err) {
        return console.error(err.toString());
    });
}

function handleNotificationAllRead() {
    connection.invoke("MarkReadAll").then(() => {
        invokeNotification();
        invokeCountUnreadNotifications();
    }).catch(function (err) {
        return console.error(err.toString());
    });
}

function resetInput() {
    $("#messaging-form").trigger("reset");
    $('#filename-input').val(null);
    $('#filename-input').addClass("hidden");
    $('#message-input').val("");
    $('#message-input').removeClass("text-white");
    $("#file-input-error").text("");

};

$('#file-input').on('change', function () {
    var file = this.files[0];
    var fileName;

    $('#filename-input').removeClass("hidden");
    if (file.name.length > 25) {
        fileName = `${file.name.slice(0, 25)}...`
    } else {
        fileName = file.name;
    }

    $('#filename-input').text(fileName);
    $('#message-input').val(fileName);
    $('#message-input').addClass("text-white");
    $('#filename-input').append(
        `<svg class="h-5 w-5 hover:cursor-pointer" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor" aria-hidden="true" onclick="resetInput()">
                                    <path fill-rule="evenodd" d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z" clip-rule="evenodd" />
                                </svg>`);
});

var sendMessageListiner = document.getElementById("message-send-button");
if (sendMessageListiner) {
    sendMessageListiner.addEventListener("click", function (event) {
        const messageBody = $('#message-input').val();
        if (messageBody) {
            const loggedUser = parseInt($("#messaging-container").attr("data-logged-user-id"));
            const negotiateId = $('#Command_NegotiateId').val();
            const fileInput = $("#file-input").prop('files')[0];
            if (fileInput !== undefined) {
                var formData = new FormData();
                formData.append("messageBody", fileInput);
                var fileTransfer = {
                    "url": `${host}/api/Messaging/PostFile`,
                    "method": "POST",
                    "crossDomain": "true",
                    "headers": {
                        "Access-Control-Allow-Origin": "*",
                        "Content-Type": "application/json"
                    },
                    "data": new FormData(fileInput)
                };
                $.ajax(fileTransfer).done(function (response) {
                    console.log(response)
                });
            }

            setTimeout(() => {
                var element = document.getElementById('end-of-messages');
                element.scrollIntoView({ block: "start" });
                $('#message-input').val('');
            }, 50);

            //    var file = fileInput.files[0];
            //    var fr = new FileReader();
            //    fr.onload = receivedText;
            //    //fr.readAsText(file);
            //    //fr.readAsBinaryString(file); //as bit work with base64 for example upload to server
            //    fr.readAsDataURL(file);


            connection.invoke("SendMessage", messageBody, negotiateId, loggedUser.toString()).catch(function (err) {
                return console.error(err.toString());
            });
            try {
                invokeNotification();
            } catch (e) {
                console.log(e);
            }
            try {
                invokeCountUnreadNotifications();
            } catch (e) {
                console.log(e);
            }
            event.preventDefault();
        }
    });
}

connection.on("ReceiveMessage", function (host, messageBody, negotiateId, loggedUser, userId,
    buyerId, buyerImageString, sellerImageString, buyerLetter, sellerLetter) {
    const wrapper = $("#messaging-container");
    var MessageDom;
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you
    // should be aware of possible script injection concerns.
    MessageDom = `<div class="col-start-1 col-end-13 px-2 rounded-lg">
                                <div class="flex items-center justify-start ${userId !== loggedUser ? "flex-row-reverse" : ""}">
                                    <div class="relative flex items-center h-10 w-10 rounded-full flex-shrink-0">
                                        <img class="h-10 w-10 rounded-full"
                                             src="${host}/Site Files/Profile_Images/${userId === buyerId ?
            buyerImageString === "default-avatar.png" ? "default-avatar.png" : buyerImageString : sellerImageString === "default-avatar.png" ? "default-avatar.png" : sellerImageString}"
                                             alt=""/>
                                        <div class="absolute text-gray-600 inset-y-7 inset-x-8 text-xs">
                                            ${userId === buyerId ? buyerLetter.toUpperCase() : sellerLetter.toUpperCase()}
                                        </div>
                                    </div>
                                    <div class="relative mr-3 ml-3 text-sm ${userId !== loggedUser ? "bg-sky-100" : ""} py-2 px-4 shadow rounded-xl">
                                        <div>${messageBody}</div>
                                    </div>
                                </div>
                            </div>`;

    wrapper.append(MessageDom);
    invokeNotification();
    invokeCountUnreadNotifications();
    setTimeout(() => {
        var element = document.getElementById('end-of-messages');
        element.scrollIntoView({ block: "start" });
    }, 20);
});

function handleRemoveDeliveryAddress(id) {
    var removeDeliveryLocation = {
        "url": `${host}/api/UserDeliveryAddress/RemoveDeliveryLocation`,
        "method": "POST",
        "dataType": "json",
        "crossDomain": "true",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json",
            "Access-Control-Allow-Origin": "*"
        },
        "data": JSON.stringify({
            "LocationId": id
        }),
    };
    $.ajax(removeDeliveryLocation).done(function (response) {
        if (response) {
            $(`td[data-delivery-address='${id}']`).remove();
        }
    });
}