using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCBuilder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSecondaryComponents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SecondaryStorageId",
                table: "PCBuilds",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SecondaryStorageQuantity",
                table: "PCBuilds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SecondaryVideoCardId",
                table: "PCBuilds",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PCBuilds_SecondaryStorageId",
                table: "PCBuilds",
                column: "SecondaryStorageId");

            migrationBuilder.CreateIndex(
                name: "IX_PCBuilds_SecondaryVideoCardId",
                table: "PCBuilds",
                column: "SecondaryVideoCardId");

            migrationBuilder.AddForeignKey(
                name: "FK_PCBuilds_Storages_SecondaryStorageId",
                table: "PCBuilds",
                column: "SecondaryStorageId",
                principalTable: "Storages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PCBuilds_VideoCards_SecondaryVideoCardId",
                table: "PCBuilds",
                column: "SecondaryVideoCardId",
                principalTable: "VideoCards",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PCBuilds_Storages_SecondaryStorageId",
                table: "PCBuilds");

            migrationBuilder.DropForeignKey(
                name: "FK_PCBuilds_VideoCards_SecondaryVideoCardId",
                table: "PCBuilds");

            migrationBuilder.DropIndex(
                name: "IX_PCBuilds_SecondaryStorageId",
                table: "PCBuilds");

            migrationBuilder.DropIndex(
                name: "IX_PCBuilds_SecondaryVideoCardId",
                table: "PCBuilds");

            migrationBuilder.DropColumn(
                name: "SecondaryStorageId",
                table: "PCBuilds");

            migrationBuilder.DropColumn(
                name: "SecondaryStorageQuantity",
                table: "PCBuilds");

            migrationBuilder.DropColumn(
                name: "SecondaryVideoCardId",
                table: "PCBuilds");
        }
    }
}
