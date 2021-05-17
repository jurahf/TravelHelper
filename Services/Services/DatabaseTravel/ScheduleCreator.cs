using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreImplementation.Args;
using CoreImplementation.Model;
using CoreImplementation.Results;
using CoreImplementation.ServiceInterfaces;
using Services.ModelsTools;
using Services.ParsedArgs;
using TravelHelperDb;

namespace Services.Services.DatabaseTravel
{
    public class ScheduleCreator
    {
        private readonly TravelHelperDatabaseContext data;
        private readonly IAddressesService addressService;
        private readonly CommonParseHelper parseHelper;

        private const int placeInDayCount = 5;    // сколько мест мы хотим посещать в день
        private const int filterCoefficient = 5;  // 1 / доля мест, которые подойдут по категориям
        private const int defaultLimit = 1000;    // максимум мест по-умолчанию
        private const decimal radius = 100m;      // радиус поиска в километрах
        private const int activeHourseCount = 8;  // сколько часов в сутках считаем активными



        public ScheduleCreator(TravelHelperDatabaseContext data, IAddressesService addressesService)
        {
            this.data = data;
            this.addressService = addressesService;

            parseHelper = new CommonParseHelper(data);
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
        public List<ScheduleSet> CreateShedules(PreSaveTravelParsedArgs parsed)
        {
            // TODO: слишком большой метод

            if (parsed?.Result?.Valid != true)
                return new List<ScheduleSet>();

            // считаем длину географических градусов для данной широты
            double lat = (double)parsed.City.Lat * Math.PI / 180;
            decimal radiusLat = radius / (decimal)(111.321 * Math.Cos(lat) - 0.094 * Math.Cos(3 * lat));
            decimal radiusLng = radius / (decimal)(111.143 - 0.562 * Math.Cos(2 * lat));

            int daysCount = (parsed.EndDate - parsed.StartDate).Days + 1;


            List<VMAddressInfo> addrInfoList = addressService
                .SearchNear(new AddressSearchNearArgs()
                {
                    Lat = parsed.City.Lat,
                    Lng = parsed.City.Lng,
                    RadiusLat = radiusLat,
                    RadiusLng = radiusLng,
                    Categories = GetCategories(parsed).Select(x => x.ConvertToVm()).ToList(),     // тут добавляются точки питания
                    Limit = Math.Max(daysCount * placeInDayCount * filterCoefficient, defaultLimit),
                    NeededPlacesCount = daysCount * placeInDayCount,  // сколько мест надо выбрать
                    City = parsed.City.ConvertToVm()
                });


            // мест может быть найдено меньше, поэтому перевычисляем
            int neededPlacesCount = addrInfoList.Count();
            int realPlaceInDayCount = neededPlacesCount / daysCount;
            int hoursOnPlace = Math.Max(1, activeHourseCount / realPlaceInDayCount);

            List<ScheduleSet> scheduleList = new List<ScheduleSet>();
            for (DateTime tempDate = parsed.StartDate.Date; tempDate <= parsed.EndDate.Date; tempDate = tempDate.AddDays(1))
            {
                ScheduleSet schedule = new ScheduleSet()
                {
                    Date = tempDate,
                    TempPoint = 0,
                    PlacePointSet = new List<PlacePointSet>(),
                    Travel = parsed.Travel
                };

                int tempHour = 9;
                int tempOrder = 0;

                if (tempDate == parsed.StartDate.Date)  // первый день - сначала в гостиницу
                {
                    tempHour = 10;  // или когда мы там заселяемся?

                    schedule.PlacePointSet.Add(GetNonSavedHotel(schedule, tempDate, tempHour, tempOrder++));
                    tempHour += hoursOnPlace;
                }

                var basePlace = addrInfoList   // опорная точка на сегодня
                    //.Where(x => x.Category !=     // может быть стоит исключить места питания?
                    .OrderBy(x => x.Latitude)
                    .FirstOrDefault();

                NaviAddressInfoSet basePlaceInDb = basePlace == null ? null : data.NaviAddressInfoSet.FirstOrDefault(x => x.Id == basePlace.Id);

                if (basePlaceInDb != null)
                {
                    schedule.PlacePointSet.Add(CreatePlacePoint(schedule, tempDate, tempHour, tempOrder++, basePlaceInDb));
                    tempHour += hoursOnPlace;
                    if (!addrInfoList.Any())
                        continue;
                    addrInfoList.Remove(basePlace);

                    addrInfoList = addrInfoList.OrderBy(x => CalcDistance(x, basePlace)).ToList();
                    for (int i = tempOrder; i <= realPlaceInDayCount; i++)
                    {
                        if (!addrInfoList.Any())
                            break;

                        NaviAddressInfoSet fromDb = data.NaviAddressInfoSet.FirstOrDefault(x => x.Id == addrInfoList[0].Id);

                        schedule.PlacePointSet.Add(CreatePlacePoint(schedule, tempDate, tempHour, tempOrder++, fromDb));
                        tempHour += hoursOnPlace;
                        addrInfoList.RemoveAt(0);
                    }

                    // TODO: предполагаем, когда что работает, добавляем точки питания
                }

                if (tempDate < parsed.EndDate.Date) // не последний день - возвращаемся в гостиницу
                {
                    schedule.PlacePointSet.Add(GetNonSavedHotel(schedule, tempDate, tempHour, tempOrder));
                }

                scheduleList.Add(schedule);
            }

            return scheduleList;
        }

        /// <summary>
        /// Собирает выбранные категории и точки питания
        /// </summary>
        private List<CategorySet> GetCategories(PreSaveTravelParsedArgs parsed)
        {
            // берем выбранные категории + точки питания
            List<CategorySet> selectedCategories = parsed.Categories.Select(c => c).ToList();
            List<CategorySet> defaultCategories = data.CategorySet.Where(x => x.Name.ToLower() == "кафе" || x.Name.ToLower() == "ресторан" || x.Name.ToLower() == "бар").ToList();
            var result = selectedCategories.Union(defaultCategories).Distinct().ToList();
            return result;
        }

        private PlacePointSet CreatePlacePoint(ScheduleSet schedule, DateTime tempDate, int hour, int order, NaviAddressInfoSet address)
        {
            return new PlacePointSet()
            {
                CustomName = address.Description,
                //Schedule = schedule,
                NaviAddressInfo = address,
                Order = order,
                Time = new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, hour, 00, 00)
            };
        }

        private PlacePointSet GetNonSavedHotel(ScheduleSet schedule, DateTime tempDate, int hour, int order)
        {
            return new PlacePointSet()
            {
                CustomName = "Гостиница",
                //Schedule = schedule,
                // NaviAddressInfo          // какая именно гостиница мы пока не знаем
                Order = order,
                Time = new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, hour, 00, 00)
            };
        }


        private double CalcDistance(NaviAddressInfoSet x, NaviAddressInfoSet y)
        {
            return Math.Sqrt(Math.Pow((double)(x.Latitude - y.Latitude), 2) + Math.Pow((double)(x.Longitude - y.Longitude), 2));
        }


        private double CalcDistance(VMAddressInfo x, VMAddressInfo y)
        {
            return Math.Sqrt(Math.Pow((double)(x.Latitude - y.Latitude), 2) + Math.Pow((double)(x.Longitude - y.Longitude), 2));
        }



    }
}