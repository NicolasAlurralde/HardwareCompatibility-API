using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PCBuilder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPCBuildEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PCBuilds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuildName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessorId = table.Column<int>(type: "int", nullable: false),
                    MotherboardId = table.Column<int>(type: "int", nullable: false),
                    RamId = table.Column<int>(type: "int", nullable: false),
                    RamQuantity = table.Column<int>(type: "int", nullable: false),
                    PowerSupplyId = table.Column<int>(type: "int", nullable: false),
                    PcCaseId = table.Column<int>(type: "int", nullable: false),
                    StorageId = table.Column<int>(type: "int", nullable: false),
                    VideoCardId = table.Column<int>(type: "int", nullable: true),
                    CoolerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PCBuilds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PCBuilds_Coolers_CoolerId",
                        column: x => x.CoolerId,
                        principalTable: "Coolers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PCBuilds_Motherboards_MotherboardId",
                        column: x => x.MotherboardId,
                        principalTable: "Motherboards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PCBuilds_PcCases_PcCaseId",
                        column: x => x.PcCaseId,
                        principalTable: "PcCases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PCBuilds_PowerSupplies_PowerSupplyId",
                        column: x => x.PowerSupplyId,
                        principalTable: "PowerSupplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PCBuilds_Processors_ProcessorId",
                        column: x => x.ProcessorId,
                        principalTable: "Processors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PCBuilds_Rams_RamId",
                        column: x => x.RamId,
                        principalTable: "Rams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PCBuilds_Storages_StorageId",
                        column: x => x.StorageId,
                        principalTable: "Storages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PCBuilds_VideoCards_VideoCardId",
                        column: x => x.VideoCardId,
                        principalTable: "VideoCards",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PCBuilds_CoolerId",
                table: "PCBuilds",
                column: "CoolerId");

            migrationBuilder.CreateIndex(
                name: "IX_PCBuilds_MotherboardId",
                table: "PCBuilds",
                column: "MotherboardId");

            migrationBuilder.CreateIndex(
                name: "IX_PCBuilds_PcCaseId",
                table: "PCBuilds",
                column: "PcCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_PCBuilds_PowerSupplyId",
                table: "PCBuilds",
                column: "PowerSupplyId");

            migrationBuilder.CreateIndex(
                name: "IX_PCBuilds_ProcessorId",
                table: "PCBuilds",
                column: "ProcessorId");

            migrationBuilder.CreateIndex(
                name: "IX_PCBuilds_RamId",
                table: "PCBuilds",
                column: "RamId");

            migrationBuilder.CreateIndex(
                name: "IX_PCBuilds_StorageId",
                table: "PCBuilds",
                column: "StorageId");

            migrationBuilder.CreateIndex(
                name: "IX_PCBuilds_VideoCardId",
                table: "PCBuilds",
                column: "VideoCardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PCBuilds");
        }
    }
}
