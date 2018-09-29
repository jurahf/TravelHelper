using Server.Controllers;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Server.Classes
{
    public class TravelCreateHelper
    {
        private readonly DBWork data;
        private readonly AddressesController addressController;
        private readonly CommonParseHelper parseHelper;
        private readonly NaviLoadHelper naviLoadHelper;

        public TravelCreateHelper(DBWork data)
        {
            this.data = data;
            // TODO: в фабрику
            addressController = new AddressesController();
            naviLoadHelper = new NaviLoadHelper(data, addressController);
            parseHelper = new CommonParseHelper(data, naviLoadHelper);
        }

        public SaveTravelParsedArgs ValidateAndParse(SaveTravelArgs saveArgs)
        {
            SaveTravelParsedArgs parsed = new SaveTravelParsedArgs();
            SaveTravelResult validation = new SaveTravelResult();
            StringBuilder message = new StringBuilder();

            parsed.Categories = parseHelper.ParseCategotyList(saveArgs.Categories);
            parsed.City = parseHelper.ParseCity(saveArgs.City);
            parsed.StartDate = parseHelper.ParseDateShort(saveArgs.StartDate);
            parsed.EndDate = parseHelper.ParseDateShort(saveArgs.EndDate);
            parsed.User = parseHelper.ParseUser(saveArgs.UserLogin);
            parsed.TravelId = saveArgs.TravelId;


            parsed.Schedules = new List<Schedule>();

            foreach (var group in saveArgs.Schedules.GroupBy(x => parseHelper.ParseDateFull(x.DateTime).Date))
            {
                Schedule sch = new Schedule()
                {
                    PlacePoint = new List<PlacePoint>(),
                    Date = group.Key,
                    TempPoint = 0
                };

                parseHelper.ParsePlacePointList(group, saveArgs.AdditionalPlaces, parsed.Categories, sch);

                parsed.Schedules.Add(sch);
            }

            validation.Valid = string.IsNullOrEmpty(message.ToString());
            validation.ErrorMessage = message.ToString();
            parsed.Result = validation;

            return parsed;
        }



        public void SaveTravel(SaveTravelParsedArgs parsed)
        {
            Travel travel = LoadOCreateTravel(parsed.TravelId);

            travel.StartDate = parsed.StartDate;
            travel.EndDate = parsed.EndDate;
            travel.City = parsed.City;
            travel.Categories = new List<Category>(parsed.Categories);
            travel.User = parsed.User;
            travel.Name = $"{parsed.City.Name} с {parsed.StartDate:dd.MM.yyyy}";
            travel.Schedules = new List<Schedule>(parsed.Schedules);
            parsed.Schedules.ForEach(s => s.Travel = travel);

            // сохранить в базу
            data.Insert(travel);
            
            // созданное путешествие становится выбранным
            var user = travel.User;
            if (user.UserSettings == null)
                user.UserSettings = new UserSettings();
            user.UserSettings.SelectedTravelId = travel.Id;

            data.Insert(user.UserSettings);

            parsed.Result.TravelId = travel.Id;
        }

        private Travel LoadOCreateTravel(int? travelId)
        {
            if (travelId == null)
                return new Travel();
            else
                return data.GetFromDatabase<Travel>(x => x.Id == travelId.Value).FirstOrDefault() ?? new Travel();
        }

        public PreSaveTravelParsedArgs ValidateAndParse(PreSaveTravelArgs preSaveArgs)
        {
            // если есть TravelId - это редактирование существующего, если нет - создание нового
            PreSaveTravelParsedArgs parsed = new PreSaveTravelParsedArgs();
            PreSaveTravelResult validation = new PreSaveTravelResult();
            StringBuilder message = new StringBuilder();

            parsed.Categories = parseHelper.ParseCategotyList(preSaveArgs.Categories);

            parsed.City = parseHelper.ParseCity(preSaveArgs.City);
            validation.CityValid = true;
            if (parsed.City == null)
            {
                message.AppendLine("Город не найден");
                validation.CityValid = false;
            }

            parsed.StartDate = parseHelper.ParseDateShort(preSaveArgs.StartDate);
            parsed.EndDate = parseHelper.ParseDateShort(preSaveArgs.EndDate);
            
            validation.DatesValid = true;
            if (parsed.StartDate > parsed.EndDate)
            {
                message.AppendLine("Дата начала не может быть больше даты конца");
                validation.DatesValid = false;
            }

            if (parsed.StartDate < DateTime.Today)
            {
                message.AppendLine("Время начала путешествия уже прошло");
                validation.DatesValid = false;
            }

            validation.Valid = validation.CityValid && validation.DatesValid;

            validation.ErrorMessage = message.ToString();
            parsed.Result = validation;
            return parsed;
        }

        /// <summary>
        /// По разобранным аргументам создает расписания и сохраняет в поле Result (в БД не сохраняем)
        /// </summary>
        /// <param name="parsed"></param>
        public void CreateShedules(PreSaveTravelParsedArgs parsed)
        {
            // TODO: слишком большой метод

            List<string> defaultCategoriesId = new List<string>() { "4", "5", "6" }; // кафе, ресторан, бар
            int placeInDayCount = 5;    // сколько мест мы хотим посещать в день
            int filterCoefficient = 5;  // 1 / доля мест, которые подойдут по категориям
            int defaultLimit = 1000;    // максимум мест по-умолчанию
            decimal radius = 100m;      // радиус поиска в километрах
            int activeHourseCount = 8;  // сколько часов в сутках считаем активными

            if (parsed?.Result?.Valid != true)
                return;

            // считаем длину географических градусов для данной широты
            double lat = (double)parsed.City.Lat * Math.PI / 180;
            decimal radiusLat = radius / (decimal)(111.321 * Math.Cos(lat) - 0.094 * Math.Cos(3 * lat));
            decimal radiusLng = radius / (decimal)(111.143 - 0.562 * Math.Cos(2 * lat));

            int daysCount = (parsed.EndDate - parsed.StartDate).Days + 1;            

            List<NaviAddressInfo> addrInfoList = addressController
                .SearchNear(new AddressesController.AddressSearchNearArgs()
                {
                    Lat = parsed.City.Lat,
                    Lng = parsed.City.Lng,
                    RadiusLat = radiusLat,
                    RadiusLng = radiusLng,
                    Limit = Math.Max(daysCount * placeInDayCount * filterCoefficient, defaultLimit) // мы еще по категориям отсеим. Но вообще, должно зависеть от количества дней
                });

            List<NaviAddressInfo> filteredAddresses = new List<NaviAddressInfo>(addrInfoList);
            // берем выбранные категории + точки питания
            var catIdList = parsed.Categories.Select(c => c.NaviId);
            catIdList = catIdList.Union(defaultCategoriesId).Distinct();      // добавляем точки питания. TODO: Возможно, это надо делать позже
            filteredAddresses = addrInfoList.Where(x => x.Category != null && catIdList.ToList().Contains(x.Category.NaviId)).ToList();

            int neededPlacesCount = daysCount * placeInDayCount;  // сколько мест надо выбрать
            filteredAddresses = filteredAddresses.OrderBy(x => CalcDistance(x, parsed.City)).Take(neededPlacesCount).ToList();            
            if (filteredAddresses.Count() < neededPlacesCount) // TODO: на случай, если мест не хватило - добавляем из других категорий ???
                filteredAddresses = filteredAddresses.Concat(addrInfoList).Take(neededPlacesCount).ToList();

            // получаем дополнительную информацию по адресам
            filteredAddresses = naviLoadHelper.LoadAdditionalInfoParallel(filteredAddresses);

            // мест может быть найдено меньше, поэтому перевычисляем
            neededPlacesCount = filteredAddresses.Count();
            placeInDayCount = neededPlacesCount / daysCount;
            int hoursOnPlace = Math.Max(1, activeHourseCount / placeInDayCount);

            List<Schedule> scheduleList = new List<Schedule>();
            for (DateTime tempDate = parsed.StartDate.Date; tempDate <= parsed.EndDate.Date; tempDate = tempDate.AddDays(1))
            {
                Schedule schedule = new Schedule()
                {
                    Date = tempDate,
                    TempPoint = 0,
                    PlacePoint = new List<PlacePoint>(),
                    Travel = parsed.Travel
                };

                int tempHour = 9;
                int tempOrder = 0;

                if (tempDate == parsed.StartDate.Date)  // первый день - сначала в гостиницу
                {
                    tempHour = 10;  // или когда мы там заселяемся?
                    
                    schedule.PlacePoint.Add(GetNonSavedHotel(schedule, tempDate, tempHour, tempOrder++));
                    tempHour += hoursOnPlace;
                }

                var basePlace = filteredAddresses   // опорная точка на сегодня
                    //.Where(x => x.Category !=     // может быть стоит исключить места питания?
                    .OrderBy(x => x.Latitude)
                    .FirstOrDefault();

                if (basePlace != null)
                {
                    schedule.PlacePoint.Add(CreatePlacePoint(schedule, tempDate, tempHour, tempOrder++, basePlace));
                    tempHour += hoursOnPlace;
                    if (!filteredAddresses.Any())
                        continue;
                    filteredAddresses.Remove(basePlace);

                    filteredAddresses = filteredAddresses.OrderBy(x => CalcDistance(x, basePlace)).ToList();
                    for (int i = tempOrder; i <= placeInDayCount; i++)
                    {
                        if (!filteredAddresses.Any())
                            break;
                        schedule.PlacePoint.Add(CreatePlacePoint(schedule, tempDate, tempHour, tempOrder++, filteredAddresses[0]));
                        tempHour += hoursOnPlace;
                        filteredAddresses.RemoveAt(0);
                    }

                    // TODO: предполагаем, когда что работает, добавляем точки питания
                }

                if (tempDate < parsed.EndDate.Date) // не последний день - возвращаемся в гостиницу
                {
                    schedule.PlacePoint.Add(GetNonSavedHotel(schedule, tempDate, tempHour, tempOrder));
                }

                scheduleList.Add(schedule);
            }

            parsed.Result.Schedules = new List<Schedule>(scheduleList);
        }

        private PlacePoint CreatePlacePoint(Schedule schedule, DateTime tempDate, int hour, int order, NaviAddressInfo address)
        {
            return new PlacePoint()
            {
                CustomName = address.Name,
                //Schedule = schedule,
                NaviAddressInfo = address,
                Order = order,
                Time = new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, hour, 00, 00)
            };
        }

        private PlacePoint GetNonSavedHotel(Schedule schedule, DateTime tempDate, int hour, int order)
        {
            return new PlacePoint()
            {
                CustomName = "Гостиница",
                //Schedule = schedule,
                // NaviAddressInfo          // какая именно гостиница мы пока не знаем
                Order = order,
                Time = new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, hour, 00, 00)
            };
        }

        private double CalcDistance(NaviAddressInfo x, City y)
        {
            return Math.Sqrt(Math.Pow((double)(x.Latitude - y.Lat), 2) + Math.Pow((double)(x.Longitude - y.Lng), 2));
        }

        private double CalcDistance(NaviAddressInfo x, NaviAddressInfo y)
        {
            return Math.Sqrt(Math.Pow((double)(x.Latitude - y.Latitude), 2) + Math.Pow((double)(x.Longitude - y.Longitude), 2));
        }

    }

    public class SaveTravelArgs
    {
        public int? TravelId { get; set; }
        public string UserLogin { get; set; }
        public string City { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<string> Categories { get; set; }
        public List<ScheduleDto> Schedules { get; set; }
        public List<PlaceDto> AdditionalPlaces { get; set; }
    }

    public class SaveTravelParsedArgs
    {
        public int? TravelId { get; set; }
        public User User { get; set; }
        public City City { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Category> Categories { get; set; }
        public List<Schedule> Schedules { get; set; }

        public SaveTravelResult Result { get; set; }
    }

    public class ScheduleDto
    {
        public string DateTime { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
    }

    public class PlaceDto
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }


    public class PreSaveTravelArgs
    {
        public int? TravelId { get; set; }
        public string UserLogin { get; set; }
        public string City { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<string> Categories { get; set; }
    }

    public class PreSaveTravelParsedArgs
    {
        public Travel Travel { get; set; }
        public User User { get; set; }
        public City City { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Category> Categories { get; set; }

        public PreSaveTravelResult Result { get; set; }
    }

    public class PreSaveTravelResult
    {
        public bool Valid { get; set; }

        public bool CityValid { get; set; }
        public bool DatesValid { get; set; }

        public string ErrorMessage { get; set; }

        public List<Schedule> Schedules { get; set; }
    }

    public class SaveTravelResult
    {
        public bool Valid { get; set; }

        public string ErrorMessage { get; set; }

        public int? TravelId { get; set; }
    }

}