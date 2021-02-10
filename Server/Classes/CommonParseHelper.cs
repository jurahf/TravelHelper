using Server.Classes.Dtos;
using Server.Controllers;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Server.Classes
{
    public class CommonParseHelper
    {
        private readonly DBWork data;
        private readonly NaviLoadHelper naviLoadHelper;
        private const string DateTimeFormatFull = "yyyy-MM-ddTHH:mm:ss";
        private const string DateTimeFormatShort = "dd.MM.yyyy";
        private const string TimeFormat = "HH:mm";

        public CommonParseHelper(DBWork data, NaviLoadHelper naviLoadHelper)
        {
            this.data = data;
            this.naviLoadHelper = naviLoadHelper;
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
            List<Category> existedCategories,
            Schedule sch)
        {
            int tempOrder = 0;
            foreach (var dto in dtoList.OrderBy(x => x.DateTime))
            {
                PlacePoint place = new PlacePoint();

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

                var addrInfo = new NaviAddressInfo();

                if (!string.IsNullOrEmpty(address))
                {
                    Regex regex = new Regex(@"\[(?<container>\d*)\](?<self>\d*)");
                    var regexGroups = regex.Match(address).Groups;

                    if (regexGroups.Count > 0)
                    {
                        addrInfo.ContainerAddress = regexGroups["container"].Value;
                        addrInfo.SelfAddress = regexGroups["self"].Value;

                        // TODO - загрузить все адреса заранее и параллельно
                        addrInfo = naviLoadHelper.LoadAdditionalInfoSingle(addrInfo);

                        // TODO: перенести в загрузчик или оставить только загрузку из БД (и убрать из параметров existedCategories)
                        if (addrInfo?.Category != null && addrInfo.Category.Id == 0)
                        {
                            if (existedCategories != null && existedCategories.Any())
                            {
                                addrInfo.Category = existedCategories.FirstOrDefault(x => x.NaviId == addrInfo.Category.NaviId);
                            }
                            else
                            {
                                addrInfo.Category = data.GetFromDatabase<Category>(x => x.NaviId == addrInfo.Category.NaviId).FirstOrDefault();
                            }
                        }
                    }
                }

                place.NaviAddressInfo = addrInfo;

                sch.PlacePoint.Add(place);
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

        public List<Category> ParseCategotyList(List<string> categories)
        {
            return data.GetFromDatabase<Category>(x => categories.Contains(x.NaviId));
        }

        public User ParseUser(string login)
        {
            return data.GetFromDatabase<User>(u => u.Login == login).FirstOrDefault();
        }

        public City ParseCity(string cityName)
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

            return data.GetFromDatabase<City>(x => x.Name == cityName && x.Country == countryName).FirstOrDefault();
        }


    }
}