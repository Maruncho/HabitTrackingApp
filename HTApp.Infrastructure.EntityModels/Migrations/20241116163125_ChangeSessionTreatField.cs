using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HTApp.Infrastructure.EntityModels.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSessionTreatField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitsBought",
                table: "SessionTreats");

            migrationBuilder.AddColumn<byte>(
                name: "UnitsLeft",
                table: "SessionTreats",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnitsLeft",
                table: "SessionTreats");

            migrationBuilder.AddColumn<byte>(
                name: "UnitsBought",
                table: "SessionTreats",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
