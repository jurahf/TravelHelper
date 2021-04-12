using Implementation.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Implementation.Args
{
    public class AddressSearchNearArgs
    {
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
        public decimal RadiusLat { get; set; }
        public decimal RadiusLng { get; set; }
        public int? Limit { get; set; }

        public int NeededPlacesCount { get; set; }

        public City City { get; set; }

        public List<Category> Categories { get; set; }
    }
}