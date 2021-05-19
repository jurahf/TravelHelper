using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UserId { get; set; }
        public int CityId { get; set; }


        private DateTime currentDate;

        public DateTime CurrentDate 
        {
            get
            {
                if (currentDate < StartDate)
                    currentDate = StartDate;
                else if (currentDate > EndDate)
                    currentDate = EndDate;

                return currentDate;
            }
            set
            {
                if (value < StartDate)
                    currentDate = StartDate;
                else if (value > EndDate)
                    currentDate = EndDate;
                else
                    currentDate = value;
            }
        }


        [ForeignKey(nameof(CityId))]
        [InverseProperty(nameof(TravelHelperDb.CitySet.TravelSet))]
        public virtual CitySet City { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(TravelHelperDb.UserSet.TravelSet))]
        public virtual UserSet User { get; set; }

        [InverseProperty(nameof(TravelHelperDb.ScheduleSet.Travel))]
        public virtual ICollection<ScheduleSet> ScheduleSet { get; set; }

        [InverseProperty(nameof(TravelHelperDb.TravelCategory.TravelCategoryCategory))]
        public virtual ICollection<TravelCategory> TravelCategory { get; set; }
    }
}
