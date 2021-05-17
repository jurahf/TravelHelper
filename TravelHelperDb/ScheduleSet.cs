using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TravelHelperDb
{
    public partial class ScheduleSet
    {
        public ScheduleSet()
        {
            PlacePointSet = new HashSet<PlacePointSet>();
        }

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int TempPoint { get; set; }
        public int TravelId { get; set; }

        public virtual TravelSet Travel { get; set; }
        public virtual ICollection<PlacePointSet> PlacePointSet { get; set; }
    }
}
