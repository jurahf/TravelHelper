using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Classes.Args
{
    public class AddressSearchArgs
    {
        public string querystr { get; set; }
        public int? limit { get; set; }
    }
}