using Server.Classes.Args;
using Server.Classes.Dtos;
using Server.Classes.Results;
using Server.Controllers;
using Server.Interfaces;
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
            IAddressesService addressService = new ServiceFactory().GetAddressesService();
            parseHelper = new CommonParseHelper(data);
        }

        public SaveScheduleParsedArgs ValidateAndParse(SaveScheduleArgs saveArgs)
        {
            SaveScheduleParsedArgs parsed = new SaveScheduleParsedArgs();
            SaveScheduleResult validation = new SaveScheduleResult();
            StringBuilder message = new StringBuilder();

            parsed.User = parseHelper.ParseUser(saveArgs.UserLogin);
            parsed.ScheduleId = saveArgs.ScheduleId;
            parsed.Schedule = data.GetFromDatabase<Schedule>(x => x.Id == saveArgs.ScheduleId).FirstOrDefault();

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

        public void UpdateRows(SaveScheduleParsedArgs parsed, SaveScheduleArgs saveArgs)
        {
            // учитываются только перестановки, удаление и добавление не учитывается

            if (parsed.Schedule == null)
                throw new ArgumentException("Не найдено расписание");

            int order = 0;
            foreach (var updatedRow in saveArgs.ScheduleRows.OrderBy(x => x.DateTime))
            {
                if (updatedRow.PlacePointId == null)
                {
                    // TODO: содаем новую точку
                }
                else
                {
                    var oldRow = parsed.Schedule.PlacePoint.FirstOrDefault(x => x.Id == updatedRow.PlacePointId);

                    if (oldRow != null)
                    {
                        // меняем время
                        oldRow.Time = parseHelper.ParseDateFull(updatedRow.DateTime);
                        oldRow.Order = order;
                    }
                }

                order++;
            }

            // удаляем
            var forDel = parsed.Schedule.PlacePoint.Where(x => !saveArgs.ScheduleRows.Any(y => y.PlacePointId == x.Id)).ToList();
            for (int i = 0; i < forDel.Count(); i++)
                parsed.Schedule.PlacePoint.Remove(forDel[i]);
        }


    }








}