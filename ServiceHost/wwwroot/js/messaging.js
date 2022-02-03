$(document).ready(() => {
    var element = document.getElementById('end-of-messages');
    //            element.scrollIntoView({ behavior: "smooth" });
    element.scrollIntoView({ block: "end" });

});

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