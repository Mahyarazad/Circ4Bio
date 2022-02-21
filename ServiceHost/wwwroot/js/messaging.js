var host = "https://localhost:5001";
//var host = "http://www.maahyarazad.ir"

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
    if (file) {
        $('#filename-input').removeClass("hidden");
        $('#filename-input').text(file.name);
        $('#message-input').val(file.name);
        $('#message-input').addClass("text-white");
        $('#filename-input').append(
            `<svg class="h-5 w-5 hover:cursor-pointer" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor" aria-hidden="true" onclick="resetInput()">
                                        <path fill-rule="evenodd" d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z" clip-rule="evenodd" />
                                    </svg>`);
    }
});

$("#message-send-button").on('click',
    function () {
        var validationCheck = $("#file-input-error").text();
        if ($("#message-input").val() !== "" & validationCheck === "") {
            $('#arrow-button').remove();
            loadingDom = `<svg height="40" width="40" class="loader">
                              <circle class="dot" cx="10" cy="20" r="3"  />
                              <circle class="dot" cx="20" cy="20" r="3"  />
                              <circle class="dot" cx="30" cy="20" r="3"  />
                            </svg>`;
            $("#message-send-button").append(loadingDom);
        }
    });

var MessagingAjax = () => {

    var updateCheck = $("#messaging-container").children().length - 1;
    const messagingId = $("#messaging-container").attr("data-messaging-id");
    const loggedUser = parseInt($("#messaging-container").attr("data-logged-user-id"));
    var RefreshMessages = {
        "url": `${host}/api/Messaging/GetMessages`,
        "method": "POST",
        "dataType": "json",
        "crossDomain": "true",
        "headers": {
            "Access-Control-Allow-Origin": "*",
            "Content-Type": "application/json"
        },
        "data": JSON.stringify({
            "NegotiateId": messagingId
        }),
    };

    $.ajax(RefreshMessages).done(function (response) {
        if (response) {
            var timer = () => {
                if (updateCheck !== response.length) {
                    setTimeout(() => {
                        var element = document.getElementById('end-of-messages');
                        element.scrollIntoView({ block: "end" });
                    }, 200);
                }
            }
            const wrapper = $("#messaging-container");
            wrapper.empty();
            response.forEach(item => {
                MessageDom = `<div class="col-start-1 col-end-13 px-2 rounded-lg">
                                <div class="flex items-center justify-start ${item.userId === loggedUser ? "flex-row-reverse" : ""}">
                                    <div class="relative flex items-center h-10 w-10 rounded-full flex-shrink-0">
                                        <img class="h-10 w-10 rounded-full"
                                             src="${host}/Site Files/Profile_Images/${item.userId === item.buyerId ?
                        item.buyyerImageString === "default-avatar.png" ? "default-avatar.png" : item.buyyerImageString : item.sellerImageString === "default-avatar.png" ? "default-avatar.png" : item.sellerImageString}"
                                             alt=""/>
                                        <div class="absolute text-gray-600 inset-y-7 inset-x-8 text-xs">
                                            ${item.userId === item.buyerId ? item.buyyerLetter.toUpperCase() : item.sellerLetter.toUpperCase()}
                                        </div>
                                    </div>
                                    <div class="relative mr-3 ml-3 text-sm ${item.userId === loggedUser ? "bg-sky-100" : ""} py-2 px-4 shadow rounded-xl">
                                        ${item.fileString === null ? `<div>${item.messageBody}</div>` : `<a href="${host}/Site%20Files/Deal%20Documents/${item.negotiateId}/${item.fileString}">${item.messageBody}</a>`}
                                    </div>
                                </div>
                            </div>`;

                wrapper.append(MessageDom);
            })
            wrapper.append(`<div id="end-of-messages"></div>`);
            timer();
        }
    });
};
//
$(document).ready(() => {
    MessagingAjax();
});
//
//setInterval(() => {
//    MessagingAjax();
//}, 5000);


