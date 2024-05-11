using Microsoft.EntityFrameworkCore;
using RecipeNest_Core.Models.Entites;
using RecipeNest_Core.Models.EntityConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeNest_Core.Models.Context
{
    public class RecipeNestDbContext : DbContext
    {
        public RecipeNestDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CardtEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DishEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DonationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FoodSectionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LoginEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ReviewEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
        }

        public virtual DbSet<Card> Cards { get; set; }
        public virtual DbSet<Dish> Dishes { get; set; }
        public virtual DbSet<Donation> Donations { get; set; }
        public virtual DbSet<FoodSection> FoodSections { get; set; }
        public virtual DbSet<Login> Logins { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
