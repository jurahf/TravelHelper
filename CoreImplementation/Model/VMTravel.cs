using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreImplementation.Model
{
    public class VMTravel : Entity
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public VMUser User { get; set;}
        public VMCity City { get; set; }
        public List<VMCategory> Categories { get; set; }
        public List<VMSchedule> Schedules { get; set; }
    }
}