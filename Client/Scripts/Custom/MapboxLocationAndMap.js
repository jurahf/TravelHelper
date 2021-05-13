mapboxgl.accessToken = 'pk.eyJ1IjoianVyYWhmIiwiYSI6ImNra253dzkweDM1M3QycXF0ZnF5MzZzMjUifQ.RlwEwOAgzieIbpxdy4TyYQ';
var map;
var canvas;
var init = false;

const moscow = [37.6170, 55.7545];
const perm = [56.25107070278494, 58.00412682942334];

function getGeolocationAndInitMap(arrPoints, city, elemId) {

    map = new mapboxgl.Map({
        container: elemId,
        style: 'mapbox://styles/mapbox/streets-v11',
        center: [city.Lng, city.Lat],
        zoom: 12.0
    });

    var geocoder = new MapboxGeocoder({
        accessToken: mapboxgl.accessToken,
        language: 'ru-RU',
        mapboxgl: mapboxgl
    });

    map.addControl(geocoder);

    var geoLocate = new mapboxgl.GeolocateControl({
            positionOptions: {
                enableHighAccuracy: true
            },
            trackUserLocation: true,
            showUserLocation: true
    })

    map.addControl(geoLocate);


    /*
    map.addControl(new MapboxDirections({
        accessToken: mapboxgl.accessToken
    }),
    'top-left'
    );
    */
    map.on('sourcedata', (e) => {
        if (map.isStyleLoaded() && init == false) {
            init = true;
            setLang('ru');
            map.off('sourcedata');

            arrPoints.forEach(function (point) {
                addMarkerAndFullRoute([point.Lng, point.Lat], point.Caption, point.ShortDescription, point.Description, point.ImageLink, point.Today, point.Order);
            });
        } 
    });

    map.on('load', function () {
        geoLocate.trigger();
    });

    canvas = map.getCanvasContainer();
}





function addMarkerAndFullRoute(point, caption, shortDescription, description, imageLink, today, order) {
    var markerColor = '#3FB1CE';
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

    addMarker(point, caption, shortDescription, description, imageLink, markerColor);


    if (today == true) {
        var routeColor = '#3FB1CE';
        if (order == tempPointIndex + 1)
            routeColor = '#be3887'

        addRoutePoint(point, routeColor);
    }
}



function nextPoint(arrPoints) {
    if (arrPoints[tempPointIndex]) {
        if (tripSetted) { // маршрут уже проложен, по-умолчанию идем к следующей точке
            moveTempPointIndex();
            pointWasPassed();
        }
        else {
            tripSetted = true;
        }

        var point = arrPoints[tempPointIndex];

        while (point.Lat == 0) {    // костыль. вообще нулей быть не должно
            moveTempPointIndex();
            point = arrPoints[tempPointIndex];
        }
        pointWasPassed();

        clearMarkersAndRouts();
        arrPoints.forEach(function (point) {
            addMarkerAndFullRoute([point.Lng, point.Lat], point.Caption, point.ShortDescription, point.Description, point.ImageLink, point.Today, point.Order);
        });
    }
}


function clearMarkersAndRouts() {
    //routs = [];

    for (var i = 0; i < layerNames.length; i++)
        map.removeLayer(layerNames[i]);

    layerNames = [];

    for (var i = 0; i < allMarkers.length; i++)
        allMarkers[i].remove();

    allMarkers = [];
}


function moveTempPointIndex() {
    tempPointIndex++;

    if (tempPointIndex > maxPointIndex) {
        tempPointIndex = minPointIndex;
    }
}


function pointWasPassed() {
    // TODO: отправить запрос на сервер - надо отметить, что точка посещена
    // данные брать из:
    //var pointsArr = [];   // точки для отображения на карте
    //var tempPointIndex = 0;
}



var allMarkers = [];
function addMarker(point, title, shortDescription, description, imageLink, color) {

    var link = '';
    if (shortDescription != description)
        link = '<a href="#" onclick="ShowInfo(\'' + title + '\', \'' + description + '\', \'' + imageLink + '\'); ">подробно...</a>';

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


var routs = [];
function addRoutePoint(point, color) {
    routs.push(point);

    if (routs.length > 1) {
        getRoute(routs[routs.length - 2], routs[routs.length - 1], 'walking', 'route-' + (routs.length - 1), color);
    }
}

var layerNames = [];
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
        } else { // otherwise, make a new request

            // Add starting point to the map
            //map.addLayer({
            //    id: 'point-start',
            //    type: 'circle',
            //    source: {
            //        type: 'geojson',
            //        data: {
            //            type: 'FeatureCollection',
            //            features: [{
            //                type: 'Feature',
            //                properties: {},
            //                geometry: {
            //                    type: 'Point',
            //                    coordinates: start
            //                }
            //            }
            //            ]
            //        }
            //    },
            //    paint: {
            //        'circle-radius': 10,
            //        'circle-color': '#3887be'
            //    }
            //});

            //map.addLayer({
            //    id: 'point-end',
            //    type: 'circle',
            //    source: {
            //        type: 'geojson',
            //        data: {
            //            type: 'FeatureCollection',
            //            features: [{
            //                type: 'Feature',
            //                properties: {},
            //                geometry: {
            //                    type: 'Point',
            //                    coordinates: end
            //                }
            //            }
            //            ]
            //        }
            //    },
            //    paint: {
            //        'circle-radius': 10,
            //        'circle-color': '#38be87'
            //    }
            //});

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



function ShowTripTo() {
    // TODO
}