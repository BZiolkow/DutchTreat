﻿using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DutchTreat.Data
{
    public class DutchContext : IdentityDbContext<StoreUser>
    {
        private readonly IConfiguration _config;
        public DutchContext(DbContextOptions<DutchContext> options): base(options)
        {

        }
        public DbSet<Product>? Products { get; set; }
        public DbSet<Order>? Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .HasData(new Order()
                {
                    Id = 1,
                    OrderDate = DateTime.UtcNow,
                    OrderNumber = "1234"
                });
        }
    }
}   
