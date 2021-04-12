using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Implementation.Model
{
    public class Category : Entity
    {
        public string Name { get; set; }
        public string NaviId { get; set; }
        public Category Parent { get; set; }
        public List<Category> Childs { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}