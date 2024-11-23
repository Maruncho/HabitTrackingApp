using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HTApp.Infrastructure.EntityModels.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCascadeRemoveTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionTransactions_Sessions_SessionId",
                table: "SessionTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionTransactions_Transactions_TransactionId",
                table: "SessionTransactions");

            migrationBuilder.AddForeignKey(
                name: "FK_SessionTransactions_Sessions_SessionId",
                table: "SessionTransactions",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionTransactions_Transactions_TransactionId",
                table: "SessionTransactions",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionTransactions_Sessions_SessionId",
                table: "SessionTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionTransactions_Transactions_TransactionId",
                table: "SessionTransactions");

            migrationBuilder.AddForeignKey(
                name: "FK_SessionTransactions_Sessions_SessionId",
                table: "SessionTransactions",
                column: "SessionId",
                principalTable: "Sessions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SessionTransactions_Transactions_TransactionId",
                table: "SessionTransactions",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
