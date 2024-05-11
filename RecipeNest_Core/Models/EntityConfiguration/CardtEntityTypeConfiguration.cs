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
    public class CardtEntityTypeConfiguration : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            // Define the primary key
            builder.HasKey(c => c.CardId);

            // Use identity column for the primary key
            builder.Property(c => c.CardId).UseIdentityColumn();

            // Set properties as required
            builder.Property(d => d.PaymentMethod).IsRequired();
            builder.Property(d => d.Point).IsRequired();
            builder.Property(d => d.Type).IsRequired();
            builder.Property(d => d.Price).IsRequired();

            // Add check constraints for Price, Point, Type, and PaymentMethod
            builder.ToTable(c => c.HasCheckConstraint("CH_Price_Range", "Price IN (10, 15, 20, 30, 40)"));
            builder.ToTable(c => c.HasCheckConstraint("CH_Point_Range", "Point IN (5, 10, 15, 25, 35)"));
            builder.ToTable(c => c.HasCheckConstraint("CH_CardType_Type", "Type IN (10, 11, 12, 13, 14)"));
            builder.ToTable(c => c.HasCheckConstraint("CH_Payment_Method", "PaymentMethod IN (100,101,102)"));

            // Add a unique index for the Type property
            builder.HasIndex(c => c.Type).IsUnique();

            // Set default values for CreationDateTime and IsDeleted
            builder.Property(c => c.CreationDateTime).IsRequired().HasDefaultValue(DateTime.Now);
            builder.Property(c => c.IsDeleted).IsRequired().HasDefaultValue(false);


        }
    }
}
