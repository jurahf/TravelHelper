using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreImplementation.Dtos;

namespace CoreImplementation.Args
{
    public class SaveScheduleArgs
    {
        public int? ScheduleId { get; set; }

        public string UserLogin { get; set; }

        public List<ScheduleDto> ScheduleRows { get; set; }
    }
}