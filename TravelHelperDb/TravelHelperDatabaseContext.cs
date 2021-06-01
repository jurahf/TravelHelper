using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TravelHelperDb
{
    public partial class TravelHelperDatabaseContext : DbContext
    {
        public TravelHelperDatabaseContext()
        {
        }

        public TravelHelperDatabaseContext(DbContextOptions<TravelHelperDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CategorySet> CategorySet { get; set; }
        public virtual DbSet<CitySet> CitySet { get; set; }
        public virtual DbSet<Geo> Geo { get; set; }
        public virtual DbSet<NaviAddressInfoSet> NaviAddressInfoSet { get; set; }
        public virtual DbSet<PlacePointSet> PlacePointSet { get; set; }
        public virtual DbSet<ScheduleSet> ScheduleSet { get; set; }
        public virtual DbSet<TravelCategory> TravelCategory { get; set; }
        public virtual DbSet<TravelSet> TravelSet { get; set; }
        public virtual DbSet<UserSet> UserSet { get; set; }
        public virtual DbSet<UserSettingsSet> UserSettingsSet { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //optionsBuilder.UseNpgsql("data source=(local);initial catalog=TravelHelperDatabase;integrated security=True;MultipleActiveResultSets=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
