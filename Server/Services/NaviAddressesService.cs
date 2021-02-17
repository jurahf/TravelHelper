using Server.Classes;
using Server.Classes.Args;
using Server.Interfaces;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Server.Services
{
    public class NaviAddressesService : IAddressesService
    {
        // TODO: из конфига
        private string serviceUrl = "https://api.naviaddress.com/api/v1.5/";  // вроде там был еще какой-то тестовый


        public List<NaviAddressInfo> SearchAddresses(AddressSearchArgs args)
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


        public List<NaviAddressInfo> SearchNear(AddressSearchNearArgs args)
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

                var list = new NaviAddressParser().ParseAddressesList(responseStr);

                List<NaviAddressInfo> filteredAddresses = new List<NaviAddressInfo>(list);
                List<string> ids = args.Categories.Select(x => x.NaviId).ToList();
                filteredAddresses = list.Where(x => x.Category != null && ids.Contains(x.Category.NaviId)).ToList();

                filteredAddresses = filteredAddresses.OrderBy(x => CalcDistance(x, args.City)).Take(args.NeededPlacesCount).ToList();
                if (filteredAddresses.Count() < args.NeededPlacesCount) // TODO: на случай, если мест не хватило - добавляем из других категорий ???
                    filteredAddresses = filteredAddresses.Concat(filteredAddresses).Take(args.NeededPlacesCount).ToList();

                // получаем дополнительную информацию по адресам
                filteredAddresses = LoadAdditionalInfoParallel(filteredAddresses);

                return filteredAddresses;
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


        private List<NaviAddressInfo> LoadAdditionalInfoParallel(List<NaviAddressInfo> addressesList)
        {
            int degreeOfParallelism = 10;
            int addrPerTask = Math.Max(addressesList.Count / degreeOfParallelism, 1);
            List<Task<List<NaviAddressInfo>>> taskList = new List<Task<List<NaviAddressInfo>>>();

            for (int i = 0; i < degreeOfParallelism; i++)
            {
                var innerList = addressesList.Skip(addrPerTask * i).Take(addrPerTask).ToList();
                taskList.Add(Task.Factory.StartNew(
                    () => LoadAdditionalInfoInner(innerList))
                );
            }

            Task.WaitAll(taskList.ToArray());
            return taskList.SelectMany(t => t.Result).ToList();
        }


        private List<NaviAddressInfo> LoadAdditionalInfoInner(List<NaviAddressInfo> addressesList)
        {
            List<NaviAddressInfo> resultList = new List<NaviAddressInfo>();

            foreach (var address in addressesList)
            {
                NaviAddressInfo additionalInfo = null;
                try
                {
                    // пробуем поискать сначала по базе, может сохраняли уже такой адрес
                    //additionalInfo = data.GetFromDatabase<NaviAddressInfo>(x => x.ContainerAddress == address.ContainerAddress && x.SelfAddress == address.SelfAddress).FirstOrDefault();

                    if (additionalInfo == null)
                        additionalInfo = GetAddressInfo(address.ContainerAddress, address.SelfAddress);
                }
                catch
                {
                }

                resultList.Add(additionalInfo ?? address);
            }

            return resultList;
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