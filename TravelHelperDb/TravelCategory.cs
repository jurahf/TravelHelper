using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TravelHelperDb
{
    public partial class TravelCategory
    {
        [Key]
        public int Id { get; set; }

        public int TravelCategoryCategoryId { get; set; }
        public int CategoriesId { get; set; }


        [ForeignKey(nameof(CategoriesId))]
        [InverseProperty(nameof(TravelHelperDb.CategorySet.TravelCategory))]
        public virtual CategorySet Categories { get; set; }

        [ForeignKey(nameof(TravelCategoryCategoryId))]
        [InverseProperty(nameof(TravelHelperDb.TravelSet.TravelCategory))]
        public virtual TravelSet TravelCategoryCategory { get; set; }
    }
}
