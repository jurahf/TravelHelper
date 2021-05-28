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
using System.Threading.Tasks;

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
                .Fetch()
                .FirstOrDefault(u => u.Login.ToLower() == user.Email.ToLower());
        }

        private NaviAddressInfoSet GetAddressInfoFromDb(VMAddressInfo addressInfo)
        {
            return data.NaviAddressInfoSet
                .FirstOrDefault(a => a.Id == addressInfo.Id);
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


        public VMSchedule UpdateSchedulePoints(int scheduleId, VMSchedule newSchedule)
        {
            ScheduleSet forUpdate = data.ScheduleSet
                .Fetch()
                .FirstOrDefault(x => x.Id == scheduleId);

            if (forUpdate == null)
                throw new ArgumentException("Расписание не найдено");

            data.PlacePointSet.RemoveRange(forUpdate.PlacePointSet);
            forUpdate.PlacePointSet.Clear();

            foreach (var newPlace in newSchedule.PlacePoint)
            {
                PlacePointSet placePoint = new PlacePointSet()
                {
                    CustomName = newPlace.CustomName,
                    NaviAddressInfo = GetAddressInfoFromDb(newPlace.AddressInfo) ?? FindByCoordsOrCreateNewAddress(newPlace.AddressInfo),
                    Order = newPlace.Order,
                    Schedule = forUpdate,
                    Time = newPlace.Time
                };

                forUpdate.PlacePointSet.Add(placePoint);
                data.PlacePointSet.Add(placePoint);
            }


            data.Update(forUpdate);
            data.SaveChanges();

            return forUpdate.ConvertToVm();
        }

        private NaviAddressInfoSet FindByCoordsOrCreateNewAddress(VMAddressInfo addressInfo)
        {
            if (addressInfo == null)
                return null;

            var findedAddress = data.NaviAddressInfoSet.FirstOrDefault(x => x.Latitude == addressInfo.Latitude && x.Longitude == addressInfo.Longitude); // TODO: искать в окрестности

            if (findedAddress != null)
                return findedAddress;
            else
            {
                var result = new NaviAddressInfoSet()
                {
                    //Category = addressInfo.Category,
                    // City
                    Description = addressInfo.Description,
                    Latitude = addressInfo.Latitude,
                    Longitude = addressInfo.Longitude,
                    Name = addressInfo.Name,
                    Picture = addressInfo.Picture
                };

                data.NaviAddressInfoSet.Add(result);

                return result;
            }
        }

        public async Task<List<VMCity>> SearchCitiesAsync(string queryString, int limit)
        {
            if (string.IsNullOrWhiteSpace(queryString))
                return await Task.FromResult(new List<VMCity>());

            return await data.CitySet
                .Fetch()
                .Where(x => x.Name.StartsWith(queryString))
                .OrderBy(x => x.Country == "Россия" ? 0 : 1)
                .Take(limit)
                .Select(x => x.ConvertToVm())
                .ToListAsync();
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



    }
}