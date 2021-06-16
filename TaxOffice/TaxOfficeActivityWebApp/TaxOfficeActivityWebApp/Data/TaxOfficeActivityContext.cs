using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TaxOfficeActivityWebApp
{
    public partial class TaxOfficeActivityContext : DbContext
    {
        public TaxOfficeActivityContext()
        {
        }

        public TaxOfficeActivityContext(DbContextOptions<TaxOfficeActivityContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<ActivityAccounting> ActivitiesAccounting { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<Entrepreneur> Entrepreneurs { get; set; }
        public virtual DbSet<TaxOffice> TaxOffices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TaxOfficeActivity;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activity>(entity =>
            {
                entity.HasKey(e => e.ActivityId)
                    .HasName("PK__Activiti__0FC9CBECCE5CE532");

                entity.Property(e => e.ActivityId).HasColumnName("activityId");

                entity.Property(e => e.ActivityName)
                    .HasColumnName("activityName")
                    .HasMaxLength(150)
                    .IsUnicode(true);

                entity.Property(e => e.Tax)
                    .HasColumnName("tax")
                    .HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<ActivityAccounting>(entity =>
            {
                entity.HasKey(e => e.ActivityAccountingId)
                    .HasName("PK__Activiti__CF991E826EB7CED7");

                entity.Property(e => e.ActivityAccountingId).HasColumnName("activityAccountingId");

                entity.Property(e => e.ActivityId).HasColumnName("activityId");

                entity.Property(e => e.EntrepreneurId).HasColumnName("entrepreneurId");

                entity.Property(e => e.Income)
                    .HasColumnName("income")
                    .HasColumnType("decimal(18, 0)");

                entity.Property(e => e.IsTaxPaid).HasColumnName("isTaxPaid");

                entity.Property(e => e.Quarter).HasColumnName("quarter");

                entity.Property(e => e.Year).HasColumnName("year");

                entity.HasOne(d => d.Activity)
                    .WithMany(p => p.ActivitiesAccounting)
                    .HasForeignKey(d => d.ActivityId)
                    .HasConstraintName("FK__Activitie__activ__2E1BDC42");

                entity.HasOne(d => d.Entrepreneur)
                    .WithMany(p => p.ActivitiesAccounting)
                    .HasForeignKey(d => d.EntrepreneurId)
                    .HasConstraintName("FK__Activitie__entre__2D27B809");
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.HasKey(e => e.DistrictId)
                    .HasName("PK__District__2BAEF7107C664830");

                entity.Property(e => e.DistrictId).HasColumnName("districtId");

                entity.Property(e => e.DistrictName)
                    .HasColumnName("districtName")
                    .HasMaxLength(150)
                    .IsUnicode(true);
            });

            modelBuilder.Entity<Entrepreneur>(entity =>
            {
                entity.HasKey(e => e.EntrepreneurId)
                    .HasName("PK__Entrepre__2F5F230E160615DE");

                entity.Property(e => e.EntrepreneurId).HasColumnName("entrepreneurId");

                entity.Property(e => e.FullName)
                    .HasColumnName("fullName")
                    .HasMaxLength(150)
                    .IsUnicode(true);

                entity.Property(e => e.TaxOfficeId).HasColumnName("taxOfficeId");

                entity.HasOne(d => d.TaxOffice)
                    .WithMany(p => p.Entrepreneurs)
                    .HasForeignKey(d => d.TaxOfficeId)
                    .HasConstraintName("FK__Entrepren__taxOf__2B3F6F97");
            });

            modelBuilder.Entity<TaxOffice>(entity =>
            {
                entity.HasKey(e => e.TaxOfficeId)
                    .HasName("PK__TaxOffic__DA5B3A11A7B188CA");

                entity.Property(e => e.TaxOfficeId).HasColumnName("taxOfficeId");

                entity.Property(e => e.DistrictId).HasColumnName("districtId");

                entity.Property(e => e.TaxOfficeName)
                    .HasColumnName("taxOfficeName")
                    .HasMaxLength(150)
                    .IsUnicode(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
