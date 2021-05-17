using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TravelHelperDb
{
    public partial class TravelSet
    {
        public TravelSet()
        {
            ScheduleSet = new HashSet<ScheduleSet>();
            TravelCategory = new HashSet<TravelCategory>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UserId { get; set; }
        public int CityId { get; set; }

        public virtual CitySet City { get; set; }
        public virtual UserSet User { get; set; }
        public virtual ICollection<ScheduleSet> ScheduleSet { get; set; }
        public virtual ICollection<TravelCategory> TravelCategory { get; set; }
    }
}
