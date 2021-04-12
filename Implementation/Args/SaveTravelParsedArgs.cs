using Implementation.Results;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Implementation.Args
{
    public class SaveTravelParsedArgs
    {
        public int? TravelId { get; set; }
        public User User { get; set; }
        public City City { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Category> Categories { get; set; }
        public List<Schedule> Schedules { get; set; }

        public SaveTravelResult Result { get; set; }
    }
}