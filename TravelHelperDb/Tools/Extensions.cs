using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace TravelHelperDb
{
    partial class CategorySet
    {
        public override string ToString()
        {
            return $"{this.Name}";
        }

    }

    partial class NaviAddressInfoSet
    {
        public override string ToString()
        {
            return $"[{ContainerAddress}]{SelfAddress} {Description}";
        }
    }

    partial class CitySet
    {
        public override string ToString()
        {
            return $"{Name} ({Country})";
        }
    }

    partial class UserSet
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