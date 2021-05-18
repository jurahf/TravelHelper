using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreImplementation.Model
{
    public class VMUser : Entity
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string AvatarUrl { get; set; }
    }
}