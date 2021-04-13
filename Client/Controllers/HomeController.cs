using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Client.Helpers;
using Implementation.Model;
using Implementation.ServiceInterfaces;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        protected VMUser currentUser = UserProvider.GetCurrentUser();
        protected readonly ITravelService travelService = ServiceFactory.GetTravelService();

        protected List<VMCategory> categories = new List<VMCategory>();
        protected List<VMTravel> travels = new List<VMTravel>();
        protected VMTravel selectedTravel = null;

        public ActionResult Index()
        {
            // TODO: 
            // Подробности по текущему путешествию
            // Если надо - меняем местоположение

            categories = travelService.GetAllCategories();

            travels = travelService.GetTravelsList(currentUser);
            int? selectedTravel_Id = travelService.GetSelectedTravelId(currentUser);
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

                int order = selectedTravel.Schedules.OrderBy(x => x.Date).ToList().IndexOf(tempSchedule);
                ViewBag.SelectedScheduleID = tempSchedule.Id;
                ViewBag.Schedule = tempSchedule;
                ViewBag.ScheduleTitle = $"Расписание на {order + 1}-й день";
            }
            else
            {
                ViewBag.ScheduleTitle = $"Нет расписания";
            }

            return View();
        }


        public ActionResult SelectNextSchedule(int scheduleId)
        {
            categories = travelService.GetAllCategories();

            travels = travelService.GetTravelsList(currentUser);
            int? selectedTravel_Id = travelService.GetSelectedTravelId(currentUser);
            if (selectedTravel_Id != null)
                selectedTravel = travels.FirstOrDefault(t => t.Id == selectedTravel_Id);

            ViewBag.User = currentUser;
            ViewBag.Categories = categories;
            ViewBag.Travels = travels;
            ViewBag.SelectedTravel = selectedTravel;

            if (selectedTravel != null && selectedTravel.Schedules?.Any() == true)
            {
                var tempSchedule = selectedTravel.Schedules.FirstOrDefault(x => x.Id == scheduleId);
                int order = selectedTravel.Schedules.OrderBy(x => x.Date).ToList().IndexOf(tempSchedule);

                ViewBag.SelectedScheduleID = tempSchedule.Id;
                ViewBag.Schedule = tempSchedule;
                ViewBag.ScheduleTitle = $"Расписание на {order + 1}-й день"; 
            }
            else
            {
                ViewBag.ScheduleTitle = $"Нет расписания";
            }

            return View("Index");
        }

    }
}