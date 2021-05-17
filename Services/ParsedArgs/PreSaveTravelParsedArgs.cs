using System;
using System.Collections.Generic;
using System.Linq;
using CoreImplementation.Results;
using TravelHelperDb;

namespace Services.ParsedArgs
{
    public class PreSaveTravelParsedArgs
    {
        public TravelSet Travel { get; set; }
        public UserSet User { get; set; }
        public CitySet City { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<CategorySet> Categories { get; set; }

        public PreSaveTravelResult Result { get; set; }
    }
}