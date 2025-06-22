using Microsoft.EntityFrameworkCore;
using PurchaseService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseService.Domain.Models
{

    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<PurchaseModel> Purchases { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {



            modelBuilder.Entity<PurchaseModel>(entity =>
            {
                entity.HasKey(e => e.PurchaseId);
                entity.Property(e => e.PurchaseId).ValueGeneratedOnAdd();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
                entity.Property(e => e.status)
  .HasDefaultValue(Status.CREATED)
  .HasConversion<string>();
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("NOW()");
                entity.Property(e => e.BuyerId).IsRequired();
                entity.Property(e => e.SellerId).IsRequired();
                entity.Property(e => e.Price).IsRequired();
                entity.Property(e => e.Street).IsRequired().HasMaxLength(200);
                entity.Property(e => e.BuyerNumber).IsRequired();
                entity.Property(e => e.City).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Country).IsRequired().HasMaxLength(100);



            });



        }
    }
}
