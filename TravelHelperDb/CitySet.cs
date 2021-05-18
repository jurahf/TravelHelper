using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }


        [InverseProperty(nameof(TravelHelperDb.NaviAddressInfoSet.City))]
        public virtual ICollection<NaviAddressInfoSet> NaviAddressInfoSet { get; set; }

        [InverseProperty(nameof(TravelHelperDb.TravelSet.City))]
        public virtual ICollection<TravelSet> TravelSet { get; set; }
    }
}
