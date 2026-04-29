using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Geography.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Geopraphy");

            migrationBuilder.CreateTable(
                name: "Governorates",
                schema: "Geopraphy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Governorates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CityCenters",
                schema: "Geopraphy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GovernorateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityCenters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CityCenters_Governorates_GovernorateId",
                        column: x => x.GovernorateId,
                        principalSchema: "Geopraphy",
                        principalTable: "Governorates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LocalUnits",
                schema: "Geopraphy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CityCenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocalUnits_CityCenters_CityCenterId",
                        column: x => x.CityCenterId,
                        principalSchema: "Geopraphy",
                        principalTable: "CityCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Villages",
                schema: "Geopraphy",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocalUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CityCenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Villages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Villages_CityCenters_CityCenterId",
                        column: x => x.CityCenterId,
                        principalSchema: "Geopraphy",
                        principalTable: "CityCenters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Villages_LocalUnits_LocalUnitId",
                        column: x => x.LocalUnitId,
                        principalSchema: "Geopraphy",
                        principalTable: "LocalUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CityCenters_GovernorateId",
                schema: "Geopraphy",
                table: "CityCenters",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_CityCenters_Name",
                schema: "Geopraphy",
                table: "CityCenters",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Governorates_Code",
                schema: "Geopraphy",
                table: "Governorates",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Governorates_Name",
                schema: "Geopraphy",
                table: "Governorates",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_LocalUnits_CityCenterId",
                schema: "Geopraphy",
                table: "LocalUnits",
                column: "CityCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_LocalUnits_Name",
                schema: "Geopraphy",
                table: "LocalUnits",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Villages_CityCenterId",
                schema: "Geopraphy",
                table: "Villages",
                column: "CityCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Villages_LocalUnitId",
                schema: "Geopraphy",
                table: "Villages",
                column: "LocalUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Villages_Name",
                schema: "Geopraphy",
                table: "Villages",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Villages",
                schema: "Geopraphy");

            migrationBuilder.DropTable(
                name: "LocalUnits",
                schema: "Geopraphy");

            migrationBuilder.DropTable(
                name: "CityCenters",
                schema: "Geopraphy");

            migrationBuilder.DropTable(
                name: "Governorates",
                schema: "Geopraphy");
        }
    }
}
