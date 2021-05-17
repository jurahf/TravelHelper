using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TravelHelperDb
{
    public partial class CitySet
    {
        public CitySet()
        {
            NaviAddressInfoSet = new HashSet<NaviAddressInfoSet>();
            TravelSet = new HashSet<TravelSet>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }

        public virtual ICollection<NaviAddressInfoSet> NaviAddressInfoSet { get; set; }
        public virtual ICollection<TravelSet> TravelSet { get; set; }
    }
}
