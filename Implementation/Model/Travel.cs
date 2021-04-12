using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Implementation.Model
{
    public class Travel : Entity
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public User User { get; set;}
        public City City { get; set; }
        public List<Category> Categories { get; set; }
        public List<Schedule> Schedules { get; set; }
    }
}