﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Server.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class NaviTravelModelContainer : DbContext
    {
        public NaviTravelModelContainer()
            : base("name=NaviTravelModelContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<User> UserSet { get; set; }
        public virtual DbSet<Travel> TravelSet { get; set; }
        public virtual DbSet<Schedule> ScheduleSet { get; set; }
        public virtual DbSet<PlacePoint> PlacePointSet { get; set; }
        public virtual DbSet<Category> CategorySet { get; set; }
        public virtual DbSet<NaviAddressInfo> NaviAddressInfoSet { get; set; }
        public virtual DbSet<City> CitySet { get; set; }
        public virtual DbSet<UserSettings> UserSettingsSet { get; set; }
    }
}
