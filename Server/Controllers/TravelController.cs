using Server.Classes;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Server.Controllers
{
    // TODO: адрес клиента из конфига
    [EnableCors(origins: "http://localhost:2168", headers: "*", methods: "*")]
    public class TravelController : ApiController
    {
        private DBWork data = new DBWork();


        [HttpGet]
        public List<Category> GetAllCategories()
        {
            var result = data.GetFromDatabase<Category>().ToList();
            return result;
        }


        [HttpGet]
        public List<Travel> GetTravelsList(string login)
        {            
            var result = data.GetFromDatabase<Travel>(t => t.User.Login.ToLower() == login.ToLower());
            return result;
        }

        [HttpGet]
        public int? GetSelectedTravel(string login)
        {
            var user = data.GetFromDatabase<User>(u => u.Login.ToLower() == login.ToLower()).FirstOrDefault();
            return user?.UserSettings?.SelectedTravelId;
        }

        [HttpGet]
        public int SelectTravel(string login, int id)
        {
            var user = data.GetFromDatabase<User>(u => u.Login.ToLower() == login.ToLower()).FirstOrDefault();

            if (user == null)
                throw new ArgumentException("Пользователь не найден");
            else
            {
                if (user.UserSettings == null)
                    user.UserSettings = new UserSettings();
                user.UserSettings.SelectedTravelId = id;

                data.Insert(user.UserSettings);
            }

            return user.UserSettings.SelectedTravelId.Value;
        }

        [HttpPut]
        public PreSaveTravelResult PreSaveTravel([FromBody] PreSaveTravelArgs preSaveArgs)
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
                TravelCreateHelper helper = new TravelCreateHelper(data);
                var parsed = helper.ValidateAndParse(preSaveArgs);

                if (parsed.Result.Valid)
                {
                    helper.CreateShedules(parsed);
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

        [HttpPut]
        public SaveTravelResult SaveTravel([FromBody] SaveTravelArgs saveArgs)
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
                TravelCreateHelper helper = new TravelCreateHelper(data);
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

        [HttpPut]
        public SaveScheduleResult SaveSchedule([FromBody] SaveScheduleArgs saveArgs)
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
                    helper.SaveSchedule(parsed);
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
