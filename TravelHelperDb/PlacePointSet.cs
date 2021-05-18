﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TravelHelperDb
{
    public partial class PlacePointSet
    {
        [Key]
        public int Id { get; set; }
        public int Order { get; set; }
        public string CustomName { get; set; }
        public DateTime Time { get; set; }
        public int? ScheduleId { get; set; }
        public int NaviAddressInfoId { get; set; }

        [ForeignKey(nameof(NaviAddressInfoId))]
        [InverseProperty(nameof(TravelHelperDb.NaviAddressInfoSet.PlacePointSet))]
        public virtual NaviAddressInfoSet NaviAddressInfo { get; set; }

        [ForeignKey(nameof(ScheduleId))]
        [InverseProperty(nameof(TravelHelperDb.ScheduleSet.PlacePointSet))]
        public virtual ScheduleSet Schedule { get; set; }
    }
}
