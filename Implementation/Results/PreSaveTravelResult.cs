using System;
using System.Collections.Generic;
using System.Linq;
using Implementation.Model;

namespace Implementation.Results
{
    public class PreSaveTravelResult
    {
        public bool Valid { get; set; }

        public bool CityValid { get; set; }
        public bool DatesValid { get; set; }

        public string ErrorMessage { get; set; }

        public List<VMSchedule> Schedules { get; set; }
    }
}