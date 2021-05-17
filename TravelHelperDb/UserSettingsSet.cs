﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TravelHelperDb
{
    public partial class UserSettingsSet
    {
        public UserSettingsSet()
        {
            UserSet = new HashSet<UserSet>();
        }

        public int Id { get; set; }
        public int? SelectedTravelId { get; set; }

        public virtual ICollection<UserSet> UserSet { get; set; }
    }
}
