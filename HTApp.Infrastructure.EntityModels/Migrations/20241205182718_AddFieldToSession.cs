using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HTApp.Infrastructure.EntityModels.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldToSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Current",
                table: "Sessions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Current",
                table: "Sessions");
        }
    }
}
