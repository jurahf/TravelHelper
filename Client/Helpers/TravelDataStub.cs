using Implementation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Helpers
{
    public class TravelDataStub
    {
        public static Travel GetTravel()
        {
            return new Travel()
            {
                
            };
        }

        public static Schedule GetSchedule()
        {
            return new Schedule()
            {
                Id = 1,
                Date = DateTime.Today,
                TempPoint = 1,
                PlacePoint = new List<PlacePoint>()
                {
                    new PlacePoint()
                    {
                        Id = 1,
                        CustomName = "",
                        Order = 0,
                        Time = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 09, 00, 00),
                        NaviAddressInfo = new NaviAddressInfo()
                        {
                            Description = "Ярославский вокзал",
                            ContainerAddress = "7",
                            SelfAddress = "722498"
                        }
                    },
                    new PlacePoint()
                    {
                        Id = 2,
                        CustomName = "Гостиница",
                        Order = 1,
                        Time = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 12, 00, 00),
                        NaviAddressInfo = new NaviAddressInfo()
                        {
                            Category = new Category() { Id = 1, Name = "Гостиница", NaviId = "2" }, 
                            Description = "Гостиница в Измайлово",
                            ContainerAddress = "7",
                            SelfAddress = "7000489"
                        }
                    }
                }
            };
        }
    }
}