using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Classes.Dtos
{
    public class ScheduleDto
    {
        public string DateTime { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
    }
}