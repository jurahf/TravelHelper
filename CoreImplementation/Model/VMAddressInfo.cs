using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreImplementation.Model
{
    public class VMAddressInfo : Entity
    {
        private const int shortDescriptionLen = 200;
        private const int maxLondDescriptionLen = 210;
        private const string basePicturePath = "/images/places/";


        public string Name { get; set; }
        public string ContainerAddress { get; set; }
        public string SelfAddress { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Picture { get; set; }

        private string description;
        public string Description 
        {
            get { return description; }
            set { description = value?.Replace(Environment.NewLine, " "); }
        }

        public VMCategory Category { get; set; }


        public string SafePictureUrl
        {
            get
            {
                if (string.IsNullOrEmpty(Picture))
                    return "";

                if (Picture.ToLower().StartsWith("http"))
                    return Picture;
                else
                    return "/images/places/" + Picture.Trim(new char[] { '/' } );
            }
        }


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