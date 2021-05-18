using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string NaviId { get; set; }
        public int? ParentId { get; set; }

        [ForeignKey(nameof(ParentId))]
        [InverseProperty(nameof(TravelHelperDb.CategorySet.InverseParent))]
        public virtual CategorySet Parent { get; set; }

        [InverseProperty(nameof(TravelHelperDb.CategorySet.Parent))]
        public virtual ICollection<CategorySet> InverseParent { get; set; }

        [InverseProperty(nameof(TravelHelperDb.NaviAddressInfoSet.Category))]
        public virtual ICollection<NaviAddressInfoSet> NaviAddressInfoSet { get; set; }

        [InverseProperty(nameof(TravelHelperDb.TravelCategory.Categories))]
        public virtual ICollection<TravelCategory> TravelCategory { get; set; }
    }
}
