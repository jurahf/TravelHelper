using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TravelHelperDb;

namespace Services.Services.MapBoxAddresses
{
    public class MapboxAddressParser
    {
        public List<NaviAddressInfoSet> ParseAddressesList(string json, List<CategorySet> allExistedCategories)
        {
            MapboxResponse response = JsonConvert.DeserializeObject<MapboxResponse>(json);

            List<NaviAddressInfoSet> result = new List<NaviAddressInfoSet>();
            foreach (var feature in response.Features)
            {
                result.Add(new NaviAddressInfoSet()
                {
                    Description = feature.Place_name,
                    SelfAddress = feature.Properties?.Address,
                    Longitude = feature.Center[0],
                    Latitude = feature.Center[1],
                    Category = allExistedCategories.FirstOrDefault(x => string.Equals(x.Name, response.Query.FirstOrDefault(), StringComparison.InvariantCultureIgnoreCase))
                    // City = 
                    // ContainerAddress = 
                    // Picture =
                    // PlacePoint =  - это уже место в расписании, определяется не здесь
                });
            }

            return result;
        }
    }


    public class MapboxResponse
    {
        public string Type { get; set; }
        public List<string> Query { get; set; }
        public List<MapboxFeature> Features { get; set; }
    }


    public class MapboxFeature
    {
        public string Id { get; set; }
        public string Type { get; set; }

        public decimal Relevance { get; set; }

        public MapboxFeatureProperties Properties { get; set; }

        public string Text { get; set; }
        public string Place_name { get; set; }
        public decimal[] Center { get; set; }
    }


    public class MapboxFeatureProperties
    {
        public string Foursquare { get; set; }
        public bool Landmark { get; set; }
        public string Address { get; set; }
        public string Category { get; set; }
    }


}