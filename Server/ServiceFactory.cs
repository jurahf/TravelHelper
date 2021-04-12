using System;
using System.Collections.Generic;
using System.Linq;
using Server.Models;
using Server.Services.DatabaseTravel;
using Server.Services.MapBoxAddresses;
using Implementation.ServiceInterfaces;

namespace Server
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