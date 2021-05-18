using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TravelHelperDb
{
    public partial class NaviAddressInfoSet
    {
        public NaviAddressInfoSet()
        {
            PlacePointSet = new HashSet<PlacePointSet>();
        }

        [Key]
        public int Id { get; set; }
        public string ContainerAddress { get; set; }
        public string SelfAddress { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public int? CategoryId { get; set; }
        public int? CityId { get; set; }
        public string Name { get; set; }


        [ForeignKey(nameof(CategoryId))]
        [InverseProperty(nameof(TravelHelperDb.CategorySet.NaviAddressInfoSet))]
        public virtual CategorySet Category { get; set; }

        [ForeignKey(nameof(CityId))]
        [InverseProperty(nameof(TravelHelperDb.CitySet.NaviAddressInfoSet))]
        public virtual CitySet City { get; set; }

        [InverseProperty(nameof(TravelHelperDb.PlacePointSet.NaviAddressInfo))]
        public virtual ICollection<PlacePointSet> PlacePointSet { get; set; }
    }
}
