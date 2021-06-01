using CoreImplementation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreImplementation.Args
{
    public class GenerateTravelArgs
    {
        public VMUser User { get; set; }
        public VMCity City { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<VMCategory> Categories { get; set; }
    }
}