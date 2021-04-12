using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Implementation.Model
{
    public class City : Entity
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
    }
}