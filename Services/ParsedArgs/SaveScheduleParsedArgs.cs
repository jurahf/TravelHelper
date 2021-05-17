using System;
using System.Collections.Generic;
using System.Linq;
using CoreImplementation.Results;
using TravelHelperDb;

namespace Services.ParsedArgs
{
    public class SaveScheduleParsedArgs
    {
        public int? ScheduleId { get; set; }

        public UserSet User { get; set; }

        public ScheduleSet Schedule { get; set; }

        public SaveScheduleResult Result { get; set; }
    }
}