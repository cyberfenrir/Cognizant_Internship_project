using Microsoft.EntityFrameworkCore;
using RxMed.Models;

namespace RxMed.Data
{
    public class ApiDbContext : DbContext 
    {

        public DbSet<User> Users { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Medicine> Medicines { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Role> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database = RxMed;Trusted_Connection=True;Encrypt=False;"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //1st relation
            //modelBuilder.Entity<User>() 
            //    .HasOne(u => u.DefaultAddress)
            //    .WithMany()
            //    .HasForeignKey(u => u.default_address_id)
            //    .OnDelete(DeleteBehavior.Restrict);
            
            //2nd relation
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.user_id)
                .OnDelete(DeleteBehavior.Cascade);
            
            //3rd relation
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Address)
                .WithMany()
                .HasForeignKey(o => o.address_id)
                .OnDelete(DeleteBehavior.Restrict);

            //4th relation
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.order_id)
                .OnDelete(DeleteBehavior.Cascade);

            //5th relation
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Medicine)
                .WithMany(m => m.OrderDetails)
                .HasForeignKey(od => od.med_id)
                .OnDelete(DeleteBehavior.Restrict);

            //6th relation
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Medicine)
                .WithMany(m => m.Reviews)
                .HasForeignKey(r => r.med_id)
                .OnDelete(DeleteBehavior.Restrict);

            //7th relation
            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.user_id)
                .OnDelete(DeleteBehavior.Restrict);

            //8th relation
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.role_id)
                .OnDelete(DeleteBehavior.Restrict);

           //9th relation
           modelBuilder.Entity<Address>()
                .HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.user_id);
        }

    }
}
