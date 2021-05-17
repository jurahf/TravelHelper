using System;
using System.Collections.Generic;
using System.Linq;
using CoreImplementation.Results;
using TravelHelperDb;

namespace Services.ParsedArgs
{
    public class SaveTravelParsedArgs
    {
        public int? TravelId { get; set; }
        public UserSet User { get; set; }
        public CitySet City { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<CategorySet> Categories { get; set; }
        public List<ScheduleSet> Schedules { get; set; }

        public SaveTravelResult Result { get; set; }
    }
}