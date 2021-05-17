using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreImplementation.Results
{
    public class SaveScheduleResult
    {
        public bool Valid { get; set; }

        public string ErrorMessage { get; set; }

        public int? ScheduleId { get; set; }
    }
}