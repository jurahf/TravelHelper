using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreImplementation.Model
{
    public class VMSchedule : Entity
    {
        public DateTime Date { get; set; }
        public List<VMPlacePoint> PlacePoint { get; set; }
        public int TempPoint { get; set; }
    }
}