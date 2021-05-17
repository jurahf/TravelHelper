using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TravelHelperDb
{
    public partial class CategorySet
    {
        public CategorySet()
        {
            InverseParent = new HashSet<CategorySet>();
            NaviAddressInfoSet = new HashSet<NaviAddressInfoSet>();
            TravelCategory = new HashSet<TravelCategory>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string NaviId { get; set; }
        public int? ParentId { get; set; }

        public virtual CategorySet Parent { get; set; }
        public virtual ICollection<CategorySet> InverseParent { get; set; }
        public virtual ICollection<NaviAddressInfoSet> NaviAddressInfoSet { get; set; }
        public virtual ICollection<TravelCategory> TravelCategory { get; set; }
    }
}
