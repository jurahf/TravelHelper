//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Server.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public partial class Schedule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Schedule()
        {
            this.PlacePoint = new HashSet<PlacePoint>();
        }
    
        public int Id { get; set; }
        public System.DateTime Date { get; set; }
        public int TempPoint { get; set; }

        [JsonIgnore]
        public virtual Travel Travel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlacePoint> PlacePoint { get; set; }
    }
}
