using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeNest_Core.Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeNest_Core.Models.EntityConfiguration
{
    public class FoodSectionEntityTypeConfiguration : IEntityTypeConfiguration<FoodSection>
    {
        public void Configure(EntityTypeBuilder<FoodSection> builder)
        {
            // Define the primary key
            builder.HasKey(fs => fs.FoodSectionId);

            // Use identity column for the primary key
            builder.Property(fs => fs.FoodSectionId).UseIdentityColumn();

            builder.Property(fs => fs.Name).IsRequired()
                .HasMaxLength(255);// Set maximum length for Name

            builder.Property(fs => fs.Description)
                .HasMaxLength(1000);// Set maximum length for Description

            // Add a unique index for the Name property
            builder.HasIndex(fs => fs.Name).IsUnique();

            // Set default values for CreationDateTime and IsDeleted
            builder.Property(fs => fs.CreationDateTime).IsRequired().HasDefaultValue(DateTime.Now);
            builder.Property(fs => fs.IsDeleted).IsRequired().HasDefaultValue(false);
        }
    }
}
