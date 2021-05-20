using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TravelHelperDb;

namespace Services.ModelsTools
{
    public static class FetchExtensions
    {
        public static IQueryable<TravelSet> Fetch(this DbSet<TravelSet> travel)
        {
            return travel.Include(x => x.City)
                .Include(x => x.TravelCategory)
                .ThenInclude(y => y.Categories)
                .Include(x => x.ScheduleSet)
                .ThenInclude(y => y.PlacePointSet)
                .ThenInclude(z => z.NaviAddressInfo);
        }


        public static IQueryable<ScheduleSet> Fetch(this DbSet<ScheduleSet> schedule)
        {
            return schedule
                .Include(y => y.PlacePointSet)
                .ThenInclude(z => z.NaviAddressInfo);
        }

    }
}
