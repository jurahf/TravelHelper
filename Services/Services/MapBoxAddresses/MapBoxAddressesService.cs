using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CoreImplementation.Args;
using CoreImplementation.ServiceInterfaces;
using CoreImplementation.Model;
using TravelHelperDb;
using Services.ModelsTools;

namespace Services.Services.MapBoxAddresses
{
    public class MapBoxAddressesService : IAddressesService
    {
        // категории: https://docs.mapbox.com/api/search/geocoding/#point-of-interest-category-coverage

        private const string mapBoxToken = "pk.eyJ1IjoianVyYWhmIiwiYSI6ImNra253dzkweDM1M3QycXF0ZnF5MzZzMjUifQ.RlwEwOAgzieIbpxdy4TyYQ";
        private const string serviceUrl = "https://api.mapbox.com/"; // TODO: в конфиг
        private readonly CultureInfo culture;
        private readonly TravelHelperDatabaseContext data;
        private List<CategorySet> allExistedCategories;


        public MapBoxAddressesService(TravelHelperDatabaseContext data)
        {
            culture = CultureInfo.InvariantCulture;
            this.allExistedCategories = data.CategorySet.ToList();
            this.data = data;
        }



        public VMAddressInfo GetAddressInfo(int id)
        {
            return data.NaviAddressInfoSet.FirstOrDefault(x => x.Id == id).ConvertToVm();
        }














        public List<VMAddressInfo> SearchAddresses(AddressSearchArgs args)
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

                    return new MapboxAddressParser()
                        .ParseAddressesList(responseStr, allExistedCategories)
                        .Select(x => x.ConvertToVm())
                        .ToList();
                }
                else
                    throw new WebException($"Сервер адресов вернул код {response.StatusCode}");
            }
        }


        public List<VMAddressInfo> SearchNear(AddressSearchNearArgs args)
        {
            int limit = args.Limit ?? 1000;

            List<Task<List<NaviAddressInfoSet>>> taskList = new List<Task<List<NaviAddressInfoSet>>>();

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

            return list.Select(x => x.ConvertToVm()).ToList();
        }


        public List<NaviAddressInfoSet> SendAndParseGeocodingQuery(string query)
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


        private double CalcDistance(NaviAddressInfoSet x, VMCity y)
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