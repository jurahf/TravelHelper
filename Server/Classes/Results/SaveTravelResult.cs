﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Classes.Results
{
    public class SaveTravelResult
    {
        public bool Valid { get; set; }

        public string ErrorMessage { get; set; }

        public int? TravelId { get; set; }
    }
}