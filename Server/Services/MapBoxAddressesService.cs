using Server.Classes.Args;
using Server.Interfaces;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Services
{
    public class MapBoxAddressesService : IAddressesService
    {
        private const string mapBoxToken = "pk.eyJ1IjoianVyYWhmIiwiYSI6ImNra253dzkweDM1M3QycXF0ZnF5MzZzMjUifQ.RlwEwOAgzieIbpxdy4TyYQ";

        public NaviAddressInfo GetAddressInfo(string containerAddress, string selfAddress)
        {
            throw new NotImplementedException();
        }

        public List<NaviAddressInfo> SearchAddresses(AddressSearchArgs args)
        {
            throw new NotImplementedException();
        }

        public List<City> SearchCity(CityFilterAndOrder args)
        {
            throw new NotImplementedException();
        }

        public List<NaviAddressInfo> SearchNear(AddressSearchNearArgs args)
        {
            // args.

            // https://api.mapbox.com/geocoding/v5/mapbox.places/кафе.json?limit=50&proximity=56.23212203460389%2C57.9982446865821&language=ru-RU&access_token={}

            throw new NotImplementedException();
        }
    }
}