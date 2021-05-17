using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TravelHelperDb
{
    public partial class TravelCategory
    {
        public int TravelCategoryCategoryId { get; set; }
        public int CategoriesId { get; set; }

        public virtual CategorySet Categories { get; set; }
        public virtual TravelSet TravelCategoryCategory { get; set; }
    }
}
