using Server.Controllers;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Server.Classes
{
    public class ScheduleSaveHelper
    {
        private readonly DBWork data;
        private readonly CommonParseHelper parseHelper;

        public ScheduleSaveHelper(DBWork data)
        {
            this.data = data;
            // TODO: в фабрику
            var addressController = new AddressesController();
            var naviLoadHelper = new NaviLoadHelper(data, addressController);
            parseHelper = new CommonParseHelper(data, naviLoadHelper);
        }

        public SaveScheduleParsedArgs ValidateAndParse(SaveScheduleArgs saveArgs)
        {
            SaveScheduleParsedArgs parsed = new SaveScheduleParsedArgs();
            SaveScheduleResult validation = new SaveScheduleResult();
            StringBuilder message = new StringBuilder();

            parsed.User = parseHelper.ParseUser(saveArgs.UserLogin);
            parsed.ScheduleId = saveArgs.ScheduleId;

            parsed.Schedule = new Schedule();   // пока с БД не работаем - может еще откатывать надо будет
            parsed.Schedule.Date = saveArgs.ScheduleRows.Select(x => parseHelper.ParseDateFull(x.DateTime).Date).First();
            parsed.Schedule.PlacePoint = new List<PlacePoint>();
            parseHelper.ParsePlacePointList(saveArgs.ScheduleRows, Enumerable.Empty<PlaceDto>(), null, parsed.Schedule);
            //parsed.Schedule.Travel  // TODO: если новое расписание, то это надо знать
            
            validation.Valid = string.IsNullOrEmpty(message.ToString());
            validation.ErrorMessage = message.ToString();
            parsed.Result = validation;

            return parsed;
        }

        public void SaveSchedule(SaveScheduleParsedArgs parsed)
        {
            Schedule forSave = null;
            List<PlacePoint> forDelete = new List<PlacePoint>();
            if (parsed.ScheduleId == null || parsed.ScheduleId.Value == 0)
            {
                forSave = parsed.Schedule;
            }
            else
            {
                forSave = data.GetFromDatabase<Schedule>(x => x.Id == parsed.ScheduleId.Value).FirstOrDefault() ?? new Schedule();
                forDelete = new List<PlacePoint>(forSave.PlacePoint);

                forSave.Date = parsed.Schedule.Date;
                forSave.PlacePoint = new List<PlacePoint>(parsed.Schedule.PlacePoint);
                //forSave.Travel
                //forSave.TempPoint
            }

            data.Insert(forSave);
            data.DeleteObject(forDelete);

            parsed.Result.ScheduleId = forSave.Id;
        }
    }

    public class SaveScheduleArgs
    {
        public int? ScheduleId { get; set; }

        public string UserLogin { get; set; }

        public List<ScheduleDto> ScheduleRows { get; set; }
    }

    public class SaveScheduleParsedArgs
    {
        public int? ScheduleId { get; set; }

        public User User { get; set; }

        public Schedule Schedule { get; set; }

        public SaveScheduleResult Result { get; set; }
    }

    public class SaveScheduleResult
    {
        public bool Valid { get; set; }

        public string ErrorMessage { get; set; }

        public int? ScheduleId { get; set; }
    }


}