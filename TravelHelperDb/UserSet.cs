using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TravelHelperDb
{
    public partial class UserSet
    {
        public UserSet()
        {
            TravelSet = new HashSet<TravelSet>();
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public int? UserSettingsId { get; set; }

        public virtual UserSettingsSet UserSettings { get; set; }
        public virtual ICollection<TravelSet> TravelSet { get; set; }
    }
}
