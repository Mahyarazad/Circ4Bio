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
                                             src="${host}/Site%20Files/Profile_Images/${item.userId === item.buyerId
                        ? item.buyyerImageString === "default-avatar.png"
                            ? "default-avatar.png"
                            : item.buyyerImageString
                        : item.sellerImageString === "default-avatar.png"
                            ? "default-avatar.png"
                            : item.sellerImageString}"
                                             alt=""/>
                                        <div class="absolute text-gray-600 inset-y-7 inset-x-8 text-xs font-bold">
                                            ${item.userId === item.buyerId
                        ? item.buyyerLetter.toUpperCase()
                        : item.sellerLetter.toUpperCase()}

                                        </div>
<div class="text-xs absolute top-10 w-32 tracking-tight ${item.userId === loggedUser
                        ? "right-0"
                        : ""}">${item.creationTime.slice(0, 10)} ${item.creationTime.slice(11,
                            item.creationTime.length - 8)
                    }</div>
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
            },
                50);
        }
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

function attachmentHandler(e) {
    var file = e.files[0];
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
};

var sendMessageListiner = document.getElementById("message-send-button");

addEventListener("keypress",
    (event) => {

        if (13 == event.keyCode) {
            const messageBody = $('#message-input').val();
            if (messageBody) {
                const loggedUser = parseInt($("#messaging-container").attr("data-logged-user-id"));
                const negotiateId = $('#Command_NegotiateId').val();
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
                $('#message-input').val("");
            } else {
                event.preventDefault();
                window.alert("You cannot send an empty message.");
                return;
            }
        }
    });

function loaderRenderer() {
    $("#message-send-button").empty();
    $("#message-send-button").removeClass("items-center");
    $("#message-send-button").removeClass("justify-center");
    $('#message-send-button').append(
        `<div class="loadingio-spinner-eclipse-button mr-6" id="location-loader" style="margin-left:9px;">
                        <div class="ldio-button-sky-700">
                            <div></div>
                        </div>
                    </div>`);
}

function removeLoader(e) {
    $("#message-send-button").empty();
    $("#message-send-button").addClass("items-center");
    $("#message-send-button").addClass("justify-center");
    $("#message-send-button").append(e)
}


function sendMessage() {

    if ($("#message-input").val() === '') {
        window.alert("You cannot send an empty message.");
        return;
    }

    var res = $("#messaging-form").validate();
    if (res.errorList.length === 0) {
        var sendButton = $("#message-send-button").html();

        const messageBody = $('#message-input').val();
        if (messageBody) {
            const loggedUser = parseInt($("#messaging-container").attr("data-logged-user-id"));
            const negotiateId = $('#Command_NegotiateId').val();
            const fileInput = $("#file-input").prop('files')[0];
            if (fileInput !== undefined) {
                loaderRenderer();
                var formData = new FormData();
                formData.append("file", fileInput, negotiateId.toString());

                $.ajax({
                    type: "POST",
                    beforeSend: function (request) {
                        request.setRequestHeader("FileName", `${fileInput.name}`);
                    },
                    processData: false,
                    contentType: false,
                    data: formData,
                    enctype: 'multipart/form-data',
                    crossDomain: true,
                    url: "https://www.circ4bio.com/api/Messaging/FileUpload",
                    success: function (response) {
                        $("#messaging-container").empty();
                        resetInput();
                        messaging();
                        window.alert(`Successful Upload --> The recipient has been notified through notifications.`);

                        removeLoader(sendButton);
                    },
                    error: function (response) {
                        resetInput();
                        window.alert(`Failed --> Connection has been closed, Please try again!`);

                        removeLoader(sendButton);
                    }
                });

            } else {
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
                event.preventDefault(sendButton);
            }

            setTimeout(() => {
                var element = document.getElementById('end-of-messages');
                element.scrollIntoView({ block: "start" });
                $('#message-input').val('');
            },
                50);
        } else {

            window.alert("Something went wrong, please refresh this page and try again.");
            return;
        }
    }
};

connection.on("ReceiveMessage", function (host, messageBody, negotiateId, loggedUser, userId,
    buyerId, buyerImageString, sellerImageString, buyerLetter, sellerLetter, creationTime) {
    const wrapper = $("#messaging-container");
    var MessageDom;
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you
    // should be aware of possible script injection concerns.
    MessageDom = `<div class="col-start-1 col-end-13 px-2 rounded-lg">
                                <div class="flex items-center justify-start ${userId !== loggedUser ? "flex-row-reverse" : ""}">
                                    <div class="relative flex items-center h-10 w-10 rounded-full flex-shrink-0">
                                        <img class="h-10 w-10 rounded-full"
                                             src="${host}/Site%20Files/Profile_Images/${userId === buyerId ?
            buyerImageString === "default-avatar.png" ? "default-avatar.png" : buyerImageString : sellerImageString === "default-avatar.png" ? "default-avatar.png" : sellerImageString}"
                                             alt=""/>
                                        <div class="absolute text-gray-600 inset-y-7 inset-x-8 text-xs">
                                            ${userId === buyerId ? buyerLetter.toUpperCase() : sellerLetter.toUpperCase()}
                                        </div>
                                        <div class="text-xs absolute top-10 w-32 tracking-tight ${userId !== loggedUser
            ? "right-0"
            : ""}">${new Date().toISOString().split('T')[0]} ${new Date().toLocaleTimeString('en-US', { hour12: false })}</div>
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

