"use strict";



var timeOut = 2000;
var connection = new signalR.HubConnectionBuilder()
    .withUrl(`/notificationHub`,
        {
            headers: { "QueryString": `${window.location.href}` },
            transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.LongPolling
        })
    .withAutomaticReconnect()
    .configureLogging(signalR.LogLevel.Information)
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
                            <a class="ml-3 w-0 flex-1 pt-0.5" href="${item.redirectUrl}">
                                <div>
                                    <p class="text-sm font-medium text-gray-900">
                                       ${item.notificationTitle}
                                    </p>
                                    <p class="mt-1 font-medium text-sm text-gray-600">
                                        ${item.notificationBody}
                                    </p>
                                </div>
                            </a>
                            <div class="ml-4 flex-shrink-0 flex">
                                <button type="button" id="notification-markread" data-notification-id="${item.recipientId}" onclick="handleNotificationRead(${item.recipientId})"
                                        class="bg-white rounded-md inline-flex text-gray-400 hover:text-gray-500 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-sky-700">
                                    <span class="sr-only">Close</span>
                                    <!-- Heroicon name: solid/x -->
                                    <svg class="h-5 w-5" id="notification-markread-svg"
                                        xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="currentColor" aria-hidden="true">
                                        <path id="notification-markread-path" fill-rule="evenodd" d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z" clip-rule="evenodd" />
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
var messagingLoader = window.location.pathname.split('/');

async function start() {
    if (connection.connection.q !== "Connected") {
        try {
            await connection.start();
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

            if (messagingLoader[messagingLoader.length - 2] === 'Messages') {
                try {
                    messaging();
                } catch (e) {
                    console.log(e);
                }
            }
        } catch (err) {
            console.log(err);
            setTimeout(start, 5000);
        }
    }
};

connection.onclose(async () => {
    await start();
});

// Start the connection.
start();

function handleRemoveDeliveryAddress(id) {
    var removeDeliveryLocation = {
        "url": `${host}/api/UserDeliveryAddress/RemoveDeliveryLocation`,
        "method": "POST",
        "dataType": "json",
        "crossDomain": "true",
        "timeout": timeOut,
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

function checkAmount(amount, listingId) {
    const unitPrice = parseFloat($("#deal-unit-price").val());
    var amount = parseFloat($("#deal-amount").val());
    var deliveryCost = parseFloat($("#delivery-cost").val());
    $("#total-cost").val((amount * unitPrice) + deliveryCost);
    $("#show-currency-1").text($("#selected-currency").find(":selected").text());
    $("#show-currency-3").text($("#selected-currency").find(":selected").text());
    $("#show-currency-4").text($("#selected-currency").find(":selected").text());

    if (typeof listingId === 'undefined') {
        return;
    } else {
        if (amount === null) {
            $("#check-amount-wrapper").addClass("hidden");
            $("#save").prop('disabled', false);
        } else {
            connection.invoke("CheckAmount", listingId).then(response => {
                if (response !== null) {
                    if (amount > response) {
                        $("#check-amount-wrapper").removeClass("hidden");
                        $("#check-amount")
                            .text(`Available amount is ${response}, you cannot create quatation more than current amount.`);
                        $("#save").prop('disabled', true);
                    }
                    if (amount <= response) {
                        $("#check-amount-wrapper").addClass("hidden");
                        $("#save").prop('disabled', false);
                    }
                }
            });
        }
    }
}

function renderLocation(locationId) {
    mapboxgl.accessToken = "pk.eyJ1IjoibWFoeWFyYXphZCIsImEiOiJjazhzaG9pNjIwYzJ4M2VyczJlNnNndzF6In0.ZFGc5daAFPaXObvBKA20CA";
    var map, marker;

    connection.invoke("GetLocation", locationId).then(response => {
        if (response) {
            if (response.userId !== 0) {
                $("#address-name").text(response.name);

                $("#delivery-location-wrapper").removeClass('hidden');

                $("#address-line-one").text(response.addressLineOne);
                $("#address-line-two").text(response.addressLineTwo);
                $("#country").text(response.country);
                $("#city").text(response.city);
                $("#postal-code").text(response.postalCode);
                map = new mapboxgl.Map({
                    container: "map-wrapper-create",
                    style: 'mapbox://styles/mapbox/streets-v11',
                    center: [50, 50],
                    zoom: 13
                });

                marker = new mapboxgl.Marker({ color: 'red', rotation: 0 })
                    .setLngLat([50, 50])
                    .addTo(map);
                marker.setLngLat([response.longitude, response.latitude]);
                map.flyTo({
                    center: [response.longitude, response.latitude]
                });

                map.on('load', () => {
                    map.resize();
                });

            } else {
                $("#delivery-location-wrapper").addClass('hidden');
            }
        }
    }).catch(e => {
        console.log(e);
    });
}

function handleNotificationRead(notificationId) {
    var table = $('#main-table').DataTable();
    connection.invoke("MarkRead", notificationId).then(() => {
        $(`div[data-notification-label='${notificationId}']`).remove();

        table.row($(`td[data-id='${notificationId}']`).parents('tr'))
            .remove()
            .draw();

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

function handleGetNaceDetail(id) {
    if (id === "0") {
        $('#nace-table-wrapper').addClass('hidden');
        $('#nace-info-wrapper').empty();
        return;
    }
    var getSingleNace = {
        "url": `${host}/api/Nace/GetSingleNace`,
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
    $.ajax(getSingleNace).done(function (response) {
        if (response) {
            console.log(response);
            if (response.length !== 0) {
                var selectListCounter = 1;
                $('#nace-info-wrapper').empty();
                $('#nace-table-wrapper').removeClass('hidden');
                response.items.forEach(item => {

                    $('#nace-info-wrapper').append(
                        `<tr class="bg-white border border-grey-500 md:border-none block md:table-row">
                            <td class="ml-2 md:border md:border-grey-200 text-left block md:table-cell">
                                <span class="inline-block w-1/3 md:hidden font-bold"></span>
                                <div>
                                    <div class="ml-2 text-sm text-gray-800" id="index-title">
                                        ${item.detailBody}
                                    </div>
                                    <input type="hidden" value="${response.naceId}" name="NaceData.NaceId">
                                </div>
                            </td>
                            <td class="p-2 md:border md:border-grey-200 text-left block md:table-cell">
                                <span class="inline-block w-1/3 md:hidden font-bold"></span>
                                <div>
                                    <div class="text-sm text-gray-800 flex-col" id="item-detail-body-${item.detailId}">

                                    </div>
                                </div>
                            </td>
                        </tr>`);
                    var indexDetail = item.listItems;

                    if (indexDetail.length > 1) {

                        $(`#item-detail-body-${item.detailId}`).append(`<select name="NaceData.SelectItemDetails"
                                id="select-${selectListCounter}" class= "mt-1 block w-full pl-3 pr-10 py-2 text-base border-gray-300 focus:outline-none focus:ring-sky-700 focus:border-sky-700 sm:text-sm rounded-md"></select>`);
                        indexDetail.forEach(detail => {
                            $(`#select-${selectListCounter}`).append(`
                                    <option value="${detail.listItemDetailId}">
                                        ${detail.listItemDetail}
                                    </option>
                            `);
                        });
                        selectListCounter++;
                    } else {
                        indexDetail.forEach(detail => {
                            $(`#item-detail-body-${item.detailId}`).append(`
                                <div class='flex'>
                                    <input type="text" name="NaceData.ItemdetailValues"
                                            class="mt-1 w-2/3 border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-sky-700 focus:border-sky-500 sm:text-sm">
                                    <input type="hidden" name="NaceData.ItemdetailIndex" value="${detail.listItemDetailId}">
                                    <p class="self-center ml-2">${detail.listItemDetail}</p>
                                </div>`
                            );
                        });
                    }

                });
            }

        }
    });
}

function handleDeleteNaceData(id) {
    var deleteNaceData = {
        "url": `${host}/api/NaceData/DeleteSingleNaceData`,
        "method": "POST",
        "dataType": "json",
        "crossDomain": "true",
        "timeout": timeOut,
        "headers": {
            "Content-Type": "application/json",
            "Access-Control-Allow-Origin": "*"
        },
        "data": JSON.stringify({
            "Id": id
        }),
    };

    var GetNaceList = {
        "url": `${host}/api/Nace/GetAllNaces`,
        "method": "GET",
        "dataType": "json",
        "crossDomain": "true",
        "timeout": timeOut,
        "headers": {
            "Content-Type": "application/json",
            "Access-Control-Allow-Origin": "*"
        },
    };

    $.ajax(deleteNaceData).done(function (response) {
        if (response) {
            if (response.isSucceeded === true) {
                alert('Selected NACE is marked as deleted');
                $('#nace-data-wrapper').remove();
                $('#new-nace-wrapper').append(`
                        <div class="mt-6 grid grid-cols-12 gap-6">
                            <div class="col-span-12 sm:col-span-6">
                                <label class="block text-sm font-medium text-gray-700">
                                    NACE Code
                                </label>
                                <select name="Model.Command.NaceId" onchange="RenderTable(this.value)"
                                        class="mt-1 block w-full pl-3 pr-10 py-2 text-base border-gray-300 focus:outline-none focus:ring-sky-700 focus:border-sky-700 sm:text-sm rounded-md">
                                    <option value="">
                                        Please Select a NACE
                                    </option>
                                </select>
                                <p class="mt-2 text-sm text-gray-500">
                                    Statistical Classification of Economic Activities in the European Community
                                </p>
                            </div>
                            <div class="shadow border-b border-gray-200 sm:rounded-lg col-span-12 hidden" id="nace-table-wrapper">
                                <table class="min-w-full border-collapse block md:table" id="main-table">
                                    <thead class="block md:table-header-group">
                                    </thead>
                                    <tbody class="block md:table-row-group" id="nace-info-wrapper">
                                    </tbody>
                                </table>
                            </div>
                        </div>`);

                $.ajax(GetNaceList).done(function (response) {
                    if (response) {
                        response.forEach(item => {
                            $('select[name="Model.Command.NaceId"]').append(`<option value="${item.naceId}">
                                        ${item.title}
                                    </option>`);
                        });
                    }
                });
            }
        }
    });
}