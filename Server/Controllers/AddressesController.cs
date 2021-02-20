using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.Classes;
using Server.Classes.Args;
using Server.Interfaces;
using Server.Models;
using Server.Services;
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
    public class AddressesController : ApiController
    {
        private readonly IAddressesService addressesService;
        private readonly DBWork data;

        public AddressesController()
        {
            data = new DBWork();
            addressesService = new ServiceFactory().GetAddressesService();
        }

        [HttpGet]
        public List<NaviAddressInfo> SearchAddresses([FromUri]AddressSearchArgs args)
        {
            return addressesService.SearchAddresses(args);
        }


        [HttpGet]
        public List<City> SearchCity([FromUri] CityFilterAndOrder args)
        {
            return data.GetCities(args);
        }


        [HttpGet]
        public List<NaviAddressInfo> SearchNear([FromUri] AddressSearchNearArgs args)
        {
            return addressesService.SearchNear(args);
        }


    }
}
