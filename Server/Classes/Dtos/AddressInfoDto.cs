using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Classes.Dtos
{
    public class AddressInfoDto
    {
        //public int Id { get; set; }
        public string ContainerAddress { get; set; }
        public string SelfAddress { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public CategoryDto Category { get; set; }
        public CityDto City { get; set; }
    }

    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NaviId { get; set; }
    }


    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
    }

}