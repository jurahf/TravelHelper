var map;
var markerMe;
var pointsArr = [];
var tempCity = {};
var elementId;

function getGeolocationAndInitMap(points, city, elemId) {
    var options = {
        enableHighAccuracy: true,
        timeout: 5000,
        maximumAge: 0
    };
    pointsArr = points;

    if (city && city.Lat && city.Lng) {
        tempCity = city;
    }
    else {
        // Москва
        tempCity.Lat = 55.7545;
        tempCity.Lng = 37.6170;
    }

    navigator.geolocation.getCurrentPosition(successGeo, errorGeo, options);
    // с яндексом - сразу инит (геолокацию не смотрим)
    //initMapYandex(tempCity.Lat, tempCity.Lng);
}

function successGeo(pos) {
    var crd = pos.coords;
    initMapGoogle(crd.latitude, crd.longitude);
};

function errorGeo(err) {
    console.warn(`ERROR(${err.code}): ${err.message}`);

    initMapGoogle(tempCity.Lat, tempCity.Lng);
};

function initMapGoogle(lat, lng) {

    var location = new google.maps.LatLng(lat, lng);
    var mapCanvas = document.getElementById('map');

    var main_color = '#0085a1',
        saturation_value = -20,
        brightness_value = 5;
    var style = [
        {
            elementType: "labels",
            stylers: [
                { saturation: saturation_value }
            ]
        },
        {
            featureType: "poi",
            elementType: "labels",
            stylers: [
                { visibility: "off" }
            ]
        },
        {
            featureType: 'road.highway',
            elementType: 'labels',
            stylers: [
                { visibility: "off" }
            ]
        },
        {
            featureType: "road.local",
            elementType: "labels.icon",
            stylers: [
                { visibility: "off" }
            ]
        },
        {
            featureType: "road.arterial",
            elementType: "labels.icon",
            stylers: [
                { visibility: "off" }
            ]
        },
        {
            featureType: "road",
            elementType: "geometry.stroke",
            stylers: [
                { visibility: "off" }
            ]
        },
        {
            featureType: "transit",
            elementType: "geometry.fill",
            stylers: [
                //{ hue: main_color },
                { visibility: "on" },
                { lightness: brightness_value },
                { saturation: saturation_value }
            ]
        },
        {
            featureType: "poi",
            elementType: "geometry.fill",
            stylers: [
                //{ hue: main_color },
                { visibility: "on" },
                { lightness: brightness_value },
                { saturation: saturation_value }
            ]
        },
        {
            featureType: "poi.government",
            elementType: "geometry.fill",
            stylers: [
                //{ hue: main_color },
                { visibility: "on" },
                { lightness: brightness_value },
                { saturation: saturation_value }
            ]
        },
        {
            featureType: "poi.sport_complex",
            elementType: "geometry.fill",
            stylers: [
                //{ hue: main_color },
                { visibility: "on" },
                { lightness: brightness_value },
                { saturation: saturation_value }
            ]
        },
        {
            featureType: "poi.attraction",
            elementType: "geometry.fill",
            stylers: [
                //{ hue: main_color },
                { visibility: "on" },
                { lightness: brightness_value },
                { saturation: saturation_value }
            ]
        },
        {
            featureType: "poi.business",
            elementType: "geometry.fill",
            stylers: [
                //{ hue: main_color },
                { visibility: "on" },
                { lightness: brightness_value },
                { saturation: saturation_value }
            ]
        },
        {
            featureType: "transit",
            elementType: "geometry.fill",
            stylers: [
                //{ hue: main_color },
                { visibility: "on" },
                { lightness: brightness_value },
                { saturation: saturation_value }
            ]
        },
        {
            featureType: "transit.station",
            elementType: "geometry.fill",
            stylers: [
                //{ hue: main_color },
                { visibility: "on" },
                { lightness: brightness_value },
                { saturation: saturation_value }
            ]
        },
        {
            featureType: "landscape",
            stylers: [
                //{ hue: main_color },
                { visibility: "on" },
                { lightness: brightness_value },
                { saturation: saturation_value }
            ]

        },
        {
            featureType: "road",
            elementType: "geometry.fill",
            stylers: [
                //{ hue: main_color },
                { visibility: "on" },
                { lightness: brightness_value },
                { saturation: saturation_value }
            ]
        },
        {
            featureType: "road.highway",
            elementType: "geometry.fill",
            stylers: [
                //{ hue: main_color },
                { visibility: "on" },
                { lightness: brightness_value },
                { saturation: saturation_value }
            ]
        },
        {
            featureType: "water",
            elementType: "geometry",
            stylers: [
                //{ hue: main_color },
                { visibility: "on" },
                { lightness: brightness_value },
                { saturation: saturation_value }
            ]
        }
    ];

    var mapOptions = {
        center: location,
        zoom: 14,
        panControl: false,
        zoomControl: true,
        mapTypeControl: false,
        streetViewControl: false,
        mapTypeId: google.maps.MapTypeId.ROADMAP,
        scrollwheel: true,
        styles: style,
        zoomControlOptions: {
            style: google.maps.ZoomControlStyle.SMALL
        }
    };
    map = new google.maps.Map(mapCanvas, mapOptions);

    markerMe = AddMarker(location, 'Я здесь', '/Images/marker-red-small.png', true);
    AddMarkerLabel(markerMe, 'Я');

    // добавим маркеры из текущего путешествия
    for (var i = 0; i < pointsArr.length; i++) {
        var point = pointsArr[i];
        if (point.Lat != 0 || point.Lng != null) {
            var point_location = new google.maps.LatLng(point.Lat, point.Lng);
            var point_marker = AddMarker(point_location, point.Caption, getImageForPoint(point));
            AddMarkerLabel(point_marker, point.Description);
        }
    }
}

function initMapYandex(lat, lng) {
    // добавим саму карту
    var myMap = new ymaps.Map("map", {
        center: [lat, lng],
        zoom: 11
    });
    // TODO:
    // центр в нужной точке
    // маркер Я
    // маркеры из текущего путешествия
}

function getImageForPoint(point) {
    // TODO: point.Category, уже посещенные
    return point.Today ? '/Images/marker-green-small.png' : '/Images/marker-disabled-small.png'
}

function ShowTripTo(finishLat, finishLng) {
    ShowTrip(
        markerMe.position.lat(),
        markerMe.position.lng(),
        finishLat,
        finishLng);
}

var directionsDisplay = new google.maps.DirectionsRenderer();
var directionsService = new google.maps.DirectionsService;
function ShowTrip(startLat, startLng, finishLat, finishLng) {
    var request = {
        origin: new google.maps.LatLng(startLat, startLng),
        destination: new google.maps.LatLng(finishLat, finishLng),
        travelMode: google.maps.DirectionsTravelMode.WALKING
        //google.maps.TravelMode.DRIVING — Автомобиль
        //google.maps.TravelMode.BICYCLING — Велосипед
        //google.maps.TravelMode.TRANSIT – Общественный транспорт (работает не везде)
        //google.maps.TravelMode.WALKING — Пешеход
    };

    directionsService.route(request, function (response, status) {
        if (status == google.maps.DirectionsStatus.OK) {
            directionsDisplay.setDirections(response);
        }
    });

    directionsDisplay.setMap(map);
}

function AddMarker(position, title, image, draggable) {
    var marker = new google.maps.Marker({
        position: position,
        title: title,
        map: map,
        icon: image,
        draggable: draggable
    });

    return marker;
}

function AddMarkerLabel(marker, label) {
    marker.label = label;
}

function AddMarkerImage() {

}