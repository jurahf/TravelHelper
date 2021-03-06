﻿using System;
using System.Collections.Generic;
using System.Linq;
using Implementation.Results;
using Server.Models;

namespace Server.ParsedArgs
{
    public class PreSaveTravelParsedArgs
    {
        public Travel Travel { get; set; }
        public User User { get; set; }
        public City City { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Category> Categories { get; set; }

        public PreSaveTravelResult Result { get; set; }
    }
}