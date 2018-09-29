using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Models.db
{
    public static class Extensions
    {
        
    }

}

namespace Server.Models
{
    partial class Category
    {
        public override string ToString()
        {
            return $"{this.Name}";
        }

    }

    partial class NaviAddressInfo
    {
        public override string ToString()
        {
            return $"[{ContainerAddress}]{SelfAddress} {Name}";
        }
    }

    partial class City
    {
        public override string ToString()
        {
            return $"{Name} ({Country})";
        }
    }

    partial class User
    {
        //[JsonIgnore]
        // Schedule.Travel
        // PlacePoint.Schedule
        // NaviAddressInfo.PlacePoint
        public override string ToString()
        {
            return Login;
        }
    }

}