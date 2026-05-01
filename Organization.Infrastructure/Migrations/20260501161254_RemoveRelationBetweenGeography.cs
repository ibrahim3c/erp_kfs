using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Organization.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRelationBetweenGeography : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrgUnits_Governorate_GovernorateId",
                schema: "Organization",
                table: "OrgUnits");

            migrationBuilder.DropTable(
                name: "Village",
                schema: "Organization");

            migrationBuilder.DropTable(
                name: "LocalUnit",
                schema: "Organization");

            migrationBuilder.DropTable(
                name: "CityCenter",
                schema: "Organization");

            migrationBuilder.DropTable(
                name: "Governorate",
                schema: "Organization");

            migrationBuilder.DropIndex(
                name: "IX_OrgUnits_GovernorateId",
                schema: "Organization",
                table: "OrgUnits");

            migrationBuilder.CreateIndex(
                name: "IX_LeadershipPositions_JobTitleId",
                schema: "Organization",
                table: "LeadershipPositions",
                column: "JobTitleId");

            migrationBuilder.AddForeignKey(
                name: "FK_LeadershipPositions_JobTitles_JobTitleId",
                schema: "Organization",
                table: "LeadershipPositions",
                column: "JobTitleId",
                principalSchema: "Organization",
                principalTable: "JobTitles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeadershipPositions_JobTitles_JobTitleId",
                schema: "Organization",
                table: "LeadershipPositions");

            migrationBuilder.DropIndex(
                name: "IX_LeadershipPositions_JobTitleId",
                schema: "Organization",
                table: "LeadershipPositions");

            migrationBuilder.CreateTable(
                name: "Governorate",
                schema: "Organization",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Governorate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CityCenter",
                schema: "Organization",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GovernorateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityCenter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CityCenter_Governorate_GovernorateId",
                        column: x => x.GovernorateId,
                        principalSchema: "Organization",
                        principalTable: "Governorate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LocalUnit",
                schema: "Organization",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CityCenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalUnit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocalUnit_CityCenter_CityCenterId",
                        column: x => x.CityCenterId,
                        principalSchema: "Organization",
                        principalTable: "CityCenter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Village",
                schema: "Organization",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocalUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CityCenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Village", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Village_CityCenter_CityCenterId",
                        column: x => x.CityCenterId,
                        principalSchema: "Organization",
                        principalTable: "CityCenter",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Village_LocalUnit_LocalUnitId",
                        column: x => x.LocalUnitId,
                        principalSchema: "Organization",
                        principalTable: "LocalUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrgUnits_GovernorateId",
                schema: "Organization",
                table: "OrgUnits",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_CityCenter_GovernorateId",
                schema: "Organization",
                table: "CityCenter",
                column: "GovernorateId");

            migrationBuilder.CreateIndex(
                name: "IX_LocalUnit_CityCenterId",
                schema: "Organization",
                table: "LocalUnit",
                column: "CityCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Village_CityCenterId",
                schema: "Organization",
                table: "Village",
                column: "CityCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Village_LocalUnitId",
                schema: "Organization",
                table: "Village",
                column: "LocalUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrgUnits_Governorate_GovernorateId",
                schema: "Organization",
                table: "OrgUnits",
                column: "GovernorateId",
                principalSchema: "Organization",
                principalTable: "Governorate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
