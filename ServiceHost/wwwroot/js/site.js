// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

function getHtml(val) {
    $.get(val,
        function (htmlPage) {
            $("#ModalConetnt").html(htmlPage);
            // We need to parse the ajax form

            const form = $("form");
            const newform = form[form.length - 1];
            $.validator.unobtrusive.parse(newform);
        });
}

function getModelFromPage(val) {
    var dateList, startDate, endDate;
    $.get(val,
        function (htmlPage) {
            var page = htmlPage;
            dateList = page.match(/[*0-9]{2}\W[*0-9]{2}\W[*0-9]{4}/g);
            startDate = new Date(dateList[1]);
            endDate = new Date(dateList[2]);
        });
}

function showModal() {
    $('#modal').css('visibility', 'visible');
    $('#modal').css('opacity', '1');
    $('#modal').css('z-index', '10');
    $('#modal').removeClass('translate-y-full');
    $('#modal').addClass('top-0');
    window.location.hash = "##";
}

function hideModal() {
    $("#modal").css("visibility", "hidden");
    $('#modal').css('opacity', '0');
    $('#modal').css('z-index', '-1');
    $('#modal').addClass('translate-y-full');
    $('#modal').removeClass('top-0');
    window.location.hash = "##";
    //    $("#modal").on("hidden.bs.modal",
    //        function () {
    //            window.location.hash = "##";
    //        });
}

//We listen to any hashChange to open the modal
$(window).on('hashchange',
    function () {
        var url = window.location.hash;
        if (url === "##") {
            return

        } else {
            url = url.split("showmodal=")[1];
            getHtml(url);
            showModal();
        }
    });

function handleAjaxGet(formData, url, action, data) {

    $.get(url,
        data,
        function (data) {
            CallBackHandler(data, action, formData);
        });
}

function handleAjaxPost(formData, url, action) {
    $.ajax({
        url: url,
        type: "post",
        data: formData,
        enctype: "multipart/form-data",
        dataType: "json",
        processData: false,
        contentType: false,
        success: function (data) {
            // Notify the user about the proccess detail
            if (!data.result.isSucceeded) {
                $("#modal").hide();
                var resultDom = $('#operation-result-failed');
                var resultDomMessage = $('#operation-result-failed-message');

                resultDomMessage.text(data.result.message);
                resultDom.css('display', 'block');
            } else {

                $("#modal").hide();
                var url = window.location.href;
                var splited = url.split('/');

                if (splited[splited.length - 2] === "notification") {
                    var updatedUrl = splited.slice(0, splited.length);
                    url = updatedUrl.join('/', updatedUrl);
                    var resultDom = $('#operation-result');
                    var resultDomMessage = $('#operation-result-message');
                    resultDomMessage.text(data.result.message);
                    resultDom.css('display', 'block');
                    setTimeout(() => { window.location.replace(url) }, 3000)
                }

                else if (splited[splited.length - 2] === "quatation") {
                    if (splited[splited.length - 1].includes('##')) {
                        var resultDom = $('#operation-result');
                        var resultDomMessage = $('#operation-result-message');
                        resultDomMessage.text(data.result.message);
                        resultDom.css('display', 'block');
                        setTimeout(() => { window.location.replace(url) }, 3000)
                    }
                    var resultDom = $('#operation-result');
                    var resultDomMessage = $('#operation-result-message');
                    resultDomMessage.text(data.result.message);
                    resultDom.css('display', 'block');
                    setTimeout(() => { window.location.replace(url) }, 3000)
                }

                else if (action === "CancelNegotiation") {
                    var updatedUrl = splited.slice(0, splited.length - 2);
                    url = updatedUrl.join('/', updatedUrl);
                    var resultDom = $('#operation-result');
                    var resultDomMessage = $('#operation-result-message');
                    resultDomMessage.text(data.result.message);
                    resultDom.css('display', 'block');
                    setTimeout(() => { window.location.replace(url) }, 3000)
                }

                else if (splited[splited.length - 2] !== "messages") {

                    if (splited[splited.length - 2] === "profile" | splited[splited.length - 2] === "create") {
                        var resultDom = $('#operation-result');
                        var resultDomMessage = $('#operation-result-message');
                        resultDomMessage.text(data.result.message);
                        resultDom.css('display', 'block');
                        setTimeout(() => { window.location.replace(url) }, 3000)
                    } else {
                        if (parseInt(splited[splited.length - 1]) > 0) {
                            var updatedUrl = splited.slice(0, splited.length - 2);
                            url = updatedUrl.join('/', updatedUrl);
                        } else if (splited[splited.length - 1] === "create") {
                            var updatedUrl = splited.slice(0, splited.length - 1);
                            url = updatedUrl.join('/', updatedUrl);
                        }
                        var resultDom = $('#operation-result');
                        var resultDomMessage = $('#operation-result-message');

                        resultDomMessage.text(data.result.message);
                        resultDom.css('display', 'block');

                        setTimeout(() => {
                            window.location.replace(url);
                        },
                            1000)
                    }
                }
            }
            setInterval(function () {
                CallBackHandler(data, action, formData);;
            }, 1500);
        },
        error: function (data) {
            // Notify the user about the proccess detail
            window.alert(data.statusText);
        }
    });
}


$(document).on("submit",
    //We listen to submit on any form that has data-ajax attribute to handel it
    'form[data-ajax="true"]',
    function (e) {
        e.preventDefault();
        var form = $(this);
        const method = form.attr("method").toLocaleLowerCase();
        const url = form.attr("action");
        var action = form.attr("data-action");

        if (method === "get") {
            const data = form.serializeArray();
            handleAjaxGet(formData, url, action, data)
        } else {
            var formData = new FormData(this);
            handleAjaxPost(formData, url, action);
        }
        return false;
    });

$(document).on("submit",
    //We listen to submit on any form that does not have data-ajax attribute to handel it
    'form[data-ajax="false"]',
    function (e) {
        e.preventDefault();
        var form = $(this);
        const method = form.attr("method").toLocaleLowerCase();
        const url = form.attr("action");
        var action = form.attr("data-action");

        if (method === "get") {
            const data = form.serializeArray();
            handleAjaxGet(formData, url, action, data)
        } else {
            var formData = new FormData(this);
            handleAjaxPost(formData, url, action);
        }
        $("button[id='notification-markread']").on('click',

            function (e) {
                var Id = $(this).attr('data-notification-id');
                var stringBuilder = "div[data-notification-label='" + Id + "']"
                $(stringBuilder).remove();

            });
        return false;
    });

$("div").click(function () {
    $('[data-toggle="datepicker"]').hide();
});

function CallBackHandler(data, action, form) {
    // The message comes directly from Operation Result class
    // in our Helper 0_Framework class library
    switch (action) {
        case "Message":

            alert(data.result.Message);
            break;
            window.location.reload();
            setInterval(function () {
                var resultDom = $('#operation-result');
                resultDom.css('display', 'none');
            },
                1500)
        case "Refresh":
            $("#modal").hide();
            if (data.result.isSucceeded) {
                window.location.reload();
                setInterval(function () {
                    var resultDom = $('#operation-result');
                    resultDom.css('display', 'none');
                },
                    1500)
            } else {
                var currentLocation = window.location.href;
                window.location.href = currentLocation;
                setInterval(function () {
                    var resultDom = $('#operation-result-failed');
                    resultDom.css('display', 'none');
                },
                    2000)
            }
        case "RefreshList":
            {
                var currentLocation = window.location.href;
                window.location.href = currentLocation;
                //                window.location.href += `?${data.Id}`
                //                window.location.replace(currentLocation + `?id=${data}`)
                //                hideModal();
                //                const refereshUrl = form.attr("data-refereshurl");
                //                const refereshDiv = form.attr("data-refereshdiv");
                //                get(refereshUrl, refereshDiv);
            }
            break;
        case "setValue":
            {
                const element = form.data("element");
                $(`#${element}`).html(data);
            }
            break;
        case "CancelNegotiation":
            {
                var currentLocation = window.location.href;
                window.location.href = currentLocation;
            }
            break;
        default:
    }
}

function get(url, refereshDiv) {
    const searchModel = window.location.search;
    $.get(url,
        searchModel,
        function (result) {
            $("#" + refereshDiv).html(result);
        });
}

function checkCart(availableStock) {
    const value = parseInt($('#sale-value').val());
    if (value > availableStock) {
        $('.cart-btn').attr('disabled', 'disabled');
        $('.cart-btn').css('text-decoration', 'line-through');
        $('.cart-btn').css('cursor', 'not-allowed');
    } else {
        $('.cart-btn').prop("disabled", false);
        $('.cart-btn').css('text-decoration', '');
        $('.cart-btn').css('cursor', 'pointer');
    }
}

function makeSlug(source, dist) {
    const value = $('#' + source).val();
    $('#' + dist).val(convertToSlug(value));
}

var convertToSlug = function (str) {
    var $slug = '';
    const trimmed = $.trim(str);
    $slug = trimmed.replace(/[^a-z0-9-]/gi, '-').replace(/-+/g, '-').replace(/^-|-$/g, '');
    return $slug.toLowerCase();
};

function checkSlugDuplication(url, dist) {
    const slug = $('#' + dist).val();
    const id = convertToSlug(slug);
    $.get({
        url: url + '/' + id,
        success: function (data) {
            if (data) {
                sendNotification('error', 'top right', "Something went wrong", "Slug cannot be duplicated!");
            }
        }
    });
}

function fillField(source, dist) {
    const value = $('#' + source).val();
    $('#' + dist).val(value);
}

$(document).on("click",
    'button[data-ajax="true"]',
    function () {
        const button = $(this);
        const form = button.data("request-form");
        const data = $(`#${form}`).serialize();
        let url = button.data("request-url");
        const method = button.data("request-method");
        const field = button.data("request-field-id");
        if (field !== undefined) {
            const fieldValue = $(`#${field}`).val();
            url = url + "/" + fieldValue;
        }
        if (button.data("request-confirm") == true) {
            if (confirm("Are you sure?")) {
                handleAjaxCall(method, url, data);
            }
        } else {
            handleAjaxCall(method, url, data);
        }
    });

function handleAjaxCall(method, url, data) {
    if (method === "post") {
        $.post(url,
            data,
            "application/json; charset=utf-8",
            "json",
            function (data) {

            }).fail(function (error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: 'Something went wrong!',
                    footer: '<a href="">Why do I have this issue?</a>'
                })
            });
    }
}

//jQuery.validator.addMethod("maxFileSize",
//    function (value, element, params) {
//        var size = element.files[0].size;
//        var maxSize = 3 * 1024 * 1024;
//        if (size > maxSize)
//            return false;
//        else {
//            return true;
//        }
//    });
//jQuery.validator.unobtrusive.adapters.addBool("maxFileSize");

jQuery.validator.addMethod("maxFileSize",
    function (value, element, params) {
        var validFormat = [".jpeg", ".jpg", ".png"];
        if (element.files.length > 0) {
            var size = element.files[0].size;

            var maxSize = 1 * 1024 * 1024;
            if (size > maxSize)
                return false;
            else
                return true;
        }
        return true;
    });
jQuery.validator.unobtrusive.adapters.addBool("maxFileSize");

jQuery.validator.addMethod("fileExtensionLimit",
    function (value, element, params) {
        var validFormat = ["image/jpeg", "image/jpg", "image/png"];
        var fileExtension = element.files[0].type;
        return validFormat.indexOf(fileExtension) > -1;
    });
jQuery.validator.unobtrusive.adapters.addBool("fileExtensionLimit");


