using Microsoft.EntityFrameworkCore;
using UsersService.Domain.Models;

namespace UsersService.Domain
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }


        public DbSet<UserModel> Users { get; set; }
   

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
 
           modelBuilder.Entity<UserModel>().HasKey(u => u.Id);

      

           
        }
    }
}
