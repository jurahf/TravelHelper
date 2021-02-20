$('#new-travel-modal').on('hidden.bs.modal', function () {
    ClearTravelValidation();
    ClearTravelSecondTab();
});

$('#schedule-modal').on('hidden.bs.modal', function () {
    ClearScheduleValidation();
});

var currUrl = "Home/Index";

function initSettings(_currUrl) {
    currUrl = _currUrl;
}


/******************************* Schedule modal *******************************************/
function exchangeScheduleRows(x, y) {
    var firstText = x.children('td.name-cell').first().text();
    x.children('td.name-cell').first().text(y.children('td.name-cell').first().text());
    y.children('td.name-cell').first().text(firstText);

    var firstValue = x.children('td.name-cell').first().attr('value');
    x.children('td.name-cell').first().attr('value', y.children('td.name-cell').first().attr('value'));
    y.children('td.name-cell').first().attr('value', firstValue);

    var firstValue = x.attr('value');
    x.attr('value', y.attr('value'));
    y.attr('value', firstValue);
}

function rowUp(tableSelector, rowSelector) {
    $prevRow = $(tableSelector + ' ' + rowSelector).prev();
    $currRow = $(tableSelector + ' ' + rowSelector);

    exchangeScheduleRows($currRow, $prevRow);
}

function rowDown(tableSelector, rowSelector) {
    $currRow = $(tableSelector + ' ' + rowSelector);
    $nextRow = $(tableSelector + ' ' + rowSelector).next();

    exchangeScheduleRows($currRow, $nextRow);
}

function rowDelete(tableSelector, rowSelector) {
    $(tableSelector + ' ' + rowSelector).remove();
}

function rowAdd(tableSelector) {
}

function rowsSave(tableSelector) {
    var schedules = [];
    var id = $('#tableTempSchedule').attr('value');

    $('#tableTempSchedule tbody tr').each(function () {
        var placePointId = $(this).attr('value');
        var time = $(this).find('.time-cell').attr('value');
        var name = $(this).find('.name-cell').attr('value');

        var sch = {};
        sch.DateTime = time;
        //sch.Address = address;
        sch.Name = name;
        sch.PlacePointId = placePointId;
        schedules.push(sch);
    });
    
    var data = {};
    data.ScheduleId = id;
    data.UserLogin = 'TestUser';            // TODO: !
    data.ScheduleRows = schedules;

    // отправляем на сервер
    ClearScheduleValidation();
    $('#scheduleProgress').removeClass('hidden');
    var json = JSON.stringify(data);

    var xmlHttp = new XMLHttpRequest(); // вообще-то json
    xmlHttp.open("PUT", travelHelperServerUrl + "/api/Travel/SaveSchedule", true); // false for synchronous request
    xmlHttp.setRequestHeader('Content-type', 'application/json; charset=utf-8');
    xmlHttp.onload = function () {
        var response = JSON.parse(xmlHttp.responseText);

        $('#scheduleProgress').addClass('hidden');

        if (response['Valid'] == false) {
            $('#scheduleErrText').text(response.ErrorMessage);
            $('#scheduleErrContainer').removeClass('hidden');
        }
        else {
            // закрываем окно и выбираем текущее путешествие (перезагружаем страничку)
            window.location.replace(currUrl);
        }
    }
    xmlHttp.send(json);
}

function ClearScheduleValidation() {
    $('#scheduleErrContainer').addClass('hidden');
}


/******************************* Travel modal *********************************************/

function ValidAndGetTravelDataFirstTab() {
    // данные с формы
    var valid = true;
    var city = $('#tbCity').val().trim();
    var startDate = $('#date-range-start').val().trim();
    var endDate = $('#date-range-end').val().trim();
    var categories = [];
    $('#categories-list input:checked').each(function (index) {
        if ($(this).val()) {
            categories.push($(this).val());
        }
    });

    // валидация
    if (!city) {
        SetCityInvalid();
        valid = false;
    }

    if (!startDate) {
        SetStartDateInvalid();
        valid = false;
    }

    if (!endDate) {
        SetEndDateInvalid();
        valid = false;
    }

    if (valid == true) {
        var data = {};
        data.City = city;
        data.StartDate = startDate;
        data.EndDate = endDate;
        data.Categories = categories;
        data.UserLogin = 'TestUser';            // TODO: !

        return data;
    }
    else
        return null;
}

function ValidAndGetTravelDataSecondTab() {
    var schedules = [];
    var tempDate = '';
    $('.generated-schedule-row').each(function () {
        var rowValue = $(this).attr('value');

        if ($(this).hasClass('date-row')) {
            tempDate = rowValue;    // дата расписания
        }
        else {
            var time = $(this).find('.time-cell').attr('value');
            var name = $(this).find('.name-cell').attr('value');

            var sch = {};
            sch.DateTime = tempDate + 'T' + time + ':00';
            //sch.Address = rowValue;
            sch.Name = name;
            sch.AddressInfo = addressesData[rowValue];
            schedules.push(sch);
        }
    });

    var additionalPlaces = [];
    $('.place-item').each(function () {
        var name = $(this).find('.place-name').attr('value');
        var value = $(this).find('.place-value').attr('value');

        var addPl = {};
        addPl.Name = name;
        addPl.Value = value;
        additionalPlaces.push(addPl);
    });

    var data = {};
    data.Schedules = schedules;
    data.AdditionalPlaces = additionalPlaces;
    return data;
}

function saveTravel(travelHelperServerUrl) {
    var firstTabData = ValidAndGetTravelDataFirstTab();
    var secondTabData = ValidAndGetTravelDataSecondTab();

    if (firstTabData && secondTabData) {
        var data = {};
        data.City = firstTabData.City;
        data.StartDate = firstTabData.StartDate;
        data.EndDate = firstTabData.EndDate;
        data.Categories = firstTabData.Categories;
        data.UserLogin = firstTabData.UserLogin;
        data.Schedules = secondTabData.Schedules;
        data.AdditionalPlaces = secondTabData.AdditionalPlaces;

        ClearTravelValidation();

        $('#travelProgress').removeClass('hidden');

        // отправляем на сервер
        var json = JSON.stringify(data);

        var xmlHttp = new XMLHttpRequest(); // вообще-то json
        xmlHttp.open("PUT", travelHelperServerUrl + "/api/Travel/SaveTravel", true); // false for synchronous request
        xmlHttp.setRequestHeader('Content-type', 'application/json; charset=utf-8');
        xmlHttp.onload = function () {
            var response = JSON.parse(xmlHttp.responseText);

            $('#travelProgress').addClass('hidden');

            if (response['Valid'] == false) {
                $('#travelErrText').text(response.ErrorMessage);
                $('#travelErrContainer').removeClass('hidden');
            }
            else {
                // закрываем окно и выбираем текущее путешествие (перезагружаем страничку)
                window.location.replace(currUrl);
            }
        }
        xmlHttp.send(json);
    }
}


function preSaveTravel(travelHelperServerUrl) {
    if ($('#tabSchedule').hasClass('active')) { // нажали "назад"
        BackTravelTab();
        return;
    }

    var data = ValidAndGetTravelDataFirstTab();

    if (data) {
        // валидация успешна
        ClearTravelValidation();
        ClearTravelSecondTab()

        $('#travelProgress').removeClass('hidden');

        // отправляем на сервер
        var json = JSON.stringify(data);

        var xmlHttp = new XMLHttpRequest(); // вообще-то json
        xmlHttp.open("PUT", travelHelperServerUrl + "/api/Travel/PreSaveTravel", true); // false for synchronous request
        xmlHttp.setRequestHeader('Content-type', 'application/json; charset=utf-8');
        xmlHttp.onload = function () {
            var response = JSON.parse(xmlHttp.responseText);

            $('#travelProgress').addClass('hidden');

            if (response['Valid'] == false) {
                $('#travelErrText').text(response.ErrorMessage);
                $('#travelErrContainer').removeClass('hidden');
            }
            else {
                GenerateScheduleTable(response);
                ForwardTravelTab();
            }
        }
        xmlHttp.send(json);
    }
}



var addressesData = [];
function GenerateScheduleTable(json) {
    addressesData = [];
    var schedules = json['Schedules'];

    for (var i = 0; i < schedules.length; i++) {
        $('#generated-schedule tbody').append(
            '<tr class="generated-schedule-row date-row" value="' + utcDateTimeToDate(schedules[i]['Date']) + '">' +
            '<td colspan="4"><b>' + utcDateTimeToDate(schedules[i]['Date']) + '</b></td></tr>');

        for (var j = 0; j < schedules[i]['PlacePoint'].length; j++) {
            var point = schedules[i]['PlacePoint'][j];

            if (point['NaviAddressInfo'])
                addressesData.push(point['NaviAddressInfo']);

            $('#generated-schedule tbody').append(
                '<tr id="gen-row-' + i + '_' + j + '" class="generated-schedule-row" value="' +
                            (point['NaviAddressInfo']
                                ? addressesData.length - 1
                                : '')
                                + '">' +
                    '<td>' +
                    '</td>' +
                    '<td class="name-cell" value="' + point['CustomName'] + '">' + point['CustomName'] + '</td>' +
                    '<td class="time-cell" value="' + utcDateTimeToTime(point['Time']) + '">' + utcDateTimeToTime(point['Time']) + '</td>' +
                    '<td>' +
                    '    <div class="btn-group" role="group">' +
                    '        <button type="button" onclick="rowUp(\'#generated-schedule\', \'#gen-row-' + i + '_' + j + '\');" class="btn btn-default' + (j == 0 ? 'disabled' : '') + '")">' +
                    '            <span class="glyphicon glyphicon-arrow-up" aria-hidden="true"></span>' +
                    '        </button>' +
                    '        <button type="button" onclick="rowDown(\'#generated-schedule\', \'#gen-row-' + i + '_' + j + '\');" class="btn btn-default' + (j == (schedules[i]['PlacePoint'].length - 1) ? 'disabled' : '') + '")">' +
                    '            <span class="glyphicon glyphicon-arrow-down" aria-hidden="true"></span>' +
                    '        </button>' +
                    '        <button type="button" onclick="rowDelete(\'#generated-schedule\', \'#gen-row-' + i + '_' + j + '\');" class="btn btn-default">' +
                    '            <span class="glyphicon glyphicon-trash" aria-hidden="true"></span>' +
                    '        </button>' +
                    '    </div>' +
                    '</td>' +
                '</tr>'
                );
        }
    }
}

function utcDateTimeToDate(dateString) {
    return dateString.substring(0, dateString.indexOf('T'));
}

function utcDateTimeToTime(dateString) {
    return dateString.substring(dateString.indexOf('T') + 1, dateString.lastIndexOf(':'));
}

function ForwardTravelTab() {
    $('#tabTravel').removeClass('active', 100);
    $('#tabSchedule').addClass('active', 500);
    $('#btnTravelForward').text('Назад');
    $('#btnTravelSave').removeClass('disabled');
}

function BackTravelTab() {
    $('#tabTravel').addClass('active', 500);
    $('#tabSchedule').removeClass('active', 100);
    $('#btnTravelForward').text('Вперед');
    $('#btnTravelSave').addClass('disabled');
}

function SetCityInvalid() {
    $('#city-container').addClass('has-error');
    $('#city-validation').addClass('glyphicon-remove');
}

function SetStartDateInvalid() {
    $('#from-container').addClass('has-error');
    $('#from-validation').addClass('glyphicon-remove');
}

function SetEndDateInvalid() {
    $('#to-container').addClass('has-error');
    $('#to-validation').addClass('glyphicon-remove');
}

function ClearTravelValidation() {
    $('#city-container').removeClass('has-error');
    $('#city-validation').removeClass('glyphicon-remove');
    $('#from-container').removeClass('has-error');
    $('#from-validation').removeClass('glyphicon-remove');
    $('#to-container').removeClass('has-error');
    $('#to-validation').removeClass('glyphicon-remove');
    $('#travelErrContainer').addClass('hidden');
}

function ClearTravelSecondTab() {
    BackTravelTab();
    $('#travelProgress').addClass('hidden');
    $('.generated-schedule-row').remove();
}