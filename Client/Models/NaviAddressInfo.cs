using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public class NaviAddressInfo : Entity
    {
        public string ContainerAddress { get; set; }
        public string SelfAddress { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Picture { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
    }
}