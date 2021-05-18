using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CoreImplementation.Model;
using TravelHelperDb;

namespace Services.Services.DatabaseTravel
{
    public class DefaultDataCreator
    {
        private const string systemUserLogin = "TestUser";

        private readonly TravelHelperDatabaseContext data;

        public DefaultDataCreator(TravelHelperDatabaseContext data)
        {
            this.data = data;
        }


        public void CreateDataForUser(VMUser user)
        {
            UserSet dbUser = FindOrCreateUser(user);

            // если у него есть путешествие - непонятно. Можем ничего и не делать
            List<TravelSet> existedTravels = data.TravelSet.Where(x => x.User.Id == dbUser.Id).ToList();
            if (existedTravels.Any())
                return;

            // если у него нет путешествий, создаем Travel, TravelCategory, Schedule, PlacePoint, обновляем UserSettings
            TravelSet srcTravel = LoadSystemTravel();

            if (srcTravel == null)
                return;

            TravelSet travel = CreateTravel(srcTravel, dbUser);

            // надо, чтобы все сохранилось в базу
            data.TravelSet.Add(travel);
            data.SaveChanges();

            dbUser.UserSettings.SelectedTravelId = travel.Id;
            data.Update(dbUser.UserSettings);
            data.SaveChanges();
        }

        private TravelSet LoadSystemTravel()
        {
            return data.TravelSet
                .Include(x => x.City)
                .Include(x => x.TravelCategory)
                .ThenInclude(y => y.Categories)
                .Include(x => x.ScheduleSet)
                .ThenInclude(y => y.PlacePointSet)
                .ThenInclude(z => z.NaviAddressInfo)
                .FirstOrDefault(x => x.User.Login == systemUserLogin);
        }

        private TravelSet CreateTravel(TravelSet srcTravel, UserSet dbUser)
        {
            TravelSet travel = new TravelSet()
            {
                User = dbUser,
                City = srcTravel.City,
                StartDate = srcTravel.StartDate,
                EndDate = srcTravel.EndDate,
                Name = srcTravel.Name,
                TravelCategory = new List<TravelCategory>(),
                ScheduleSet = new List<ScheduleSet>()
            };

            CopyCategories(travel, srcTravel);
            CopySchedule(travel, srcTravel);            

            return travel;
        }

        private void CopyCategories(TravelSet travel, TravelSet srcTravel)
        {
            foreach (var srcTravelCategory in srcTravel.TravelCategory)
            {
                TravelCategory travelCategory = new TravelCategory()
                {
                    Categories = srcTravelCategory.Categories,
                    TravelCategoryCategory = travel
                };

                travel.TravelCategory.Add(travelCategory);
            }
        }

        private void CopySchedule(TravelSet travel, TravelSet srcTravel)
        {
            foreach (var srcSchedule in srcTravel.ScheduleSet)
            {
                ScheduleSet schedule = new ScheduleSet()
                {
                    Date = srcSchedule.Date,
                    TempPoint = srcSchedule.TempPoint,
                    Travel = travel,
                    PlacePointSet = new List<PlacePointSet>()
                };

                CopyPlacePoints(schedule, srcSchedule);

                travel.ScheduleSet.Add(schedule);
            }
        }

        private void CopyPlacePoints(ScheduleSet schedule, ScheduleSet srcSchedule)
        {
            foreach (var srcPlacePoint in srcSchedule.PlacePointSet)
            {
                PlacePointSet placePoint = new PlacePointSet()
                {
                    CustomName = srcPlacePoint.CustomName,
                    Order = srcPlacePoint.Order,
                    Schedule = schedule,
                    Time = srcPlacePoint.Time,
                    NaviAddressInfo = srcPlacePoint.NaviAddressInfo
                };

                schedule.PlacePointSet.Add(placePoint);
            }
        }

        private UserSet FindOrCreateUser(VMUser user)
        {
            // проверим наличие пользователя
            UserSet userFromDb = GetUserFromDb(user);

            // если его нет - создаем User и UserSettings
            if (userFromDb == null)
            {
                UserSettingsSet settings = new UserSettingsSet();
                userFromDb = new UserSet()
                {
                    Login = user.Email,
                    UserSettings = settings
                };

                data.UserSettingsSet.Add(settings);
                data.UserSet.Add(userFromDb);
                data.SaveChanges();
            }

            return userFromDb;
        }

        private UserSet GetUserFromDb(VMUser user)
        {
            return data.UserSet
                .Include(x => x.UserSettings)
                .FirstOrDefault(u => u.Login.ToLower() == user.Email.ToLower());
        }



    }
}
