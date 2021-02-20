using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using Server.Classes;
using Server.Classes.Args;
using Server.Classes.Results;
using Server.Interfaces;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    public class TravelController : ApiController
    {
        private readonly ITravelService travelSrvice;

        public TravelController()
        {
            travelSrvice = new ServiceFactory().GetTravelService();
        }

        


        [HttpGet]
        public List<Category> GetAllCategories()
        {
            return travelSrvice.GetAllCategories();
        }


        [HttpGet]
        public List<Travel> GetTravelsList(string login)
        {
            return travelSrvice.GetTravelsList(login);
        }

        [HttpGet]
        public int? GetSelectedTravel(string login)
        {
            return travelSrvice.GetSelectedTravelId(login);
        }

        [HttpGet]
        public int SelectTravel(string login, int id)
        {
            return travelSrvice.SelectTravel(login, id);
        }

        [HttpPut]
        public PreSaveTravelResult PreSaveTravel([FromBody] PreSaveTravelArgs preSaveArgs)
        {
            return travelSrvice.PreSaveTravel(preSaveArgs);
        }

        [HttpPut]
        public SaveTravelResult SaveTravel([FromBody] SaveTravelArgs saveArgs)
        {
            return travelSrvice.SaveTravel(saveArgs);
        }

        [HttpPut]
        public SaveScheduleResult SaveSchedule([FromBody] SaveScheduleArgs saveArgs)
        {
            return travelSrvice.SaveSchedule(saveArgs);
        }


    }
}
