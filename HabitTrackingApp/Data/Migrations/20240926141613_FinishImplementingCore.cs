using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HabitTrackingApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class FinishImplementingCore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "SessionRestartTime",
                table: "AspNetUsers",
                type: "time(0)",
                nullable: false,
                defaultValue: new TimeSpan(0, 6, 0, 0, 0));

            migrationBuilder.CreateTable(
                name: "BadHabits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    CreditsSuccess = table.Column<int>(type: "int", nullable: false),
                    CreditsFail = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BadHabits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BadHabits_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoodHabits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    CreditsSuccess = table.Column<int>(type: "int", nullable: false),
                    CreditsFail = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodHabits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoodHabits_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Treats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    QuantityPerSession = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreditsPrice = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Treats_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_TransactionTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "TransactionTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "TransactionTypes",
                columns: new[] { "Id", "Message" },
                values: new object[,]
                {
                    { 0, "Credits From Unknown Source" },
                    { 1, "Credits From Manual Insertion" },
                    { 2, "Credits From ... AHHHH I DON'T REMEMBER" },
                    { 100, "Credits Earned From Finishing a Good Habit" },
                    { 101, "Credits Lost From Failing a Good Habit" },
                    { 102, "Credits Lost From Change of Status of a Good Habit" },
                    { 200, "Credits Eearned From Quitting a Bad Habit" },
                    { 201, "Credits Lost From Failing To Quit a Bad Habit" },
                    { 202, "Credits Earned From Change Of Status of a Bad Habit" },
                    { 300, "Credits Lost From Buying a Treat. Cheers!" },
                    { 301, "Credits Earned From Refunding a Treat. Uncheers!" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BadHabits_UserId",
                table: "BadHabits",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GoodHabits_UserId",
                table: "GoodHabits",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_TypeId",
                table: "Transactions",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserId",
                table: "Transactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Treats_UserId",
                table: "Treats",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BadHabits");

            migrationBuilder.DropTable(
                name: "GoodHabits");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Treats");

            migrationBuilder.DropTable(
                name: "TransactionTypes");

            migrationBuilder.DropColumn(
                name: "SessionRestartTime",
                table: "AspNetUsers");
        }
    }
}
