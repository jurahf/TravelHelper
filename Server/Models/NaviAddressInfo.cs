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

    public partial class NaviAddressInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NaviAddressInfo()
        {
            this.PlacePoint = new HashSet<PlacePoint>();
        }
    
        public int Id { get; set; }
        public string ContainerAddress { get; set; }
        public string SelfAddress { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Место в расписании
        /// </summary>
        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlacePoint> PlacePoint { get; set; }
        public virtual Category Category { get; set; }
        public virtual City City { get; set; }
    }
}