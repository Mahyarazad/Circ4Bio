"use strict";
var host = "https://localhost:5001";


var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();


connection.start().then(function () {
    connection.invoke("GettAllMessages", 2).then(function (a) {
        //        console.log(a)
    })
})

connection.start().then(function () {
    document.getElementById("message-send-button").disabled = false;
}).catch(function (err) {
    console.log(err);
});

document.getElementById("message-send-button").addEventListener("click", function (event) {
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
        event.preventDefault();
    }
});

connection.on("ReceiveMessage", function (host, messageBody, negotiateId, loggedUser, userId,
    buyerId, buyerImageString, sellerImageString, buyerLetter, sellerLetter) {
    const wrapper = $("#messaging-container");
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
    setTimeout(() => {
        var element = document.getElementById('end-of-messages');
        element.scrollIntoView({ block: "start" });
    }, 20);
});
