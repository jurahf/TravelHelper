using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreImplementation.Results
{
    public class SaveTravelResult
    {
        public bool Valid { get; set; }

        public string ErrorMessage { get; set; }

        public int? TravelId { get; set; }
    }
}