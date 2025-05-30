using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListingsService.Domain.Models
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Listing> Listings { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

           
            modelBuilder.Entity<Listing>(entity =>
            {
                entity.HasKey(e => e.ListingId);
                entity.Property(e => e.ListingId).ValueGeneratedOnAdd(); 

                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.ProductPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Condition).IsRequired();

                entity.HasOne(e => e.Category)
                      .WithMany(c => c.Listings)
                      .HasForeignKey(e => e.CategoryId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

       
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId);
                entity.Property(e => e.CategoryId).ValueGeneratedOnAdd();

                entity.Property(e => e.CategoryName).IsRequired().HasMaxLength(50);
            });

        }
    }
}
