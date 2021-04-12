using Implementation.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Implementation.Args
{
    public class SaveScheduleArgs
    {
        public int? ScheduleId { get; set; }

        public string UserLogin { get; set; }

        public List<ScheduleDto> ScheduleRows { get; set; }
    }
}