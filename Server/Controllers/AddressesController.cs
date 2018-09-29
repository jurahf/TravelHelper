using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.Classes;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Server.Controllers
{
    // TODO: адрес клиента из конфига
    [EnableCors(origins: "http://localhost:2168", headers: "*", methods: "*")]
    public class AddressesController : ApiController
    {
        private DBWork data = new DBWork();
        // TODO: из конфига
        private string serviceUrl = "https://api.naviaddress.com/api/v1.5/";  // вроде там был еще какой-то тестовый

        [HttpGet]
        public List<NaviAddressInfo> SearchAddresses([FromUri]AddressSearchArgs args)
        {
            int limit = args.limit ?? 100;
            string query = $"Addresses/Search?lat=0&lng=0&querystr={args.querystr}&limit={limit}";
            var client = CreateClient();

            HttpResponseMessage response = client.GetAsync(query).Result;

            if (response.IsSuccessStatusCode)
            {
                string responseStr = response.Content.ReadAsStringAsync().Result;

                return new NaviAddressParser().ParseAddressesList(responseStr);
            }
            else
                throw new WebException($"Сервер адресов вернул код {response.StatusCode}");
        }


        [HttpGet]
        public List<City> SearchCity([FromUri] CityFilterAndOrder args)
        {
            return data.GetCities(args);
        }


        [HttpGet]
        public List<NaviAddressInfo> SearchNear([FromUri] AddressSearchNearArgs args)
        {
            int limit = args.Limit ?? 1000;

            string query = ($"Map?" +
                        $"lt_lat={args.Lat - args.RadiusLat:0.####}" +
                        $"&lt_lng={args.Lng - args.RadiusLng:0.####}" +
                        $"&rb_lat={args.Lat + args.RadiusLat:0.####}" +
                        $"&rb_lng={args.Lng + args.RadiusLng:0.####}" +
                        $"&limit={limit}" +
                        $"&zoom=15"     // методом тыка
                        //$"&address_type=free"
                        ).Replace(',', '.');  // лучше задать через культуру

            var client = CreateClient();

            HttpResponseMessage response = client.GetAsync(query).Result;

            if (response.IsSuccessStatusCode)
            {
                string responseStr = response.Content.ReadAsStringAsync().Result;

                return new NaviAddressParser().ParseAddressesList(responseStr);
            }
            else
                throw new WebException($"Сервер адресов вернул код {response.StatusCode}");
        }

        public NaviAddressInfo GetAddressInfo(string containerAddress, string selfAddress)
        {
            string query = ($"Addresses/{containerAddress}/{selfAddress}?lang=ru");

            var client = CreateClient();

            HttpResponseMessage response = client.GetAsync(query).Result;

            if (response.IsSuccessStatusCode)
            {
                string responseStr = response.Content.ReadAsStringAsync().Result;

                return new NaviAddressParser().ParseAddresseSingle(responseStr);
            }
            else
                throw new WebException($"Сервер адресов вернул код {response.StatusCode}");
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

        public class AddressSearchNearArgs
        {
            public decimal Lat { get; set; }
            public decimal Lng { get; set; }
            public decimal RadiusLat { get; set; }
            public decimal RadiusLng { get; set; }
            public int? Limit { get; set; }
        }

        public class AddressSearchArgs
        {
            public string querystr { get; set; }
            public int? limit { get; set; }
        }

        public class CityFilterAndOrder
        {
            public string querystr { get; set; }
            public int? limit { get; set; }

            public decimal? lat { get; set; }
            public decimal? lng { get; set; }
        }
    }
}
