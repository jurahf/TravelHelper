using Server.Classes;
using Server.Classes.Args;
using Server.Classes.Results;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для работы с путешествиями и расписаниями пользователя
    /// </summary>
    public interface ITravelService
    {
        List<Category> GetAllCategories();

        List<Travel> GetTravelsList(string login);

        int? GetSelectedTravelId(string login);

        int SelectTravel(string login, int id);

        PreSaveTravelResult PreSaveTravel(PreSaveTravelArgs preSaveArgs);

        SaveTravelResult SaveTravel(SaveTravelArgs saveArgs);

        SaveScheduleResult SaveSchedule(SaveScheduleArgs saveArgs);
    }
}