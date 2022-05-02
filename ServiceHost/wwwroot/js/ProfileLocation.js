var x = document.getElementById("location-message");
//mapboxgl.accessToken = @Html.Raw(Json.Serialize(_Configuration.GetSection("MapBox")["SecretKey"]));
mapboxgl.accessToken = "pk.eyJ1IjoibWFoeWFyYXphZCIsImEiOiJjazhzaG9pNjIwYzJ4M2VyczJlNnNndzF6In0.ZFGc5daAFPaXObvBKA20CA";

function showPosition(position) {
    $("#latitude").val(position.coords.latitude);
    $("#longitude").val(position.coords.longitude);
    $("#mapholder").css('display', 'block');
    $("#hideMap").css('display', 'block');

    $("#get-current-location-button").text("Get Location");
    $("#location-loader").addClass('hidden');

    const map = new mapboxgl.Map({
        container: 'mapholder', // container ID
        style: 'mapbox://styles/mapbox/streets-v11', // style URL
        center: [position.coords.longitude, position.coords.latitude], // starting position [lng, lat]
        zoom: 9 // starting zoom
    });
    // Create a default Marker and add it to the map.
    const marker = new mapboxgl.Marker({ color: 'red', rotation: 0, draggable: true })
        .setLngLat([position.coords.longitude, position.coords.latitude])
        .addTo(map);


    marker.on('drag',
        function (e) {
            $("#latitude").val(e.target._lngLat.lat.toFixed(6));
            $("#longitude").val(e.target._lngLat.lng.toFixed(6));
        });
}

function hideMap() {
    $("#mapholder").css('display', 'none');
    $("#hideMap").css('display', 'none');
}

var hideLocationErrorSpan = () => {
    $("#location-message-wrapper").addClass('invisible');
    $("#location-message-wrapper").addClass('fixed');
    $("#location-message-wrapper").addClass('top-0');
    $("#location-message-wrapper").addClass('-translate-y-[2rem]');
}

var showLocationErrorSpan = () => {
    $("#location-message-wrapper").removeClass('invisible');
    $("#location-message-wrapper").removeClass('fixed');
    $("#location-message-wrapper").removeClass('top-0');
    $("#location-message-wrapper").removeClass('-translate-y-[2rem]');
}

function showError(error) {
    showLocationErrorSpan();
    if (error.code == 1) {
        x.innerHTML = "User denied the request for geolocation."
    } else if (error.code == 2) {
        x.innerHTML = "Location information is unavailable."
    } else if (error.code == 3) {
        x.innerHTML = "The request to get user location timed out."
    } else {
        x.innerHTML = "An unknown error occurred."
    }
    $("#get-current-location-button").text("Get Location");
    $("#location-loader").addClass('hidden');

    setTimeout(() => {
        hideLocationErrorSpan();
    },
        5000);

}



function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#user-avatar-image').attr('src', e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
}

function readURLMobile(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#user-avatar-image-mobile').attr('src', e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
}



$("#user-avatar").change(function () {
    readURL(this);
});

$("#user-avatar-mobile").change(function () {
    readURLMobile(this);
});

function getLocation() {
    $("#get-current-location-button").text("");
    $("#location-loader").removeClass('hidden');
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition, showError);

    } else {
        showLocationErrorSpan();
        x.innerHTML = "Geolocation is not supported by this browser.";
    }
}