﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RecipeNest_Core.Models.Context;

#nullable disable

namespace RecipeNest_Core.Migrations
{
    [DbContext(typeof(RecipeNestDbContext))]
    partial class RecipeNestDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.18")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RecipeNest_Core.Models.Entites.Card", b =>
                {
                    b.Property<int>("CardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CardId"));

                    b.Property<DateTime>("CreationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2024, 5, 11, 19, 28, 2, 479, DateTimeKind.Local).AddTicks(1810));

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int>("PaymentMethod")
                        .HasColumnType("int");

                    b.Property<float>("Point")
                        .HasColumnType("real");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("CardId");

                    b.HasIndex("Type")
                        .IsUnique();

                    b.ToTable("Cards", t =>
                        {
                            t.HasCheckConstraint("CH_CardType_Type", "Type IN (10, 11, 12, 13, 14)");

                            t.HasCheckConstraint("CH_Payment_Method", "PaymentMethod IN (100,101,102)");

                            t.HasCheckConstraint("CH_Point_Range", "Point IN (5, 10, 15, 25, 35)");

                            t.HasCheckConstraint("CH_Price_Range", "Price IN (10, 15, 20, 30, 40)");
                        });
                });

            modelBuilder.Entity("RecipeNest_Core.Models.Entites.Dish", b =>
                {
                    b.Property<int>("DishId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DishId"));

                    b.Property<DateTime>("CreationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2024, 5, 11, 19, 28, 2, 487, DateTimeKind.Local).AddTicks(8816));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("DishImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FoodSectionId")
                        .HasColumnType("int");

                    b.Property<string>("Ingredients")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Steps")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("DishId");

                    b.HasIndex("FoodSectionId");

                    b.HasIndex("UserId");

                    b.ToTable("Dishes", t =>
                        {
                            t.HasCheckConstraint("CHK_Min_Ingredients", "LEN(Ingredients) >= 3");

                            t.HasCheckConstraint("CHK_Min_Steps", "LEN(Steps) >= 3");
                        });
                });

            modelBuilder.Entity("RecipeNest_Core.Models.Entites.Donation", b =>
                {
                    b.Property<int>("DonationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DonationId"));

                    b.Property<int?>("CardId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2024, 5, 11, 19, 28, 2, 492, DateTimeKind.Local).AddTicks(3998));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("DonationId");

                    b.HasIndex("CardId")
                        .IsUnique()
                        .HasFilter("[CardId] IS NOT NULL");

                    b.HasIndex("UserId");

                    b.ToTable("Donations");
                });

            modelBuilder.Entity("RecipeNest_Core.Models.Entites.FoodSection", b =>
                {
                    b.Property<int>("FoodSectionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FoodSectionId"));

                    b.Property<DateTime>("CreationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2024, 5, 11, 19, 28, 2, 503, DateTimeKind.Local).AddTicks(9011));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("FoodSectionId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("FoodSections");
                });

            modelBuilder.Entity("RecipeNest_Core.Models.Entites.Login", b =>
                {
                    b.Property<int>("LoginId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LoginId"));

                    b.Property<string>("ApiKey")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2024, 5, 11, 19, 28, 2, 506, DateTimeKind.Local).AddTicks(5286));

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("LoginId");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Logins", t =>
                        {
                            t.HasCheckConstraint("CH_Password_Length", "LEN(Password) BETWEEN 8 AND 20");
                        });
                });

            modelBuilder.Entity("RecipeNest_Core.Models.Entites.Review", b =>
                {
                    b.Property<int>("ReviewId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReviewId"));

                    b.Property<string>("Comment")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("CreationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2024, 5, 11, 19, 28, 2, 510, DateTimeKind.Local).AddTicks(9339));

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<float>("Rating")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("real")
                        .HasDefaultValue(0f);

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ReviewId");

                    b.HasIndex("UserId");

                    b.ToTable("Reviews", t =>
                        {
                            t.HasCheckConstraint("CH_Rating_Range", "Rating >= 0 AND Rating <= 10");
                        });
                });

            modelBuilder.Entity("RecipeNest_Core.Models.Entites.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<DateTime>("CreationDateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2024, 5, 11, 19, 28, 2, 514, DateTimeKind.Local).AddTicks(262));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("IsAdmin")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ProfileImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users", t =>
                        {
                            t.HasCheckConstraint("CHK_Email_Length", "LEN(Email) >= 13 AND LEN(Email) <= 255");

                            t.HasCheckConstraint("CHK_FirstName_Length", "LEN(FirstName) >= 3 AND LEN(FirstName) <= 255");

                            t.HasCheckConstraint("CHK_LastName_Length", "LEN(LastName) >= 3 AND LEN(LastName) <= 255");

                            t.HasCheckConstraint("CK_EmailFormat", "Email LIKE '%_@__%.__%'");
                        });
                });

            modelBuilder.Entity("RecipeNest_Core.Models.Entites.Dish", b =>
                {
                    b.HasOne("RecipeNest_Core.Models.Entites.FoodSection", "FoodSection")
                        .WithMany("Dishes")
                        .HasForeignKey("FoodSectionId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("RecipeNest_Core.Models.Entites.User", "User")
                        .WithMany("Dishes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("FoodSection");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RecipeNest_Core.Models.Entites.Donation", b =>
                {
                    b.HasOne("RecipeNest_Core.Models.Entites.Card", "Card")
                        .WithOne("Donation")
                        .HasForeignKey("RecipeNest_Core.Models.Entites.Donation", "CardId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("RecipeNest_Core.Models.Entites.User", "User")
                        .WithMany("Donations")
                        .HasForeignKey("UserId");

                    b.Navigation("Card");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RecipeNest_Core.Models.Entites.FoodSection", b =>
                {
                    b.HasOne("RecipeNest_Core.Models.Entites.User", "User")
                        .WithMany("FoodSections")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RecipeNest_Core.Models.Entites.Login", b =>
                {
                    b.HasOne("RecipeNest_Core.Models.Entites.User", "User")
                        .WithOne("Login")
                        .HasForeignKey("RecipeNest_Core.Models.Entites.Login", "UserId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("User");
                });

            modelBuilder.Entity("RecipeNest_Core.Models.Entites.Review", b =>
                {
                    b.HasOne("RecipeNest_Core.Models.Entites.User", "User")
                        .WithMany("Reviews")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RecipeNest_Core.Models.Entites.Card", b =>
                {
                    b.Navigation("Donation");
                });

            modelBuilder.Entity("RecipeNest_Core.Models.Entites.FoodSection", b =>
                {
                    b.Navigation("Dishes");
                });

            modelBuilder.Entity("RecipeNest_Core.Models.Entites.User", b =>
                {
                    b.Navigation("Dishes");

                    b.Navigation("Donations");

                    b.Navigation("FoodSections");

                    b.Navigation("Login");

                    b.Navigation("Reviews");
                });
#pragma warning restore 612, 618
        }
    }
}