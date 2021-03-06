﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Implementation.Args;
using Implementation.Results;
using Implementation.ServiceInterfaces;
using Server.Models;
using Server.ParsedArgs;

namespace Server.Services.DatabaseTravel
{
    public class TravelCreator
    {
        private readonly DBWork data;
        private readonly IAddressesService addressService;
        private readonly CommonParseHelper parseHelper;

        public TravelCreator(DBWork data)
        {
            this.data = data;
            // TODO: в фабрику
            addressService = new ServiceFactory().GetAddressesService();
            parseHelper = new CommonParseHelper(data);
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

                parseHelper.ParsePlacePointList(group, saveArgs.AdditionalPlaces, parsed.Categories, sch, parsed.City);

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




    }


}