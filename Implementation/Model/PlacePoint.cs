using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Implementation.Model
{
    public class PlacePoint : Entity
    {
        public int Order { get; set; }
        public DateTime Time { get; set; }
        public string CustomName { get; set; }
        public AddressInfo NaviAddressInfo { get; set; }

        public override string ToString()
        {
            string result = string.IsNullOrEmpty(CustomName) ? NaviAddressInfo?.Description : CustomName;
            if (NaviAddressInfo != null)
            {
                result += $" [{NaviAddressInfo.ContainerAddress}]{NaviAddressInfo.SelfAddress}";
            }
            return result;
        }
    }
}