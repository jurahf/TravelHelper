using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Classes.Args
{
    public class CityFilterAndOrder
    {
        public string querystr { get; set; }
        public int? limit { get; set; }

        public decimal? lat { get; set; }
        public decimal? lng { get; set; }
    }
}