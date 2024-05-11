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
    public class DonationEntityTypeConfiguration : IEntityTypeConfiguration<Donation>
    {
        public void Configure(EntityTypeBuilder<Donation> builder)
        {
            // Define the primary key
            builder.HasKey(d => d.DonationId);

            // Use identity column for the primary key
            builder.Property(d => d.DonationId).UseIdentityColumn();

            builder.Property(d => d.Description)
                .HasMaxLength(1000);// Set maximum length for Description

            // Set default values for CreationDateTime and IsDeleted
            builder.Property(d => d.CreationDateTime).IsRequired().HasDefaultValue(DateTime.Now);
            builder.Property(d => d.IsDeleted).IsRequired().HasDefaultValue(false);

            // Define the relationship with the Donation entity
            builder.HasOne(d => d.Card).WithOne(c => c.Donation).HasForeignKey("Donation", "CardId").OnDelete(DeleteBehavior.NoAction);
        }
    }
}
