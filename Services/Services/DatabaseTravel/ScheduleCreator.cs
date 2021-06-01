using System;
using System.Collections.Generic;
using System.Linq;
using CoreImplementation.Args;
using CoreImplementation.Model;
using CoreImplementation.ServiceInterfaces;
using TravelHelperDb;

namespace Services.Services.DatabaseTravel
{
    public class ScheduleCreator
    {
        private readonly TravelHelperDatabaseContext data;
        private readonly IAddressesService addressService;

        private const int placeInDayCount = 5;    // сколько мест мы хотим посещать в день
        private const int filterCoefficient = 5;  // 1 / доля мест, которые подойдут по категориям
        private const int defaultLimit = 1000;    // максимум мест по-умолчанию
        private const decimal radius = 100m;      // радиус поиска в километрах
        private const int activeHourseCount = 8;  // сколько часов в сутках считаем активными



        public ScheduleCreator(TravelHelperDatabaseContext data, IAddressesService addressesService)
        {
            this.data = data;
            this.addressService = addressesService;
        }





        public List<ScheduleSet> CreateShedules(GenerateTravelArgs args, TravelSet travel)
        {
            // TODO: слишком большой метод

            if (args == null)
                return new List<ScheduleSet>();

            // считаем длину географических градусов для данной широты
            double lat = (double)args.City.Lat * Math.PI / 180;
            decimal radiusLat = radius / (decimal)(111.321 * Math.Cos(lat) - 0.094 * Math.Cos(3 * lat));
            decimal radiusLng = radius / (decimal)(111.143 - 0.562 * Math.Cos(2 * lat));

            int daysCount = (args.EndDate - args.StartDate).Days + 1;


            List<VMAddressInfo> addrInfoList = addressService
                .SearchNear(new AddressSearchNearArgs()
                {
                    Lat = args.City.Lat,
                    Lng = args.City.Lng,
                    RadiusLat = radiusLat,
                    RadiusLng = radiusLng,
                    Categories = args.Categories,     // TODO: добавлять тут точки питания
                    Limit = Math.Max(daysCount * placeInDayCount * filterCoefficient, defaultLimit),
                    NeededPlacesCount = daysCount * placeInDayCount,  // сколько мест надо выбрать
                    City = args.City
                });


            // мест может быть найдено меньше, поэтому перевычисляем
            int neededPlacesCount = addrInfoList.Count();
            int realPlaceInDayCount = neededPlacesCount / daysCount;
            int hoursOnPlace = Math.Max(1, activeHourseCount / realPlaceInDayCount);

            List<ScheduleSet> scheduleList = new List<ScheduleSet>();
            for (DateTime tempDate = args.StartDate.Date; tempDate <= args.EndDate.Date; tempDate = tempDate.AddDays(1))
            {
                ScheduleSet schedule = new ScheduleSet()
                {
                    Date = tempDate,
                    TempPoint = 0,
                    PlacePointSet = new List<PlacePointSet>(),
                    Travel = travel
                };

                int tempHour = 9;
                int tempOrder = 0;

                if (tempDate == args.StartDate.Date)  // первый день - сначала в гостиницу
                {
                    tempHour = 10;  // или когда мы там заселяемся?

                    // TODO: 
                    //schedule.PlacePointSet.Add(GetNonSavedHotel(schedule, tempDate, tempHour, tempOrder++));
                    tempHour += hoursOnPlace;
                }

                var basePlace = addrInfoList   // опорная точка на сегодня
                    //.Where(x => x.Category !=     // может быть стоит исключить места питания?
                    .OrderBy(x => x.Latitude)
                    .FirstOrDefault();

                NaviAddressInfoSet basePlaceInDb = FindOrCreateAddr(basePlace);

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

                        NaviAddressInfoSet fromDb = FindOrCreateAddr(addrInfoList[0]);

                        schedule.PlacePointSet.Add(CreatePlacePoint(schedule, tempDate, tempHour, tempOrder++, fromDb));
                        tempHour += hoursOnPlace;
                        addrInfoList.RemoveAt(0);
                    }

                    // TODO: предполагаем, когда что работает, добавляем точки питания
                }

                if (tempDate < args.EndDate.Date) // не последний день - возвращаемся в гостиницу
                {
                    // TODO:
                    // schedule.PlacePointSet.Add(GetNonSavedHotel(schedule, tempDate, tempHour, tempOrder));
                }

                data.ScheduleSet.Add(schedule);
                scheduleList.Add(schedule);
            }

            return scheduleList;
        }

        private NaviAddressInfoSet FindOrCreateAddr(VMAddressInfo basePlace)
        {
            var finded = data.NaviAddressInfoSet
                .Where(x => x.Latitude == basePlace.Latitude && x.Longitude == basePlace.Longitude)
                .FirstOrDefault();

            if (finded == null)
            {
                var created = new NaviAddressInfoSet()
                {
                    Category = data.CategorySet.FirstOrDefault(x => x.Id == basePlace.Category.Id),
                    Latitude = basePlace.Latitude,
                    Longitude = basePlace.Longitude,
                    Description = basePlace.Description,
                    SelfAddress = basePlace.SelfAddress
                };

                data.NaviAddressInfoSet.Add(created);

                return created;
            }
            else
            {
                return finded;
            }
        }

        private PlacePointSet CreatePlacePoint(ScheduleSet schedule, DateTime tempDate, int hour, int order, NaviAddressInfoSet address)
        {
            var placePoint = new PlacePointSet()
            {
                CustomName = address.Description,
                //Schedule = schedule,
                NaviAddressInfo = address,
                Order = order,
                Time = new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, hour, 00, 00)
            };

            data.PlacePointSet.Add(placePoint);

            return placePoint;
        }

        private PlacePointSet GetNonSavedHotel(ScheduleSet schedule, DateTime tempDate, int hour, int order)
        {
            var placePoint = new PlacePointSet()
            {
                CustomName = "Гостиница",
                //Schedule = schedule,
                // NaviAddressInfo          // какая именно гостиница мы пока не знаем
                Order = order,
                Time = new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, hour, 00, 00)
            };

            data.PlacePointSet.Add(placePoint);

            return placePoint;
        }


        private double CalcDistance(VMAddressInfo x, VMAddressInfo y)
        {
            return Math.Sqrt(Math.Pow((double)(x.Latitude - y.Latitude), 2) + Math.Pow((double)(x.Longitude - y.Longitude), 2));
        }



    }
}