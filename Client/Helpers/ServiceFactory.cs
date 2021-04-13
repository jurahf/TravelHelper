using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Implementation.ServiceInterfaces;
using Server.Services.DatabaseTravel;

namespace Client.Helpers
{
    // TODO: заменить на DI
    // из-за этого класса Client ссылается на Server

    public static class ServiceFactory
    {
        public static ITravelService GetTravelService()
        {
            return new DatabaseTravelService();
        }

    }
}