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
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Define the primary key
            builder.HasKey(u => u.UserId);

            // Use identity column for the primary key
            builder.Property(u => u.UserId).UseIdentityColumn();

            // Set properties as required
            builder.Property(u => u.ProfileImagePath).IsRequired(false);
            builder.Property(u => u.FirstName).IsRequired();
            builder.Property(u => u.LastName).IsRequired();
            builder.Property(u => u.Email).IsRequired();

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(255); // Set maximum length for FirstName

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(255); // Set maximum length for LastName

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255); // Set maximum length for Email

            // Add a check constraint for FirstName
            builder.ToTable(u => u.HasCheckConstraint("CHK_FirstName_Length", "LEN(FirstName) >= 3 AND LEN(FirstName) <= 255"));

            // Add a check constraint for LastName
            builder.ToTable(u => u.HasCheckConstraint("CHK_LastName_Length", "LEN(LastName) >= 3 AND LEN(LastName) <= 255"));

            // Add a check constraint for Email Length
            builder.ToTable(u => u.HasCheckConstraint("CHK_Email_Length", "LEN(Email) >= 13 AND LEN(Email) <= 255"));

            // Add a check constraint for Email Format
            builder.ToTable(u => u.HasCheckConstraint("CK_EmailFormat", "Email LIKE '%_@__%.__%'"));

            // Add a unique index for the Type property
            builder.HasIndex(u => u.Email).IsUnique();

            // Set default values for CreationDateTime, IsDeleted and IsAdmin
            builder.Property(u => u.CreationDateTime).IsRequired().HasDefaultValue(DateTime.Now);
            builder.Property(u => u.IsAdmin).IsRequired().HasDefaultValue(0);
            builder.Property(u => u.IsDeleted).IsRequired().HasDefaultValue(false);

        }
    }
}
