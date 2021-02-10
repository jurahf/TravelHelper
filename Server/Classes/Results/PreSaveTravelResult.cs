using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Classes.Results
{
    public class PreSaveTravelResult
    {
        public bool Valid { get; set; }

        public bool CityValid { get; set; }
        public bool DatesValid { get; set; }

        public string ErrorMessage { get; set; }

        public List<Schedule> Schedules { get; set; }
    }
}