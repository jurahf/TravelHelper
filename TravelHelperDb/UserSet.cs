using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Key]
        public int Id { get; set; }
        public string Login { get; set; }
        public int? UserSettingsId { get; set; }

        [ForeignKey(nameof(UserSettingsId))]
        [InverseProperty(nameof(TravelHelperDb.UserSettingsSet.UserSet))]
        public virtual UserSettingsSet UserSettings { get; set; }

        [InverseProperty(nameof(TravelHelperDb.TravelSet.User))]
        public virtual ICollection<TravelSet> TravelSet { get; set; }
    }
}
