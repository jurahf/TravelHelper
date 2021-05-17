using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using CoreImplementation.Dtos;
using TravelHelperDb;

namespace Services.Services.DatabaseTravel
{
    public class CommonParseHelper
    {
        private readonly TravelHelperDatabaseContext data;
        private const string DateTimeFormatFull = "yyyy-MM-ddTHH:mm:ss";
        private const string DateTimeFormatShort = "dd.MM.yyyy";
        private const string TimeFormat = "HH:mm";

        public CommonParseHelper(TravelHelperDatabaseContext data)
        {
            this.data = data;
        }


        /// <summary>
        /// Разбирает информацию о местах и добавляет в расписание
        /// </summary>
        /// <param name="dtoList"></param>
        /// <param name="additionalPlaces"></param>
        /// <param name="existedCategories"></param>
        /// <param name="sch"></param>
        /// <param name="dateTimeFormat"></param>
        public void ParsePlacePointList(
            IEnumerable<ScheduleDto> dtoList,
            IEnumerable<PlaceDto> additionalPlaces,
            List<CategorySet> existedCategories,
            ScheduleSet sch,
            CitySet city)
        {
            int tempOrder = 0;


            foreach (var dto in dtoList.OrderBy(x => x.DateTime))
            {
                PlacePointSet place = new PlacePointSet();

                place.Time = ParseDateFull(dto.DateTime);
                place.Order = tempOrder++;

                place.CustomName = dto.Name;

                string address = "";

                if (!string.IsNullOrEmpty(dto.Address) || dto.Address == "[]")
                    address = dto.Address;
                if (string.IsNullOrEmpty(address))
                {
                    address = additionalPlaces.FirstOrDefault(x => x.Name.ToLower() == dto.Name.ToLower())?.Value;
                    // если и там нет - поискать просто по имени ?
                }

                // TODO: тут начинаются всякие дикие вещи

                NaviAddressInfoSet addrInfo = new NaviAddressInfoSet();

                if (dto.AddressInfo != null)
                {
                    addrInfo = new NaviAddressInfoSet()
                    {
                        Description = dto.AddressInfo.Description,
                        ContainerAddress = dto.AddressInfo.ContainerAddress,
                        SelfAddress = dto.AddressInfo.SelfAddress,
                        Latitude = dto.AddressInfo.Latitude,
                        Longitude = dto.AddressInfo.Longitude,
                        Picture = dto.AddressInfo.Picture,
                        Category = existedCategories.FirstOrDefault(x => x.Id == dto.AddressInfo.Category?.Id),
                        City = city
                    };
                }
                else if (!string.IsNullOrEmpty(address))
                {
                    Regex regex = new Regex(@"\[(?<container>\d*)\](?<self>\d*)");
                    var regexGroups = regex.Match(address).Groups;

                    if (regexGroups.Count > 0)
                    {
                        addrInfo = new NaviAddressInfoSet();
                        addrInfo.ContainerAddress = regexGroups["container"].Value;
                        addrInfo.SelfAddress = regexGroups["self"].Value;

                        // TODO - загрузить все адреса заранее и параллельно
                        //addrInfo = naviLoadHelper.LoadAdditionalInfoSingle(addrInfo);

                        // TODO: перенести в загрузчик или оставить только загрузку из БД (и убрать из параметров existedCategories)
                        if (addrInfo?.Category != null && addrInfo.Category.Id == 0)
                        {
                            if (existedCategories != null && existedCategories.Any())
                            {
                                addrInfo.Category = existedCategories.FirstOrDefault(x => x.NaviId == addrInfo.Category.NaviId);
                            }
                            else
                            {
                                addrInfo.Category = data.CategorySet.Where(x => x.NaviId == addrInfo.Category.NaviId).FirstOrDefault();
                            }
                        }
                    }
                }

                place.NaviAddressInfo = addrInfo;

                sch.PlacePointSet.Add(place);
            }
        }

        public DateTime ParseDateFull(string dateString)
        {
            return DateTime.ParseExact(dateString, DateTimeFormatFull, CultureInfo.CurrentCulture);
        }

        public DateTime ParseDateShort(string dateString)
        {
            return DateTime.ParseExact(dateString, DateTimeFormatShort, CultureInfo.CurrentCulture);
        }

        public List<CategorySet> ParseCategotyList(List<string> categories)
        {
            return data.CategorySet.Where(x => categories.Contains(x.NaviId)).ToList();
        }

        public UserSet ParseUser(string login)
        {
            return data.UserSet.Where(u => u.Login == login).FirstOrDefault();
        }

        public CitySet ParseCity(string cityName)
        {
            string countryName = "";

            if (cityName.Contains('('))
            {
                Regex regex = new Regex(@"(?<city>.*)\s?\((?<country>.*)\)");
                var groups = regex.Match(cityName).Groups;
                if (groups.Count > 0)
                {
                    cityName = groups["city"].Value.Trim();
                    countryName = groups["country"].Value.Trim();
                }
            }

            return data.CitySet.Where(x => x.Name == cityName && x.Country == countryName).FirstOrDefault();
        }


    }
}