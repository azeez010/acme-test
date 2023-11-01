using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AcmeCorp.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace AcmeCorp.Infrastructure.DataContext
{
    
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet properties for your models
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(c => c.CustomerId);
                entity.HasIndex(c => c.CustomerId).IsUnique();
                entity.Property(c => c.CustomerId).ValueGeneratedOnAdd();
            });
            // Define relationships between entities
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.ContactInfo)
                .WithOne(ci => ci.Customer)
                .HasForeignKey(ci => ci.CustomerId);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId);

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(c => c.OrderId);
                entity.HasIndex(c => c.OrderId).IsUnique();
                entity.Property(c => c.OrderId).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<ContactInfo>(entity =>
            {
                entity.HasKey(c => c.ContactInfoId);
                entity.HasIndex(c => c.ContactInfoId).IsUnique();
                entity.Property(c => c.ContactInfoId).ValueGeneratedOnAdd();
            });
        }
    }

}
