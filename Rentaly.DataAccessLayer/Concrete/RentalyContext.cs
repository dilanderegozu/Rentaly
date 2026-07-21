
using Microsoft.EntityFrameworkCore;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.DataAccessLayer.Concrete
{
    public class RentalyContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=Rentaly;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        public DbSet<Branch> Branches { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarModel> CarModels { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<OurFeature> OurFeatures { get; set; }
        public DbSet<Award> Awards { get; set; }
        public DbSet<LatestNew> LatestNews { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<CarImage> CarImages { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Activity> Activities { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.PickUpBranch)
                .WithMany()
                .HasForeignKey(b => b.PickUpBranchId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.DropOffBranch)
                .WithMany()
                .HasForeignKey(b => b.DropOffBranchId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<CarModel>()
    .HasOne(cm => cm.Brand)
    .WithMany()
    .HasForeignKey(cm => cm.BrandId)
    .OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(modelBuilder);
        }
    }

}


