using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using CoreImplementation.Args;
using CoreImplementation.Results;
using CoreImplementation.ServiceInterfaces;
using Services.ParsedArgs;
using TravelHelperDb;

namespace Services.Services.DatabaseTravel
{
    public class TravelCreator
    {
        private readonly TravelHelperDatabaseContext data;
        private readonly CommonParseHelper parseHelper;

        public TravelCreator(TravelHelperDatabaseContext data)
        {
            this.data = data;

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


            parsed.Schedules = new List<ScheduleSet>();

            foreach (var group in saveArgs.Schedules.GroupBy(x => parseHelper.ParseDateFull(x.DateTime).Date))
            {
                ScheduleSet sch = new ScheduleSet()
                {
                    PlacePointSet = new List<PlacePointSet>(),
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
            TravelSet travel = LoadOCreateTravel(parsed.TravelId);

            travel.StartDate = parsed.StartDate;
            travel.EndDate = parsed.EndDate;
            travel.City = parsed.City;
            travel.TravelCategory = new List<CategorySet>(parsed.Categories).Select(x => new TravelCategory() { Categories = x }).ToList();
            travel.User = parsed.User;
            travel.Name = $"{parsed.City.Name} с {parsed.StartDate:dd.MM.yyyy}";
            travel.ScheduleSet = new List<ScheduleSet>(parsed.Schedules);
            parsed.Schedules.ForEach(s => s.Travel = travel);

            // сохранить в базу
            data.TravelSet.Add(travel);
            
            // созданное путешествие становится выбранным
            var user = travel.User;
            if (user.UserSettings == null)
                user.UserSettings = new UserSettingsSet();
            user.UserSettings.SelectedTravelId = travel.Id;

            data.UserSettingsSet.Add(user.UserSettings);

            parsed.Result.TravelId = travel.Id;
        }

        private TravelSet LoadOCreateTravel(int? travelId)
        {
            if (travelId == null)
                return new TravelSet();
            else
                return data.TravelSet.FirstOrDefault(x => x.Id == travelId.Value) ?? new TravelSet();
        }




    }


}