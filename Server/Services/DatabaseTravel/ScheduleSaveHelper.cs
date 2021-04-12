using Implementation.Args;
using Implementation.Dtos;
using Implementation.Results;
using Server.Controllers;
using Implementation.ServiceInterfaces;
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

        public void UpdateRows(SaveScheduleParsedArgs parsed, SaveScheduleArgs saveArgs)
        {
            // добавление не учитывается

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