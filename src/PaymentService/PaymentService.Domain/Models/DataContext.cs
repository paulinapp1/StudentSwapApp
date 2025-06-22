using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Domain.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<PaymentTransaction> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaymentTransaction>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.PurchaseId)
                    .IsRequired();

                entity.Property(e => e.UserId)
                    .IsRequired();

                entity.Property(e => e.CardProvider)
                    .HasConversion<int>()
                    .IsRequired();

                entity.Property(e => e.CardLast4)
                    .HasMaxLength(4)
                    .IsRequired();

                entity.Property(e => e.Status)
                    .HasConversion<int>()
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .IsRequired();

                entity.Property(e => e.CompletedAt)
                    .IsRequired(false);


            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
