using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Classes.Args
{
    public class AddressSearchNearArgs
    {
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
        public decimal RadiusLat { get; set; }
        public decimal RadiusLng { get; set; }
        public int? Limit { get; set; }
    }
}