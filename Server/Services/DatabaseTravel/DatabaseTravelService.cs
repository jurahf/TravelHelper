using System;
using System.Collections.Generic;
using System.Linq;
using Server.Models;
using Implementation.Args;
using Implementation.Results;
using Implementation.ServiceInterfaces;
using Implementation.Model;

namespace Server.Services.DatabaseTravel
{
    public class DatabaseTravelService : ITravelService
    {
        private DBWork data = new DBWork();

        public List<VMCategory> GetAllCategories()
        {
            List<Category> fromDb = data.GetFromDatabase<Category>();

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
            List<Travel> result = data.GetFromDatabase<Travel>(t => t.User.Login.ToLower() == user.Login.ToLower());
            return result.Select(x => x.ConvertToVm()).ToList();
        }

        public int? GetSelectedTravelId(VMUser user)
        {
            var userFromDb = data.GetFromDatabase<User>(u => u.Login.ToLower() == user.Login.ToLower()).FirstOrDefault();
            return userFromDb?.UserSettings?.SelectedTravelId;
        }

        public int SelectTravel(VMUser user, int id)
        {
            var userFromDb = data.GetFromDatabase<User>(u => u.Login.ToLower() == user.Login.ToLower()).FirstOrDefault();

            if (userFromDb == null)
                throw new ArgumentException("Пользователь не найден");
            else
            {
                if (userFromDb.UserSettings == null)
                    userFromDb.UserSettings = new UserSettings();
                userFromDb.UserSettings.SelectedTravelId = id;

                data.Insert(userFromDb.UserSettings);
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
                ScheduleCreator helper = new ScheduleCreator(data);
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
                TravelCreator helper = new TravelCreator(data);
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