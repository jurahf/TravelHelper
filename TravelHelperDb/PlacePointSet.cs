using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TravelHelperDb
{
    public partial class PlacePointSet
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string CustomName { get; set; }
        public DateTime Time { get; set; }
        public int? ScheduleId { get; set; }
        public int NaviAddressInfoId { get; set; }

        public virtual NaviAddressInfoSet NaviAddressInfo { get; set; }
        public virtual ScheduleSet Schedule { get; set; }
    }
}
