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

    public partial class PlacePoint
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string CustomName { get; set; }
        public System.DateTime Time { get; set; }

        [JsonIgnore]
        public virtual Schedule Schedule { get; set; }
        public virtual NaviAddressInfo NaviAddressInfo { get; set; }
    }
}
