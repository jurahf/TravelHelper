using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Classes.Args
{
    public class PreSaveTravelArgs
    {
        public int? TravelId { get; set; }
        public string UserLogin { get; set; }
        public string City { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<string> Categories { get; set; }
    }
}