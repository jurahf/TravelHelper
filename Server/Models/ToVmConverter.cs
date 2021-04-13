using Implementation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    public static class ToVmConverter
    {
        public static VMCategory ConvertToVm(this Category category)
        {
            if (category == null)
                return null;

            return new VMCategory()
            {
                Id = category.Id,
                NaviId = category.NaviId,
                Name = category.Name,
                Parent = ConvertToVm(category.Parent),
                // Childs - TODO: сейчас заполняется где-то на клиенте (TravelRemoteService)
            };
        }


        public static VMTravel ConvertToVm(this Travel travel)
        {
            if (travel == null)
                return null;

            return new VMTravel()
            {
                Id = travel.Id,
                Name = travel.Name,
                StartDate = travel.StartDate,
                EndDate = travel.EndDate,
                Categories = travel.Categories.Select(x => x.ConvertToVm()).ToList(),
                City = travel.City.ConvertToVm(),
                User = travel.User.ConvertToVm(),
                Schedules = travel.Schedules.Select(x => x.ConvertToVm()).ToList()
            };
        }


        public static VMCity ConvertToVm(this City city)
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


        public static VMUser ConvertToVm(this User user)
        {
            if (user == null)
                return null;

            return new VMUser()
            {
                Id = user.Id,
                Login = user.Login
            };
        }


        public static VMSchedule ConvertToVm(this Schedule schedule)
        {
            if (schedule == null)
                return null;

            return new VMSchedule()
            {
                Id = schedule.Id,
                Date = schedule.Date,
                TempPoint = schedule.TempPoint,
                PlacePoint = schedule.PlacePoint.Select(x => x.ConvertToVm()).ToList()
            };
        }


        public static VMPlacePoint ConvertToVm(this PlacePoint placePoint)
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


        public static VMAddressInfo ConvertToVm(this NaviAddressInfo address)
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
