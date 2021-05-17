using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreImplementation.Model;
using TravelHelperDb;

namespace Services.ModelsTools
{
    public static class ToVmConverter
    {
        public static VMCategory ConvertToVm(this CategorySet category)
        {
            if (category == null)
                return null;

            return new VMCategory()
            {
                Id = category.Id,
                NaviId = category.NaviId,
                Name = category.Name,
                Parent = ConvertToVm(category.Parent),
                // Childs - заполняется в месте вызова при необходимости, потому что там есть все категории
            };
        }


        public static VMTravel ConvertToVm(this TravelSet travel)
        {
            if (travel == null)
                return null;

            return new VMTravel()
            {
                Id = travel.Id,
                Name = travel.Name,
                StartDate = travel.StartDate,
                EndDate = travel.EndDate,
                Categories = travel.TravelCategory.Select(x => x.Categories).Select(x => x.ConvertToVm()).ToList(),
                City = travel.City.ConvertToVm(),
                User = travel.User.ConvertToVm(),
                Schedules = travel.ScheduleSet.Select(x => x.ConvertToVm()).ToList()
            };
        }


        public static VMCity ConvertToVm(this CitySet city)
        {
            if (city == null)
                return null;

            return new VMCity()
            {
                Id = city.Id,
                Name = city.Name,
                Country = city.Country,
                Lat = city.Lat,
                Lng = city.Lng
            };
        }


        public static VMUser ConvertToVm(this UserSet user)
        {
            if (user == null)
                return null;

            return new VMUser()
            {
                Id = user.Id,
                Login = user.Login
            };
        }


        public static VMSchedule ConvertToVm(this ScheduleSet schedule)
        {
            if (schedule == null)
                return null;

            return new VMSchedule()
            {
                Id = schedule.Id,
                Date = schedule.Date,
                TempPoint = schedule.TempPoint,
                PlacePoint = schedule.PlacePointSet.Select(x => x.ConvertToVm()).ToList()
            };
        }


        public static VMPlacePoint ConvertToVm(this PlacePointSet placePoint)
        {
            if (placePoint == null)
                return null;

            return new VMPlacePoint()
            {
                Id = placePoint.Id,
                CustomName = placePoint.CustomName,
                Order = placePoint.Order,
                Time = placePoint.Time,
                AddressInfo = placePoint.NaviAddressInfo.ConvertToVm()
            };
        }


        public static VMAddressInfo ConvertToVm(this NaviAddressInfoSet address)
        {
            if (address == null)
                return null;

            return new VMAddressInfo()
            {
                Id = address.Id,
                Category = address.Category.ConvertToVm(),
                ContainerAddress = address.ContainerAddress,
                Description = address.Description,
                Latitude = address.Latitude,
                Longitude = address.Longitude,
                Picture = address.Picture,
                SelfAddress = address.SelfAddress
            };
        }


    }
}
