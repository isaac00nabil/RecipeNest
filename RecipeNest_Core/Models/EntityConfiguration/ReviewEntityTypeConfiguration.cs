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
    public class ReviewEntityTypeConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            // Define the primary key
            builder.HasKey(r => r.ReviewId);

            // Use identity column for the primary key
            builder.Property(r => r.ReviewId).UseIdentityColumn();

            // Set properties as required
            builder.Property(r => r.Rating).IsRequired();
            builder.Property(r => r.Comment).IsRequired(false)
                .HasMaxLength(255);// Set maximum length for Comment

            // Add check constraints for Rating
            builder.ToTable(r => r.HasCheckConstraint("CH_Rating_Range", "Rating >= 0 AND Rating <= 10"));

            // Set default values for CreationDateTime, IsDeleted and Rating
            builder.Property(r => r.Rating).HasDefaultValue(0);
            builder.Property(r => r.CreationDateTime).IsRequired().HasDefaultValue(DateTime.Now);
            builder.Property(r => r.IsDeleted).IsRequired().HasDefaultValue(false);
        }
    }
}
