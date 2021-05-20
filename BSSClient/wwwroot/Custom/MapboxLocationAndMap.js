﻿mapboxgl.accessToken = 'pk.eyJ1IjoianVyYWhmIiwiYSI6ImNra253dzkweDM1M3QycXF0ZnF5MzZzMjUifQ.RlwEwOAgzieIbpxdy4TyYQ';
var map;
var canvas;

var tempDate;
var tempPointIndex = 0;

var init = false;
var arrPoints = [];
var allMarkers = [];
var routs = [];
var layerNames = [];


// Lng, Lat
const moscow = [37.6170, 55.7545];
const perm = [56.25107070278494, 58.00412682942334];


function resetToDefault() {
    init = false;
    arrPoints = [];
    allMarkers = [];
    routs = [];
    layerNames = [];
}


function getGeolocationAndInitMap(arrPoints, city, elemId, currDate, currPoint) {
    console.log('getGeolocationAndInitMap');

    resetToDefault();
    this.arrPoints = arrPoints;

    if (!city) {
        city = {};
        city.lng = moscow[0];
        city.lat = moscow[1];
    }

    // сама карта
    map = new mapboxgl.Map({
        container: elemId,
        style: 'mapbox://styles/mapbox/streets-v11',
        center: [city.lng, city.lat],
        zoom: 12.0
    });

    // геокодирование
    var geocoder = new MapboxGeocoder({
        accessToken: mapboxgl.accessToken,
        language: 'ru-RU',
        mapboxgl: mapboxgl
    });

    map.addControl(geocoder);


    // геолокация
    var geoLocate = new mapboxgl.GeolocateControl({
        positionOptions: {
            enableHighAccuracy: true
        },
        trackUserLocation: true,
        showUserLocation: true
    })

    map.addControl(geoLocate);


    // установка языка, точек
    map.on('sourcedata', (e) => {
        if (map.isStyleLoaded() && init == false) {
            init = true;
            setLang('ru');
            map.off('sourcedata');

            console.log('map.on sourcedata');

            selectDateAndPoint(currDate, currPoint);
        }
    });

    map.on('load', function () {
        //geoLocate.trigger();
    });


    canvas = map.getCanvasContainer();
}


function FillArrPoints() {
    console.log('FillArrPoints (' + arrPoints.length + ')');

    arrPoints.forEach(function (point) {
        addMarkerAndFullRoute([point.lng, point.lat], point.addrId,  point.caption, point.shortDescription, point.description, point.imageLink, point.date, point.order);
    });
}


function selectDateAndPoint(date, pointIndex) {
    console.log('selectDateAndPoint');

    tempDate = date;
    tempPointIndex = pointIndex;

    clearMarkersAndRouts();
    FillArrPoints();
}



function addMarkerAndFullRoute(point, addrId, caption, shortDescription, description, imageLink, date, order) {
    var markerColor = '#3FB1CE';
    var today = date == tempDate;
    if (today) {
        if (order <= tempPointIndex) {
            markerColor = 'green';
        }
        else if (order == tempPointIndex + 1) {
            markerColor = 'yellow'
        }
        else {
            markerColor = 'gray';
        }
    }

    addMarker(point, addrId, caption, shortDescription, description, imageLink, markerColor);


    if (today) {
        var routeColor = '#3FB1CE';
        if (order == tempPointIndex + 1)
            routeColor = '#be3887'

        addRoutePoint(point, routeColor);
    }
}




function clearMarkersAndRouts() {
    console.log('clear');

    for (var i = 0; i < layerNames.length; i++)
        map.removeLayer(layerNames[i]);

    layerNames = [];

    for (var i = 0; i < allMarkers.length; i++)
        allMarkers[i].remove();

    allMarkers = [];
}



function addMarker(point, addrId, title, shortDescription, description, imageLink, color) {

    var link = '';
    if (shortDescription != description)
        link = '<a href="/AddrInfo/' + addrId + '">подробно...</a>';

    var img = '';
    if (imageLink)
        img = '<div> <img src="' + imageLink + '" class="small-img" /> </div>';

    var marker = new mapboxgl.Marker({ color: color })
        .setLngLat(point)
        .setPopup(new mapboxgl.Popup({ offset: 25 }) // add popups
            .setHTML(img + '<h5>' + title + '</h5><p>' + shortDescription + '</p> ' + link))
        .addTo(map);
    allMarkers.push(marker);

}


function setLang(language) {
    var props =
        ['country-label',   // страны
            'state-label',
            'settlement-label', // города
            'settlement-subdivision-label', // районы города
            'airport-label',
            /*'poi-label',*/        // объекты социальной инфраструктуры
            'water-point-label',        // моря
            'water-line-label',         // другие моря
            'natural-point-label',
            'natural-line-label',
            /*'waterway-label', */          // реки и каналы
            /*'road-label',*/       // улицы
        ]

    props.forEach(element => {
        map.setLayoutProperty(
            element,
            'text-field',
            [
                'get',
                'name_' + language
            ]);
    });
}


function addRoutePoint(point, color) {
    routs.push(point);

    if (routs.length > 1) {
        getRoute(routs[routs.length - 2], routs[routs.length - 1], 'walking', 'route-' + (routs.length - 1), color);
    }
}

function getRoute(start, end, profile, layerName, color) {

    if (layerName == undefined)
        layerName = 'route';

    // walking, cycling, driving, driving-traffic
    var url = 'https://api.mapbox.com/directions/v5/mapbox/' + profile + '/' + start[0] + ',' + start[1] + ';' + end[0] + ',' + end[1] + '?steps=true&geometries=geojson&access_token=' + mapboxgl.accessToken;
    // make an XHR request https://developer.mozilla.org/en-US/docs/Web/API/XMLHttpRequest
    var req = new XMLHttpRequest();
    req.open('GET', url, true);
    req.onload = function () {
        var json = JSON.parse(req.response);
        var data = json.routes[0];
        var route = data.geometry.coordinates;
        var geojson = {
            type: 'Feature',
            properties: {},
            geometry: {
                type: 'LineString',
                coordinates: route
            }
        };

        // if the route already exists on the map, reset it using setData
        if (map.getSource(layerName)) {
            map.getSource(layerName).setData(geojson);
        } else {
            // add route line
            layerNames.push(layerName);
            map.addLayer({
                id: layerName,
                type: 'line',
                source: {
                    type: 'geojson',
                    data: {
                        type: 'Feature',
                        properties: {},
                        geometry: {
                            type: 'LineString',
                            coordinates: geojson
                        }
                    }
                },
                layout: {
                    'line-join': 'round',
                    'line-cap': 'round'
                },
                paint: {
                    'line-color': color,
                    'line-width': 5,
                    'line-opacity': 0.75
                }
            });
            map.getSource(layerName).setData(geojson);
        }
        // add turn instructions here at the end
    };
    req.send();
}


function setOnClickHandler() {
    map.on('click', function (e) {
        var coordsObj = e.lngLat;
        canvas.style.cursor = '';

        var coords = Object.keys(coordsObj).map(function (key) {
            return coordsObj[key];
        });

        // или mapbox.places-permanent                           
        var url = 'https://api.mapbox.com/geocoding/v5/mapbox.places/' + coords[0] + ',' + coords[1] + '.json?access_token=' + mapboxgl.accessToken;

        var req = new XMLHttpRequest();
        req.open('GET', url, true);
        req.onload = function () {
            var json = JSON.parse(req.response);
            var feature = json.features[0];


            alert(feature.text + '\n\r' + feature.properties.category + '\n\r' + feature.properties.address);
        };
        req.send();
    });
}


