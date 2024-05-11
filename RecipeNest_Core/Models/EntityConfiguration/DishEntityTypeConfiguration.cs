using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeNest_Core.Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeNest_Core.Models.EntityConfiguration
{
    public class DishEntityTypeConfiguration : IEntityTypeConfiguration<Dish>
    {
        public void Configure(EntityTypeBuilder<Dish> builder)
        {
            // Define the primary key
            builder.HasKey(d => d.DishId);

            // Use identity column for the primary key
            builder.Property(d => d.DishId).UseIdentityColumn();

            // Set properties as required
            builder.Property(d => d.DishImagePath).IsRequired(false);
            builder.Property(d => d.Name).IsRequired();
            builder.Property(d => d.Description).IsRequired();

            // Configure custom conversion and value comparer for storing and retrieving Steps and Ingredients data
            builder.Property(d => d.Steps).IsRequired().HasConversion(
                v => string.Join(",", v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList())
                .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

            // Configure custom conversion and value comparer for storing and retrieving Steps and Ingredients data
            builder.Property(d => d.Ingredients).IsRequired().HasConversion(
                v => string.Join(",", v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList())
                .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

            // Add check constraints for Steps and Ingredients
            builder.ToTable(d => d.HasCheckConstraint("CHK_Min_Steps", "LEN(Steps) >= 3"));
            builder.ToTable(d => d.HasCheckConstraint("CHK_Min_Ingredients", "LEN(Ingredients) >= 3"));

            builder.Property(d => d.Name).IsRequired().HasMaxLength(255);
            builder.Property(d => d.Description).HasMaxLength(1000);

            //// Add a unique index for the Type property
            //builder.HasIndex(d => d.Name).IsUnique();

            // Set default values for CreationDateTime and IsDeleted
            builder.Property(d => d.CreationDateTime).IsRequired().HasDefaultValue(DateTime.Now);
            builder.Property(d => d.IsDeleted).IsRequired().HasDefaultValue(false);

            // Define the relationship with the FoodSection entity
            builder.HasOne(fs => fs.FoodSection).WithMany(d => d.Dishes).OnDelete(DeleteBehavior.NoAction);

            // Define the relationship with the User entity
            builder.HasOne(u => u.User).WithMany(d => d.Dishes).HasForeignKey(d => d.UserId).OnDelete(DeleteBehavior.NoAction);

        }
    }
}
