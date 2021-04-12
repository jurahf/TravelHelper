using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Implementation.Dtos
{
    public class ScheduleDto
    {
        public int? PlacePointId { get; set; }
        public string DateTime { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public AddressInfoDto AddressInfo { get; set; }
    }
}