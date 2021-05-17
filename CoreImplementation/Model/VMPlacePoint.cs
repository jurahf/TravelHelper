using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreImplementation.Model
{
    public class VMPlacePoint : Entity
    {
        public int Order { get; set; }
        public DateTime Time { get; set; }
        public string CustomName { get; set; }
        public VMAddressInfo AddressInfo { get; set; }

        public override string ToString()
        {
            string result = string.IsNullOrEmpty(CustomName) ? AddressInfo?.Description : CustomName;
            if (AddressInfo != null)
            {
                result += $" [{AddressInfo.ContainerAddress}]{AddressInfo.SelfAddress}";
            }
            return result;
        }
    }
}