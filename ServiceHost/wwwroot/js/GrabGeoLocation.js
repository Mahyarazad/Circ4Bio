var x = document.getElementById("edit-user-form");

mapboxgl.accessToken = 'pk.eyJ1IjoibWFoeWFyYXphZCIsImEiOiJjazhzaG9pNjIwYzJ4M2VyczJlNnNndzF6In0.ZFGc5daAFPaXObvBKA20CA';
function getLocation() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(showPosition, showError);
    }
    else { x.innerHTML = "Geolocation is not supported by this browser."; }
}


function showPosition(position) {
    $("#latitude").val(position.coords.latitude);
    $("#longitude").val(position.coords.longitude);
    $("#mapholder").css('display', 'block');
    $("#hideMap").css('display', 'block');
    const map = new mapboxgl.Map({
        container: 'mapholder', // container ID
        style: 'mapbox://styles/mapbox/streets-v11', // style URL
        center: [position.coords.longitude, position.coords.latitude], // starting position [lng, lat]
        zoom: 9 // starting zoom
    });

    // Create a default Marker and add it to the map.
    const marker = new mapboxgl.Marker({ color: 'red', rotation: 0 })
        .setLngLat([position.coords.longitude, position.coords.latitude])
        .addTo(map);
}

function hideMap() {
    $("#mapholder").css('display', 'none');
    $("#hideMap").css('display', 'none');
}

function showError(error) {

    if (error.code == 1) {
        x.innerHTML = "User denied the request for Geolocation."
    }
    else if (err.code == 2) {
        x.innerHTML = "Location information is unavailable."
    }
    else if (err.code == 3) {
        x.innerHTML = "The request to get user location timed out."
    }
    else {
        x.innerHTML = "An unknown error occurred."
    }
}