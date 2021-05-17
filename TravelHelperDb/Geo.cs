using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TravelHelperDb
{
    public partial class Geo
    {
        public int Id { get; set; }
        public string CountryEn { get; set; }
        public string RegionEn { get; set; }
        public string CityEn { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public int Population { get; set; }
    }
}
