using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeNest_Core.Migrations
{
    /// <inheritdoc />
    public partial class sd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    CardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Point = table.Column<float>(type: "real", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    CreationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2024, 5, 11, 19, 28, 2, 479, DateTimeKind.Local).AddTicks(1810)),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.CardId);
                    table.CheckConstraint("CH_CardType_Type", "Type IN (10, 11, 12, 13, 14)");
                    table.CheckConstraint("CH_Payment_Method", "PaymentMethod IN (100,101,102)");
                    table.CheckConstraint("CH_Point_Range", "Point IN (5, 10, 15, 25, 35)");
                    table.CheckConstraint("CH_Price_Range", "Price IN (10, 15, 20, 30, 40)");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2024, 5, 11, 19, 28, 2, 514, DateTimeKind.Local).AddTicks(262)),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.CheckConstraint("CHK_Email_Length", "LEN(Email) >= 13 AND LEN(Email) <= 255");
                    table.CheckConstraint("CHK_FirstName_Length", "LEN(FirstName) >= 3 AND LEN(FirstName) <= 255");
                    table.CheckConstraint("CHK_LastName_Length", "LEN(LastName) >= 3 AND LEN(LastName) <= 255");
                    table.CheckConstraint("CK_EmailFormat", "Email LIKE '%_@__%.__%'");
                });

            migrationBuilder.CreateTable(
                name: "Donations",
                columns: table => new
                {
                    DonationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CardId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    CreationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2024, 5, 11, 19, 28, 2, 492, DateTimeKind.Local).AddTicks(3998)),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Donations", x => x.DonationId);
                    table.ForeignKey(
                        name: "FK_Donations_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "CardId");
                    table.ForeignKey(
                        name: "FK_Donations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "FoodSections",
                columns: table => new
                {
                    FoodSectionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    CreationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2024, 5, 11, 19, 28, 2, 503, DateTimeKind.Local).AddTicks(9011)),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodSections", x => x.FoodSectionId);
                    table.ForeignKey(
                        name: "FK_FoodSections_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Logins",
                columns: table => new
                {
                    LoginId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApiKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    CreationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2024, 5, 11, 19, 28, 2, 506, DateTimeKind.Local).AddTicks(5286)),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logins", x => x.LoginId);
                    table.CheckConstraint("CH_Password_Length", "LEN(Password) BETWEEN 8 AND 20");
                    table.ForeignKey(
                        name: "FK_Logins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rating = table.Column<float>(type: "real", nullable: false, defaultValue: 0f),
                    Comment = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    CreationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2024, 5, 11, 19, 28, 2, 510, DateTimeKind.Local).AddTicks(9339)),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewId);
                    table.CheckConstraint("CH_Rating_Range", "Rating >= 0 AND Rating <= 10");
                    table.ForeignKey(
                        name: "FK_Reviews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Dishes",
                columns: table => new
                {
                    DishId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DishImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Steps = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ingredients = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    FoodSectionId = table.Column<int>(type: "int", nullable: true),
                    CreationDateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2024, 5, 11, 19, 28, 2, 487, DateTimeKind.Local).AddTicks(8816)),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes", x => x.DishId);
                    table.CheckConstraint("CHK_Min_Ingredients", "LEN(Ingredients) >= 3");
                    table.CheckConstraint("CHK_Min_Steps", "LEN(Steps) >= 3");
                    table.ForeignKey(
                        name: "FK_Dishes_FoodSections_FoodSectionId",
                        column: x => x.FoodSectionId,
                        principalTable: "FoodSections",
                        principalColumn: "FoodSectionId");
                    table.ForeignKey(
                        name: "FK_Dishes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_Type",
                table: "Cards",
                column: "Type",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_FoodSectionId",
                table: "Dishes",
                column: "FoodSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_UserId",
                table: "Dishes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_CardId",
                table: "Donations",
                column: "CardId",
                unique: true,
                filter: "[CardId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Donations_UserId",
                table: "Donations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodSections_Name",
                table: "FoodSections",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FoodSections_UserId",
                table: "FoodSections",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Logins_UserId",
                table: "Logins",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Logins_Username",
                table: "Logins",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dishes");

            migrationBuilder.DropTable(
                name: "Donations");

            migrationBuilder.DropTable(
                name: "Logins");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "FoodSections");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
