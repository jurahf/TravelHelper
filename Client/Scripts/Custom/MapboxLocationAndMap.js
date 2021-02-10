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
        center: moscow,
        zoom: 12.0
    });

    var geocoder = new MapboxGeocoder({
        accessToken: mapboxgl.accessToken,
        language: 'ru-RU',
        mapboxgl: mapboxgl
    });

    map.addControl(geocoder);

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
                addMarker([point.Lng, point.Lat], point.Caption, point.Description, point.Today);
            });

        }
    });

    canvas = map.getCanvasContainer();
}


function addMarker(point, title, description, today) {
    var marker = new mapboxgl.Marker(today ? { color: 'green' } : { color: '#3FB1CE' })
        .setLngLat(point)
        .setPopup(new mapboxgl.Popup({ offset: 25 }) // add popups
            .setHTML('<h3>' + title + '</h3><p>' + description + '</p>'))
        .addTo(map);
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




function getRoute(start, end, profile) {
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
        if (map.getSource('route')) {
            map.getSource('route').setData(geojson);
        } else { // otherwise, make a new request

            // Add starting point to the map
            map.addLayer({
                id: 'point-start',
                type: 'circle',
                source: {
                    type: 'geojson',
                    data: {
                        type: 'FeatureCollection',
                        features: [{
                            type: 'Feature',
                            properties: {},
                            geometry: {
                                type: 'Point',
                                coordinates: start
                            }
                        }
                        ]
                    }
                },
                paint: {
                    'circle-radius': 10,
                    'circle-color': '#3887be'
                }
            });

            map.addLayer({
                id: 'point-end',
                type: 'circle',
                source: {
                    type: 'geojson',
                    data: {
                        type: 'FeatureCollection',
                        features: [{
                            type: 'Feature',
                            properties: {},
                            geometry: {
                                type: 'Point',
                                coordinates: end
                            }
                        }
                        ]
                    }
                },
                paint: {
                    'circle-radius': 10,
                    'circle-color': '#38be87'
                }
            });

            // add route line
            map.addLayer({
                id: 'route',
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
                    'line-color': '#be3887',
                    'line-width': 5,
                    'line-opacity': 0.75
                }
            });
            map.getSource('route').setData(geojson);
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