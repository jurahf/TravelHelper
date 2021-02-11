using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Classes
{
    public class NaviAddressParser
    {
        public NaviAddressInfo ParseAddresseSingle(string json)
        {
            JObject objects = JsonConvert.DeserializeObject<JObject>(json);
            List<NaviAddressInfo> result = new List<NaviAddressInfo>();

            var children = objects.First.Children();

            return ParseAddress(children.FirstOrDefault());
        }

        public List<NaviAddressInfo> ParseAddressesList(string json)
        {
            JObject objects = JsonConvert.DeserializeObject<JObject>(json);
            List<NaviAddressInfo> result = new List<NaviAddressInfo>();

            var children = objects.First.First.Children();
            if (!children.Any())
                children = objects.First.Next.First.Children();

            foreach (var obj in children)
            {
                var addrInfo = ParseAddress(obj);
                result.Add(addrInfo);
            }

            return result;
        }

        private NaviAddressInfo ParseAddress(JToken obj)
        {
            NaviAddressInfo addrInfo = new NaviAddressInfo();
            foreach (JProperty field in obj.Children().OfType<JProperty>())
            {
                try
                {
                    ParseAddressField(field, addrInfo);
                }
                catch
                {
                    continue;
                }
            }

            return addrInfo;
        }

        private void ParseAddressField(JProperty field, NaviAddressInfo addrInfo)
        {
            switch (field.Name.ToLower())
            {
                case "container":
                    addrInfo.ContainerAddress = field.Value.Value<string>();
                    break;
                case "naviaddress":
                    addrInfo.SelfAddress = field.Value.Value<string>();
                    break;
                case "name":
                    addrInfo.Description = field.Value.Value<string>();
                    break;
                case "image":
                    if (string.IsNullOrEmpty(addrInfo.Picture))
                        addrInfo.Picture = field.Value.Value<string>();
                    break;
                case "cover":
                    if (string.IsNullOrEmpty(addrInfo.Picture))
                    {
                        addrInfo.Picture = field.First?.First?.Children()
                            .OfType<JProperty>()
                            .FirstOrDefault(t => t.Name.ToLower() == "image")
                            ?.Value?.Value<string>();
                    }
                    break;
                case "point":    // "point":{"lat":58.001388888,"lng":56.2475}}
                    addrInfo.Latitude = field.Children().Children()
                        .OfType<JProperty>()
                        .First(t => t.Name.ToLower() == "lat")
                        .Value.Value<decimal>();
                    addrInfo.Longitude = field.Children().Children()
                        .OfType<JProperty>()
                        .First(t => t.Name.ToLower() == "lng")
                        .Value.Value<decimal>();
                    break;
                case "category": // "category":{"id":44}
                    string catId = field.Children().Children()
                        .OfType<JProperty>()
                        .First(t => t.Name.ToLower() == "id")
                        .Value.Value<string>();
                    addrInfo.Category = new Category() { NaviId = catId };
                    break;
            }
        }



    }
}