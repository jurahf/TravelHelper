using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Server.Classes;
using Server.Classes.Args;
using Server.Interfaces;
using Server.Models;

namespace Server.Services
{
    public class MapBoxAddressesService : IAddressesService
    {
        // категории: https://docs.mapbox.com/api/search/geocoding/#point-of-interest-category-coverage

        private const string mapBoxToken = "pk.eyJ1IjoianVyYWhmIiwiYSI6ImNra253dzkweDM1M3QycXF0ZnF5MzZzMjUifQ.RlwEwOAgzieIbpxdy4TyYQ";
        private const string serviceUrl = "https://api.mapbox.com/"; // TODO: в конфиг
        private readonly CultureInfo culture;
        private List<Category> allExistedCategories;

        public MapBoxAddressesService(List<Category> allExistedCategories)
        {
            culture = CultureInfo.InvariantCulture;
            this.allExistedCategories = allExistedCategories;
        }

        public List<NaviAddressInfo> SearchAddresses(AddressSearchArgs args)
        {
            int limit = args.limit ?? 100;
            string query = ($"geocoding/v5/mapbox.places/{args.querystr}.json?" +
                        //$"proximity={args.Lat.ToString(culture)},{args.Lng.ToString(culture)}" +
                        $"&language=ru-RU" +
                        $"&access_token={mapBoxToken}" +
                        $"&limit={limit}"
                        );
            using (var client = CreateClient())
            {
                HttpResponseMessage response = client.GetAsync(query).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseStr = response.Content.ReadAsStringAsync().Result;

                    return new MapboxAddressParser().ParseAddressesList(responseStr, allExistedCategories);
                }
                else
                    throw new WebException($"Сервер адресов вернул код {response.StatusCode}");
            }
        }


        public List<NaviAddressInfo> SearchNear(AddressSearchNearArgs args)
        {
            int limit = args.Limit ?? 1000;

            List<Task<List<NaviAddressInfo>>> taskList = new List<Task<List<NaviAddressInfo>>>();

            foreach (var category in args.Categories)
            {
                string query = ($"geocoding/v5/mapbox.places/{category.Name}.json?" +
                            $"proximity={args.Lng.ToString(culture)},{args.Lat.ToString(culture)}" +
                            $"&language=ru-RU" +
                            $"&access_token={mapBoxToken}" +
                            $"&limit={limit}"
                            );

                taskList.Add(Task.Factory.StartNew(() => SendAndParseGeocodingQuery(query)));
            }

            Task.WaitAll(taskList.ToArray());
            var list = taskList.Where(x => !x.IsFaulted).SelectMany(t => t.Result).ToList();

            // отфильтруем, чтобы было в пределах нашего города // TODO: можно задать через bbox: https://docs.mapbox.com/api/search/geocoding/
            list = list.Where(x => x.Latitude > args.Lat - args.RadiusLat
                && x.Latitude < args.Lat + args.RadiusLat
                && x.Longitude > args.Lng - args.RadiusLng
                && x.Longitude < args.Lng + args.RadiusLng)
            .ToList();

            if (list.Count() < args.NeededPlacesCount) 
                list = list.Concat(list).Take(args.NeededPlacesCount).ToList(); // не хватило - добавляем по нескольку раз ???
            list = list.OrderBy(x => CalcDistance(x, args.City)).Take(args.NeededPlacesCount).ToList();

            return list;
        }


        public List<NaviAddressInfo> SendAndParseGeocodingQuery(string query)
        {
            using (var client = CreateClient())
            {
                HttpResponseMessage response = client.GetAsync(query).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseStr = response.Content.ReadAsStringAsync().Result;

                    var list = new MapboxAddressParser().ParseAddressesList(responseStr, allExistedCategories);
                    return list;
                }
                else
                    throw new WebException($"Сервер адресов вернул код {response.StatusCode}"); // return new List<NaviAddressInfo>();
            }
        }


        private double CalcDistance(NaviAddressInfo x, City y)
        {
            return Math.Sqrt(Math.Pow((double)(x.Latitude - y.Lat), 2) + Math.Pow((double)(x.Longitude - y.Lng), 2));
        }


        private HttpClient CreateClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(serviceUrl);
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/json"));
            // auth info?

            // чтобы можно было https 
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            return client;
        }

    }
}