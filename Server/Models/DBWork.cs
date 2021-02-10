using Server.Classes.Args;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using static Server.Controllers.AddressesController;

namespace Server.Models
{
    public class DBWork
    {
        private const string idFieldName = "Id";
        private static NaviTravelModelContainer database = null;

        private static NaviTravelModelContainer Database
        {
            get
            {
                if (database == null)
                {
                    database = new NaviTravelModelContainer();
                }

                return database;
            }
        }

        private int GetId<T>(T obj) where T : class
        {
            return (int)typeof(T).GetProperty(idFieldName).GetValue(obj, null);
        }


        public List<T> GetFromDatabase<T>() where T : class
        {
            try
            {
                return Database.Set<T>().ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<T> GetFromDatabase<T>(Func<T, bool> filter) where T : class
        {
            try
            {
                return Database.Set<T>().Where(filter).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private const string Russia = "РОССИЯ";         // TODO: определять страну, или давать выбрать    
        private const decimal MoscowLat = 55.755826m;
        private const decimal MoscowLng = 37.6173m;

        public List<City> GetCities(CityFilterAndOrder args)
        {
            int limit = args.limit ?? 10;
            string queryStr = args.querystr?.ToLower();
            decimal lat = args.lat ?? MoscowLat;
            decimal lng = args.lng ?? MoscowLng;

            var result = Database.CitySet
                .Where(c => c.Name.ToLower().StartsWith(queryStr))
                .ToList();

            return result
                .OrderBy(x => x.Country.ToUpper() == Russia ? 0 : 1)
                .ThenBy(x => Math.Sqrt(Math.Pow((double)(x.Lat - lat), 2) + Math.Pow((double)(x.Lng - lng), 2)))
                .Take(limit)
                .ToList();
        }



        public void DeleteObject<T>(T obj) where T : class
        {
            try
            {
                Database.Set<T>().Remove(obj);

                Database.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void DeleteObject<T>(List<T> objList) where T : class
        {
            try
            {
                foreach (var obj in objList)
                {
                    Database.Set<T>().Remove(obj);
                }

                Database.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Update<T>(T obj) where T : class
        {
            try
            {
                Database.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Update<T>(List<T> objList) where T : class
        {
            Update(objList.FirstOrDefault());
        }

        public void Insert<T>(T obj) where T : class
        {
            try
            {
                // объект мог сохраниться с другими объектами, поэтому добавлять его не всегда надо
                if (GetFromDatabase<T>().FirstOrDefault(x => GetId(x) == GetId(obj)) == null) // костыль и долго, но зато ошибок с двойным сохранением точно не будет
                {
                    Database.Set<T>().Add(obj);
                }

                Database.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public void Insert<T>(List<T> objList) where T : class
        {
            try
            {
                foreach (var obj in objList)
                {
                    // объект мог сохраниться с другими объектами, поэтому добавлять его не всегда надо
                    if (GetFromDatabase<T>().FirstOrDefault(x => GetId(x) == GetId(obj)) == null) // костыль и долго, но зато ошибок с двойным сохранением точно не будет
                    {
                        Database.Set<T>().Add(obj);
                    }
                }

                Database.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }




    }
}