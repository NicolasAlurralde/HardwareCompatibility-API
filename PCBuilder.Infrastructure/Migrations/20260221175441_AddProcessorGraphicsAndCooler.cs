using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCBuilder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProcessorGraphicsAndCooler : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasIntegratedGraphics",
                table: "Processors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "StorageQuantity",
                table: "PCBuilds",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasIntegratedGraphics",
                table: "Processors");

            migrationBuilder.DropColumn(
                name: "StorageQuantity",
                table: "PCBuilds");
        }
    }
}
