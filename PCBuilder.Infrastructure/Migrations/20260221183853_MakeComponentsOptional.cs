using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCBuilder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeComponentsOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PCBuilds_PcCases_PcCaseId",
                table: "PCBuilds");

            migrationBuilder.DropForeignKey(
                name: "FK_PCBuilds_PowerSupplies_PowerSupplyId",
                table: "PCBuilds");

            migrationBuilder.DropForeignKey(
                name: "FK_PCBuilds_Rams_RamId",
                table: "PCBuilds");

            migrationBuilder.DropForeignKey(
                name: "FK_PCBuilds_Storages_StorageId",
                table: "PCBuilds");

            migrationBuilder.AlterColumn<int>(
                name: "StorageId",
                table: "PCBuilds",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RamId",
                table: "PCBuilds",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PowerSupplyId",
                table: "PCBuilds",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PcCaseId",
                table: "PCBuilds",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_PCBuilds_PcCases_PcCaseId",
                table: "PCBuilds",
                column: "PcCaseId",
                principalTable: "PcCases",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PCBuilds_PowerSupplies_PowerSupplyId",
                table: "PCBuilds",
                column: "PowerSupplyId",
                principalTable: "PowerSupplies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PCBuilds_Rams_RamId",
                table: "PCBuilds",
                column: "RamId",
                principalTable: "Rams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PCBuilds_Storages_StorageId",
                table: "PCBuilds",
                column: "StorageId",
                principalTable: "Storages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PCBuilds_PcCases_PcCaseId",
                table: "PCBuilds");

            migrationBuilder.DropForeignKey(
                name: "FK_PCBuilds_PowerSupplies_PowerSupplyId",
                table: "PCBuilds");

            migrationBuilder.DropForeignKey(
                name: "FK_PCBuilds_Rams_RamId",
                table: "PCBuilds");

            migrationBuilder.DropForeignKey(
                name: "FK_PCBuilds_Storages_StorageId",
                table: "PCBuilds");

            migrationBuilder.AlterColumn<int>(
                name: "StorageId",
                table: "PCBuilds",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RamId",
                table: "PCBuilds",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PowerSupplyId",
                table: "PCBuilds",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PcCaseId",
                table: "PCBuilds",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PCBuilds_PcCases_PcCaseId",
                table: "PCBuilds",
                column: "PcCaseId",
                principalTable: "PcCases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PCBuilds_PowerSupplies_PowerSupplyId",
                table: "PCBuilds",
                column: "PowerSupplyId",
                principalTable: "PowerSupplies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PCBuilds_Rams_RamId",
                table: "PCBuilds",
                column: "RamId",
                principalTable: "Rams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PCBuilds_Storages_StorageId",
                table: "PCBuilds",
                column: "StorageId",
                principalTable: "Storages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
