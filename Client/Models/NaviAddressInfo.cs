using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public class NaviAddressInfo : Entity
    {
        private const int shortDescriptionLen = 200;
        private const int maxLondDescriptionLen = 210;
        
        
        public string ContainerAddress { get; set; }
        public string SelfAddress { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }



        public string ЗатравочкаОписания
        {
            get
            {
                if (string.IsNullOrEmpty(Description) || Description.Length <= maxLondDescriptionLen)
                    return Description;

                return Description.Substring(0, shortDescriptionLen);
            }
        }

    }
}