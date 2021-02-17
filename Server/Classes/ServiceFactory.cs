using Server.Interfaces;
using Server.Models;
using Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Classes
{
    public class ServiceFactory
    {
        public DBWork CreateDbWork()
        {
            return new DBWork();
        }

        public ITravelService GetTravelService()
        {
            return new DatabaseTravelService();
        }

        public IAddressesService GetAddressesService()
        {
            return new MapBoxAddressesService(CreateDbWork().GetFromDatabase<Category>());
        }

    }
}