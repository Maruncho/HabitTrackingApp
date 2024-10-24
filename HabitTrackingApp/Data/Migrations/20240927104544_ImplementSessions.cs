using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HabitTrackingApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class ImplementSessions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionRestartTime",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<byte>(
                name: "QuantityPerSession",
                table: "Treats",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)1,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "Credits",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<byte>(
                name: "RefundsPerSession",
                table: "AspNetUsers",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)5);

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Refunds = table.Column<byte>(type: "tinyint", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionBadHabits",
                columns: table => new
                {
                    SessionId = table.Column<int>(type: "int", nullable: false),
                    BadHabitId = table.Column<int>(type: "int", nullable: false),
                    Failed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionBadHabits", x => new { x.SessionId, x.BadHabitId });
                    table.ForeignKey(
                        name: "FK_SessionBadHabits_BadHabits_BadHabitId",
                        column: x => x.BadHabitId,
                        principalTable: "BadHabits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SessionBadHabits_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionGoodHabits",
                columns: table => new
                {
                    SessionId = table.Column<int>(type: "int", nullable: false),
                    GoodHabitId = table.Column<int>(type: "int", nullable: false),
                    Completed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionGoodHabits", x => new { x.SessionId, x.GoodHabitId });
                    table.ForeignKey(
                        name: "FK_SessionGoodHabits_GoodHabits_GoodHabitId",
                        column: x => x.GoodHabitId,
                        principalTable: "GoodHabits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SessionGoodHabits_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionTransactions",
                columns: table => new
                {
                    SessionId = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionTransactions", x => new { x.SessionId, x.TransactionId });
                    table.ForeignKey(
                        name: "FK_SessionTransactions_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SessionTransactions_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionTreats",
                columns: table => new
                {
                    SessionId = table.Column<int>(type: "int", nullable: false),
                    TreatId = table.Column<int>(type: "int", nullable: false),
                    UnitsBought = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionTreats", x => new { x.SessionId, x.TreatId });
                    table.ForeignKey(
                        name: "FK_SessionTreats_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionTreats_Treats_TreatId",
                        column: x => x.TreatId,
                        principalTable: "Treats",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SessionBadHabits_BadHabitId",
                table: "SessionBadHabits",
                column: "BadHabitId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionGoodHabits_GoodHabitId",
                table: "SessionGoodHabits",
                column: "GoodHabitId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_UserId",
                table: "Sessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionTransactions_TransactionId",
                table: "SessionTransactions",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionTreats_TreatId",
                table: "SessionTreats",
                column: "TreatId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessionBadHabits");

            migrationBuilder.DropTable(
                name: "SessionGoodHabits");

            migrationBuilder.DropTable(
                name: "SessionTransactions");

            migrationBuilder.DropTable(
                name: "SessionTreats");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropColumn(
                name: "RefundsPerSession",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "QuantityPerSession",
                table: "Treats",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldDefaultValue: (byte)1);

            migrationBuilder.AlterColumn<int>(
                name: "Credits",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "SessionRestartTime",
                table: "AspNetUsers",
                type: "time(0)",
                nullable: false,
                defaultValue: new TimeSpan(0, 6, 0, 0, 0));
        }
    }
}
