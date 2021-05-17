using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Implementation.Args;
using Implementation.Model;
using Implementation.Results;

namespace Implementation.ServiceInterfaces
{
    /// <summary>
    /// Интерфейс сервиса для работы с путешествиями и расписаниями пользователя
    /// </summary>
    public interface ITravelService
    {
        List<VMCategory> GetAllCategories();

        List<VMTravel> GetTravelsList(VMUser user);

        int? GetSelectedTravel(VMUser user);

        int SelectTravel(VMUser user, int id);

        /// <summary>
        /// Создаются сущности, но не сохраняются. Здесь и генерируется расписание, а потом предлагается его отредактировать и сохранить
        /// </summary>
        PreSaveTravelResult PreSaveTravel(PreSaveTravelArgs preSaveArgs);

        /// <summary>
        /// Сущности должны сохраниться
        /// </summary>
        SaveTravelResult SaveTravel(SaveTravelArgs saveArgs);

        SaveScheduleResult SaveSchedule(SaveScheduleArgs saveArgs);
    }
}