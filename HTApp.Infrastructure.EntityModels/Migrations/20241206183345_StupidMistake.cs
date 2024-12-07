using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HTApp.Infrastructure.EntityModels.Migrations
{
    /// <inheritdoc />
    public partial class StupidMistake : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessionTransactions");

            migrationBuilder.RenameColumn(
                name: "Current",
                table: "Sessions",
                newName: "Last");

            migrationBuilder.AddColumn<int>(
                name: "SessionId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SessionId",
                table: "Transactions",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Sessions_SessionId",
                table: "Transactions",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Sessions_SessionId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_SessionId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "Last",
                table: "Sessions",
                newName: "Current");

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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionTransactions_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SessionTransactions_TransactionId",
                table: "SessionTransactions",
                column: "TransactionId");
        }
    }
}
