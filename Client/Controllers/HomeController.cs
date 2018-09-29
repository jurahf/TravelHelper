using Client.Helpers;
using Client.Models;
using Client.RemoteServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        protected User currentUser = UserProvider.GetCurrentUser();
        protected List<Category> categories = new List<Category>();
        protected List<Travel> travels = new List<Travel>();
        protected Travel selectedTravel = null;

        public ActionResult Index()
        {
            // TODO: 
            // Подробности по текущему путешествию
            // Если надо - меняем местоположение

            TravelRemoteService trs = new TravelRemoteService(HttpQueryHelper.CreateHttpClient());
            categories = trs.GetCategories();

            travels = trs.GetTravelsList(currentUser);
            int? selectedTravel_Id = trs.GetSelectedTravelId(currentUser);
            if (selectedTravel_Id != null)
                selectedTravel = travels.FirstOrDefault(t => t.Id == selectedTravel_Id);

            ViewBag.User = currentUser;
            ViewBag.Categories = categories;
            ViewBag.Travels = travels;
            ViewBag.SelectedTravel = selectedTravel;

            if (selectedTravel != null && selectedTravel.Schedules?.Any() == true)
            {
                var tempSchedule = selectedTravel.Schedules
                    .OrderBy(x => Math.Abs((x.Date - DateTime.Today).Days)) // ближайшее к сегодня расписание
                    .FirstOrDefault();

                ViewBag.Schedule = tempSchedule;
                ViewBag.ScheduleTitle = $"Расписание на {tempSchedule.Date:dd.MM.yyyy}";
            }
            else
            {
                ViewBag.ScheduleTitle = $"Расписание на {DateTime.Today:dd.MM.yyyy}";
            }

            return View();
        }
    }
}