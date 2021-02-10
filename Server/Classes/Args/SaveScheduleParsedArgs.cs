using Server.Classes.Results;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Classes.Args
{
    public class SaveScheduleParsedArgs
    {
        public int? ScheduleId { get; set; }

        public User User { get; set; }

        public Schedule Schedule { get; set; }

        public SaveScheduleResult Result { get; set; }
    }
}