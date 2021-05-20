using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TravelHelperDb;
using CoreImplementation.Args;
using CoreImplementation.Results;
using CoreImplementation.ServiceInterfaces;
using CoreImplementation.Model;
using Services.ModelsTools;

namespace Services.Services.DatabaseTravel
{
    public class DatabaseTravelService : ITravelService
    {
        private readonly TravelHelperDatabaseContext data;
        private readonly IAddressesService addressesService;

        public DatabaseTravelService(TravelHelperDatabaseContext data, IAddressesService addressesService)
        {
            this.data = data;
            this.addressesService = addressesService;
        }

        private UserSet GetUserFromDb(VMUser user)
        {
            return data.UserSet
                .Include(x => x.UserSettings)
                .FirstOrDefault(u => u.Login.ToLower() == user.Email.ToLower());
        }

        public List<VMCategory> GetAllCategories()
        {
            List<CategorySet> fromDb = data.CategorySet.ToList();

            List<VMCategory> result = fromDb.Select(x => x.ConvertToVm()).ToList();

            foreach (var cat in result)
            {
                if (cat.Parent != null)
                {
                    var parent = result
                        .FirstOrDefault(x => x.Id == cat.Parent.Id);

                    if (parent != null)
                    {
                        if (parent.Childs == null)
                            parent.Childs = new List<VMCategory>();
                        parent.Childs.Add(cat);
                    }
                }
            }

            return result;
        }

        public List<VMTravel> GetTravelsList(VMUser user)
        {
            List<TravelSet> result = data.TravelSet
                .Fetch()
                .Where(t => t.User.Login.ToLower() == user.Email.ToLower()).ToList();

            return result.Select(x => x.ConvertToVm()).ToList();
        }

        public int? GetSelectedTravelId(VMUser user)
        {
            var userFromDb = GetUserFromDb(user);
            return userFromDb?.UserSettings?.SelectedTravelId;
        }


        public void CreateDefaultDataForUser(VMUser user)
        {
            DefaultDataCreator creator = new DefaultDataCreator(data);
            creator.CreateDataForUser(user);
        }


        public VMTravel MoveToNextPoint(VMTravel travel)
        {
            var dbTravel = data.TravelSet
                .Fetch()
                .FirstOrDefault(x => x.Id == travel.Id);

            if (dbTravel == null)
                throw new ArgumentException("Путешествие не найдено");

            var dbSchedule = dbTravel.ScheduleSet.FirstOrDefault(x => x.Date == dbTravel.CurrentDate);

            if (dbSchedule != null)
            {
                dbSchedule.TempPoint++;

                if (dbSchedule.TempPoint >= dbSchedule.PlacePointSet.Count)
                    dbSchedule.TempPoint = 0;

                data.Update(dbSchedule);
                data.SaveChanges();
            }

            return dbTravel.ConvertToVm();
        }


        public VMTravel MoveToDate(VMTravel travel, DateTime date)
        {
            var dbTravel = data.TravelSet
                .Fetch()
                .FirstOrDefault(x => x.Id == travel.Id);

            if (dbTravel == null)
                throw new ArgumentException("Путешествие не найдено");

            dbTravel.CurrentDate = date;

            data.Update(dbTravel);
            data.SaveChanges();

            return dbTravel.ConvertToVm();
        }


        public VMSchedule GetSchedule(int id)
        {
            return data.ScheduleSet
                .Fetch()
                .FirstOrDefault(x => x.Id == id)
                .ConvertToVm();
        }


















        public int SelectTravel(VMUser user, int id)
        {
            var userFromDb = GetUserFromDb(user);

            if (userFromDb == null)
                throw new ArgumentException("Пользователь не найден");
            else
            {
                if (userFromDb.UserSettings == null)
                    userFromDb.UserSettings = new UserSettingsSet();
                userFromDb.UserSettings.SelectedTravelId = id;

                data.UserSettingsSet.Add(userFromDb.UserSettings);
            }

            return userFromDb.UserSettings.SelectedTravelId.Value;
        }


        public PreSaveTravelResult PreSaveTravel(PreSaveTravelArgs preSaveArgs)
        {
            PreSaveTravelResult result = new PreSaveTravelResult();

            if (string.IsNullOrEmpty(preSaveArgs?.UserLogin))
            {
                result.Valid = false;
                result.ErrorMessage = "Данные отсутствуют.";
                return result;
            }

            try
            {
                ScheduleCreator helper = new ScheduleCreator(data, addressesService);
                var parsed = helper.ValidateAndParse(preSaveArgs);

                if (parsed.Result.Valid)
                {
                    parsed.Result.Schedules = helper.CreateShedules(parsed).Select(x => x.ConvertToVm()).ToList();
                }

                return parsed.Result;
            }
            catch (Exception ex)
            {
                result.Valid = false;
                result.ErrorMessage = "Ошибка при обработке запроса. Попробуйте повторить попытку позже.";
                return result;
            }
        }

        public SaveTravelResult SaveTravel(SaveTravelArgs saveArgs)
        {
            SaveTravelResult result = new SaveTravelResult();

            if (string.IsNullOrEmpty(saveArgs?.UserLogin))
            {
                result.Valid = false;
                result.ErrorMessage = "Данные отсутствуют.";
                return result;
            }

            try
            {
                TravelFromArgsCreator helper = new TravelFromArgsCreator(data);
                var parsed = helper.ValidateAndParse(saveArgs);

                if (parsed.Result.Valid)
                {
                    helper.SaveTravel(parsed);
                }

                return parsed.Result;
            }
            catch (Exception ex)
            {
                result.Valid = false;
                result.ErrorMessage = "Ошибка при обработке запроса. Попробуйте повторить попытку позже.";
                return result;
            }
        }

        public SaveScheduleResult SaveSchedule(SaveScheduleArgs saveArgs)
        {
            SaveScheduleResult result = new SaveScheduleResult();

            if (string.IsNullOrEmpty(saveArgs?.UserLogin))
            {
                result.Valid = false;
                result.ErrorMessage = "Данные отсутствуют.";
                return result;
            }

            try
            {
                ScheduleSaveHelper helper = new ScheduleSaveHelper(data);
                var parsed = helper.ValidateAndParse(saveArgs);

                if (parsed.Result.Valid)
                {
                    helper.UpdateRows(parsed, saveArgs);
                    data.Update(parsed.Schedule);
                }

                return parsed.Result;
            }
            catch (Exception ex)
            {
                result.Valid = false;
                result.ErrorMessage = "Ошибка при обработке запроса. Попробуйте повторить попытку позже.";
                return result;
            }
        }


    }
}