using Implementation.Results;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Implementation.Args
{
    public class SaveScheduleParsedArgs
    {
        public int? ScheduleId { get; set; }

        public User User { get; set; }

        public Schedule Schedule { get; set; }

        public SaveScheduleResult Result { get; set; }
    }
}