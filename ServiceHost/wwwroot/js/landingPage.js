"use strict";
var host = "https://localhost:5001";
//var host = "http://www.maahyarazad.ir";

$("#search-input-landing").on("keyup",
    function () {
        var filtered;
        var value = $(this).val();

        if (value.length > 0) {
            $("#landing-page-search-overlay").removeClass('hidden');
            $("#landing-page-search-overlay").show();
            $.get(
                `${host}/api/marketlisting`,
                function (data) {
                    var marketListingItemsObject = data;
                    filtered = marketListingItemsObject.filter(function (data) {
                        return data.name.toLowerCase().includes(value.toLowerCase()) ||
                            data.description.toLowerCase().includes(value.toLowerCase()) ||
                            data.email.toLowerCase().includes(value.toLowerCase()) ||
                            data.fullName.toLowerCase().includes(value.toLowerCase())
                    });
                    $("#search-result-landing-page").removeClass("hidden");
                    $("#search-result-landing-page").removeClass("invisible");
                    $("#landing-page-result-wrapper").empty();

                    if (filtered.length === 0) {
                        $("#landing-page-result-wrapper").append(`
                        <div class="flex justify-between border-b py-1 mt-1">
                            <div>
                                <p class="text-base font-medium pb-2">No Result</p>
                                </div>
                        </div >`);
                        $("#landing-page-search-overlay").addClass('hidden');
                        $("#landing-page-search-overlay").hide();
                    } else {
                        filtered.forEach(e => {
                            $("#landing-page-result-wrapper").append(`
                        <a href="/marketplace/itemdetail/${e.id}">
                            <div class="flex justify-between border-b py-1 mt-1">
                                <div>
                                    <p class="text-xs font-medium">${e.name[0].toUpperCase()}${e.name.slice(1)}</p>
                                    <p class="text-xs">${e.isService ? "Service Tarriff" : "Available Amount"}: ${e.isService ? "" : '<b>' + e.amount + '</b>'} <b>${e.unit.toUpperCase()
                                }</b>  Unit Price: <b>${e.unitPrice} ${e.currency}</b></p>
                                </div>
                                <img class="w-10 h-10 place-self-center rounded-md" src="${host}/Site Files/Listing_Images/${e.image === "listing-default.png" ? "listing-default.png" : e.type + "/" + e.image}"/>
                            </div >
                        </a>`);
                        });
                        $("#landing-page-search-overlay").addClass('hidden');
                        $("#landing-page-search-overlay").hide();
                    }
                });
        } else {
            $("#landing-page-search-overlay").addClass('hidden');
            $("#landing-page-search-overlay").hide();
            $("#search-result-landing-page").addClass("hidden");
            $("#search-result-landing-page").addClass("invisible");
            $("#landing-page-result-wrapper").empty();
        }

    });