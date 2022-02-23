"use strict";
//var host = "https://localhost:5001";
//var host = "http://www.maahyarazad.ir"
//
//function resetInput() {
//    $("#messaging-form").trigger("reset");
//    $('#filename-input').val(null);
//    $('#filename-input').addClass("hidden");
//    $('#message-input').val("");
//    $('#message-input').removeClass("text-white");
//    $("#file-input-error").text("");
//
//};
//
//$('#file-input').on('change', function () {
//    var file = this.files[0];
//    var fileName;
//
//    $('#filename-input').removeClass("hidden");
//    if (file.name.length > 25) {
//        fileName = `${file.name.slice(0, 25)}...`
//    } else {
//        fileName = file.name;
//    }
//
//    $('#filename-input').text(fileName);
//    $('#message-input').val(fileName);
//    $('#message-input').addClass("text-white");
//    $('#filename-input').append(
//        `<svg class="h-5 w-5 hover:cursor-pointer" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor" aria-hidden="true" onclick="resetInput()">
//                                    <path fill-rule="evenodd" d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z" clip-rule="evenodd" />
//                                </svg>`);
//});

//$("#message-send-button").on('click',
//    function () {
//        var validationCheck = $("#file-input-error").text();
//        if ($("#message-input").val() !== "" & validationCheck === "") {
//            $('#arrow-button').remove();
//            loadingDom = `<svg height="40" width="40" class="loader">
//                              <circle class="dot" cx="10" cy="20" r="3"  />
//                              <circle class="dot" cx="20" cy="20" r="3"  />
//                              <circle class="dot" cx="30" cy="20" r="3"  />
//                            </svg>`;
//            $("#message-send-button").append(loadingDom);
//        }
//    });

//var connection = new signalR.HubConnectionBuilder()
//    .withUrl("/chatHub")
//    .withAutomaticReconnect()
//    .configureLogging(signalR.LogLevel.None)
//    .build();
//
//connection.start().then(function () {
//    const loggedUser = parseInt($("#messaging-container").attr("data-logged-user-id"));
//    const negotiateId = $('#Command_NegotiateId').val();
//    var MessageDom;
//    connection.invoke("GetAllMessages", parseInt(negotiateId)).then(function (response) {
//        if (response) {
//            const wrapper = $("#messaging-container");
//            response.forEach(item => {
//                MessageDom = `<div class="col-start-1 col-end-13 px-2 rounded-lg">
//                                <div class="flex items-center justify-start ${item.userId === loggedUser
//                        ? "flex-row-reverse"
//                        : ""}">
//                                    <div class="relative flex items-center h-10 w-10 rounded-full flex-shrink-0">
//                                        <img class="h-10 w-10 rounded-full"
//                                             src="${host}/Site Files/Profile_Images/${item.userId === item.buyerId
//                        ? item.buyyerImageString === "default-avatar.png"
//                            ? "default-avatar.png"
//                            : item.buyyerImageString
//                        : item.sellerImageString === "default-avatar.png"
//                            ? "default-avatar.png"
//                            : item.sellerImageString}"
//                                             alt=""/>
//                                        <div class="absolute text-gray-600 inset-y-7 inset-x-8 text-xs">
//                                            ${item.userId === item.buyerId
//                        ? item.buyyerLetter.toUpperCase()
//                        : item.sellerLetter.toUpperCase()}
//                                        </div>
//                                    </div>
//                                    <div class="relative mr-3 ml-3 text-sm ${item.userId === loggedUser
//                        ? "bg-sky-100"
//                        : ""} py-2 px-4 shadow rounded-xl">
//                                        ${item.fileString === null
//                        ? `<div>${item.messageBody}</div>`
//                        : `<a href="${host}/Site%20Files/Deal%20Documents/${item.negotiateId}/${item.fileString}">${item
//                            .messageBody}</a>`}
//                                    </div>
//                                </div>
//                            </div>`;
//
//                wrapper.append(MessageDom);
//            })
//            wrapper.append(`<div id="end-of-messages"></div>`);
//            setTimeout(() => {
//                var element = document.getElementById('end-of-messages');
//                element.scrollIntoView({ block: "start" });
//                $('#message-input').val('');
//            }, 50);
//        }
//    })
//})
//
//connection.start().then(function () {
//    document.getElementById("message-send-button").disabled = false;
//}).catch(function (err) {
//    console.log(err);
//});
//
//document.getElementById("message-send-button").addEventListener("click", function (event) {
//    const messageBody = $('#message-input').val();
//    if (messageBody) {
//        const loggedUser = parseInt($("#messaging-container").attr("data-logged-user-id"));
//        const negotiateId = $('#Command_NegotiateId').val();
//        const fileInput = $("#file-input").prop('files')[0];
//        if (fileInput !== undefined) {
//            var formData = new FormData();
//            formData.append("messageBody", fileInput);
//            var fileTransfer = {
//                "url": `${host}/api/Messaging/PostFile`,
//                "method": "POST",
//                "crossDomain": "true",
//                "headers": {
//                    "Access-Control-Allow-Origin": "*",
//                    "Content-Type": "application/json"
//                },
//                "data": new FormData(fileInput)
//            };
//            $.ajax(fileTransfer).done(function (response) {
//                console.log(response)
//            });
//        }
//
//        setTimeout(() => {
//            var element = document.getElementById('end-of-messages');
//            element.scrollIntoView({ block: "start" });
//            $('#message-input').val('');
//        }, 50);
//
//        //    var file = fileInput.files[0];
//        //    var fr = new FileReader();
//        //    fr.onload = receivedText;
//        //    //fr.readAsText(file);
//        //    //fr.readAsBinaryString(file); //as bit work with base64 for example upload to server
//        //    fr.readAsDataURL(file);
//
//
//        connection.invoke("SendMessage", messageBody, negotiateId, loggedUser.toString()).catch(function (err) {
//            return console.error(err.toString());
//        });
//        event.preventDefault();
//    }
//});
//
//connection.on("ReceiveMessage", function (host, messageBody, negotiateId, loggedUser, userId,
//    buyerId, buyerImageString, sellerImageString, buyerLetter, sellerLetter) {
//    const wrapper = $("#messaging-container");
//    var MessageDom;
//    // We can assign user-supplied strings to an element's textContent because it
//    // is not interpreted as markup. If you're assigning in any other way, you
//    // should be aware of possible script injection concerns.
//    MessageDom = `<div class="col-start-1 col-end-13 px-2 rounded-lg">
//                                <div class="flex items-center justify-start ${userId !== loggedUser ? "flex-row-reverse" : ""}">
//                                    <div class="relative flex items-center h-10 w-10 rounded-full flex-shrink-0">
//                                        <img class="h-10 w-10 rounded-full"
//                                             src="${host}/Site Files/Profile_Images/${userId === buyerId ?
//            buyerImageString === "default-avatar.png" ? "default-avatar.png" : buyerImageString : sellerImageString === "default-avatar.png" ? "default-avatar.png" : sellerImageString}"
//                                             alt=""/>
//                                        <div class="absolute text-gray-600 inset-y-7 inset-x-8 text-xs">
//                                            ${userId === buyerId ? buyerLetter.toUpperCase() : sellerLetter.toUpperCase()}
//                                        </div>
//                                    </div>
//                                    <div class="relative mr-3 ml-3 text-sm ${userId !== loggedUser ? "bg-sky-100" : ""} py-2 px-4 shadow rounded-xl">
//                                        <div>${messageBody}</div>
//                                    </div>
//                                </div>
//                            </div>`;
//
//    wrapper.append(MessageDom);
//    setTimeout(() => {
//        var element = document.getElementById('end-of-messages');
//        element.scrollIntoView({ block: "start" });
//    }, 20);
//});

setTimeout(() => {
    var element = document.getElementById('end-of-messages');
    element.scrollIntoView({ block: "start" });
}, 20);