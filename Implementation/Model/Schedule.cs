﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Implementation.Model
{
    public class Schedule : Entity
    {
        public DateTime Date { get; set; }
        public List<PlacePoint> PlacePoint { get; set; }
        public int TempPoint { get; set; }
    }
}