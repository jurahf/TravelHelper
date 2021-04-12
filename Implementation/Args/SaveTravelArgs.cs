using Implementation.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Implementation.Args
{
    public class SaveTravelArgs
    {
        public int? TravelId { get; set; }
        public string UserLogin { get; set; }
        public string City { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<string> Categories { get; set; }
        public List<ScheduleDto> Schedules { get; set; }
        public List<PlaceDto> AdditionalPlaces { get; set; }
    }
}