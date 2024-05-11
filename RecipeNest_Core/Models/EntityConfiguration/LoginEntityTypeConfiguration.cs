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
    public class LoginEntityTypeConfiguration : IEntityTypeConfiguration<Login>
    {
        public void Configure(EntityTypeBuilder<Login> builder)
        {
            // Define the primary key
            builder.HasKey(l => l.LoginId);

            // Use identity column for the primary key
            builder.Property(l => l.LoginId).UseIdentityColumn();

            // Set properties as required
            builder.Property(l => l.Password).IsRequired();
            builder.Property(l => l.ApiKey).IsRequired(false);

            // Add check constraints for Password
            builder.ToTable(l => l.HasCheckConstraint("CH_Password_Length", "LEN(Password) BETWEEN 8 AND 20"));

            builder.Property(l => l.Username).IsRequired()
                .HasMaxLength(255);// Set required and maximum length for Username

            // Add a unique index for the Type property
            builder.HasIndex(l => l.Username).IsUnique();

            // Set default values for CreationDateTime and IsDeleted
            builder.Property(l => l.CreationDateTime).IsRequired().HasDefaultValue(DateTime.Now);
            builder.Property(l => l.IsDeleted).IsRequired().HasDefaultValue(false);

            // Define the relationship with the User entity
            builder.HasOne(u => u.User).WithOne(l => l.Login).HasForeignKey("Login", "UserId").OnDelete(DeleteBehavior.NoAction);
        }
    }
}
