using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreImplementation.Model
{
    public class VMCategory : Entity
    {
        public string Name { get; set; }
        public string NaviId { get; set; }
        public VMCategory Parent { get; set; }
        public List<VMCategory> Childs { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}