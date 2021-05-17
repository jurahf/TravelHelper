using CoreImplementation.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreImplementation.Args
{
    public class AddressSearchNearArgs
    {
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
        public decimal RadiusLat { get; set; }
        public decimal RadiusLng { get; set; }
        public int? Limit { get; set; }

        public int NeededPlacesCount { get; set; }

        public VMCity City { get; set; }

        public List<VMCategory> Categories { get; set; }
    }
}