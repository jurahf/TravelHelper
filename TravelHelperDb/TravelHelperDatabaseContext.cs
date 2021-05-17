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
                optionsBuilder.UseSqlServer("data source=(local);initial catalog=TravelHelperDatabase;integrated security=True;MultipleActiveResultSets=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategorySet>(entity =>
            {
                entity.HasIndex(e => e.ParentId)
                    .HasName("IX_FK_CategoryCategory");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.ParentId).HasColumnName("Parent_Id");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_CategoryCategory");
            });

            modelBuilder.Entity<CitySet>(entity =>
            {
                entity.Property(e => e.Lat).HasColumnType("decimal(18, 10)");

                entity.Property(e => e.Lng).HasColumnType("decimal(18, 10)");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Geo>(entity =>
            {
                entity.ToTable("geo");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasColumnName("city")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CityEn)
                    .IsRequired()
                    .HasColumnName("city_en")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasColumnName("country")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CountryEn)
                    .IsRequired()
                    .HasColumnName("country_en")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Lat)
                    .IsRequired()
                    .HasColumnName("lat")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Lng)
                    .IsRequired()
                    .HasColumnName("lng")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Population)
                    .HasColumnName("population")
                    .HasDefaultValueSql("('0')");

                entity.Property(e => e.Region)
                    .IsRequired()
                    .HasColumnName("region")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RegionEn)
                    .IsRequired()
                    .HasColumnName("region_en")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<NaviAddressInfoSet>(entity =>
            {
                entity.HasIndex(e => e.CategoryId)
                    .HasName("IX_FK_CategoryNaviAddressInfo");

                entity.HasIndex(e => e.CityId)
                    .HasName("IX_FK_CityNaviAddressInfo");

                entity.Property(e => e.CategoryId).HasColumnName("Category_Id");

                entity.Property(e => e.CityId).HasColumnName("City_Id");

                entity.Property(e => e.Latitude).HasColumnType("decimal(18, 10)");

                entity.Property(e => e.Longitude).HasColumnType("decimal(18, 10)");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.NaviAddressInfoSet)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_CategoryNaviAddressInfo");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.NaviAddressInfoSet)
                    .HasForeignKey(d => d.CityId)
                    .HasConstraintName("FK_CityNaviAddressInfo");
            });

            modelBuilder.Entity<PlacePointSet>(entity =>
            {
                entity.HasIndex(e => e.NaviAddressInfoId)
                    .HasName("IX_FK_NaviAddressInfoPlacePoint");

                entity.HasIndex(e => e.ScheduleId)
                    .HasName("IX_FK_SchedulePlacePoint");

                entity.Property(e => e.CustomName).IsRequired();

                entity.Property(e => e.NaviAddressInfoId).HasColumnName("NaviAddressInfo_Id");

                entity.Property(e => e.ScheduleId).HasColumnName("Schedule_Id");

                entity.Property(e => e.Time).HasColumnType("datetime");

                entity.HasOne(d => d.NaviAddressInfo)
                    .WithMany(p => p.PlacePointSet)
                    .HasForeignKey(d => d.NaviAddressInfoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NaviAddressInfoPlacePoint");

                entity.HasOne(d => d.Schedule)
                    .WithMany(p => p.PlacePointSet)
                    .HasForeignKey(d => d.ScheduleId)
                    .HasConstraintName("FK_SchedulePlacePoint");
            });

            modelBuilder.Entity<ScheduleSet>(entity =>
            {
                entity.HasIndex(e => e.TravelId)
                    .HasName("IX_FK_TravelSchedule");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.TravelId).HasColumnName("Travel_Id");

                entity.HasOne(d => d.Travel)
                    .WithMany(p => p.ScheduleSet)
                    .HasForeignKey(d => d.TravelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TravelSchedule");
            });

            modelBuilder.Entity<TravelCategory>(entity =>
            {
                entity.HasKey(e => new { e.TravelCategoryCategoryId, e.CategoriesId });

                entity.HasIndex(e => e.CategoriesId)
                    .HasName("IX_FK_TravelCategory_Category");

                entity.Property(e => e.TravelCategoryCategoryId).HasColumnName("TravelCategory_Category_Id");

                entity.Property(e => e.CategoriesId).HasColumnName("Categories_Id");

                entity.HasOne(d => d.Categories)
                    .WithMany(p => p.TravelCategory)
                    .HasForeignKey(d => d.CategoriesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TravelCategory_Category");

                entity.HasOne(d => d.TravelCategoryCategory)
                    .WithMany(p => p.TravelCategory)
                    .HasForeignKey(d => d.TravelCategoryCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TravelCategory_Travel");
            });

            modelBuilder.Entity<TravelSet>(entity =>
            {
                entity.HasIndex(e => e.CityId)
                    .HasName("IX_FK_CityTravel");

                entity.HasIndex(e => e.UserId)
                    .HasName("IX_FK_UserTravel");

                entity.Property(e => e.CityId).HasColumnName("City_Id");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("User_Id");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.TravelSet)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CityTravel");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TravelSet)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserTravel");
            });

            modelBuilder.Entity<UserSet>(entity =>
            {
                entity.HasIndex(e => e.UserSettingsId)
                    .HasName("IX_FK_UserSettingsUser");

                entity.Property(e => e.Login).IsRequired();

                entity.Property(e => e.UserSettingsId).HasColumnName("UserSettings_Id");

                entity.HasOne(d => d.UserSettings)
                    .WithMany(p => p.UserSet)
                    .HasForeignKey(d => d.UserSettingsId)
                    .HasConstraintName("FK_UserSettingsUser");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
