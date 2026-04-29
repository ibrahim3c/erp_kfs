using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Organization.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Organization");

            migrationBuilder.CreateTable(
                name: "JobGrades",
                schema: "Organization",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GradeLevel = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    YearsNo = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobGrades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrgUnitTypes",
                schema: "Organization",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LevelOrder = table.Column<int>(type: "int", nullable: false),
                    CanHaveChild = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgUnitTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QualitativeGroups",
                schema: "Organization",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualitativeGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrgUnits",
                schema: "Organization",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrgUnitTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    GovernorateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrgUnitTypeId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrgUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrgUnits_OrgUnitTypes_OrgUnitTypeId",
                        column: x => x.OrgUnitTypeId,
                        principalSchema: "Organization",
                        principalTable: "OrgUnitTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrgUnits_OrgUnitTypes_OrgUnitTypeId1",
                        column: x => x.OrgUnitTypeId1,
                        principalSchema: "Organization",
                        principalTable: "OrgUnitTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrgUnits_OrgUnits_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Organization",
                        principalTable: "OrgUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FunctionalGroups",
                schema: "Organization",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QualitativeGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    QualitativeGroupId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FunctionalGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FunctionalGroups_QualitativeGroups_QualitativeGroupId",
                        column: x => x.QualitativeGroupId,
                        principalSchema: "Organization",
                        principalTable: "QualitativeGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FunctionalGroups_QualitativeGroups_QualitativeGroupId1",
                        column: x => x.QualitativeGroupId1,
                        principalSchema: "Organization",
                        principalTable: "QualitativeGroups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LeadershipPositions",
                schema: "Organization",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrgUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobTitleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadershipPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadershipPositions_OrgUnits_OrgUnitId",
                        column: x => x.OrgUnitId,
                        principalSchema: "Organization",
                        principalTable: "OrgUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobTitles",
                schema: "Organization",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FunctionalGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FunctionalGroupId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTitles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobTitles_FunctionalGroups_FunctionalGroupId",
                        column: x => x.FunctionalGroupId,
                        principalSchema: "Organization",
                        principalTable: "FunctionalGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobTitles_FunctionalGroups_FunctionalGroupId1",
                        column: x => x.FunctionalGroupId1,
                        principalSchema: "Organization",
                        principalTable: "FunctionalGroups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LeadershipPositionHistories",
                schema: "Organization",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeadershipPositionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DecisionNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DecisionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadershipPositionHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadershipPositionHistories_LeadershipPositions_LeadershipPositionId",
                        column: x => x.LeadershipPositionId,
                        principalSchema: "Organization",
                        principalTable: "LeadershipPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FunctionalGroups_QualitativeGroupId",
                schema: "Organization",
                table: "FunctionalGroups",
                column: "QualitativeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionalGroups_QualitativeGroupId1",
                schema: "Organization",
                table: "FunctionalGroups",
                column: "QualitativeGroupId1");

            migrationBuilder.CreateIndex(
                name: "IX_JobTitles_FunctionalGroupId",
                schema: "Organization",
                table: "JobTitles",
                column: "FunctionalGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_JobTitles_FunctionalGroupId1",
                schema: "Organization",
                table: "JobTitles",
                column: "FunctionalGroupId1");

            migrationBuilder.CreateIndex(
                name: "IX_LeadershipPositionHistories_EmployeeId",
                schema: "Organization",
                table: "LeadershipPositionHistories",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadershipPositionHistories_LeadershipPositionId",
                schema: "Organization",
                table: "LeadershipPositionHistories",
                column: "LeadershipPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadershipPositions_OrgUnitId",
                schema: "Organization",
                table: "LeadershipPositions",
                column: "OrgUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgUnits_Code",
                schema: "Organization",
                table: "OrgUnits",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrgUnits_Name",
                schema: "Organization",
                table: "OrgUnits",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_OrgUnits_OrgUnitTypeId",
                schema: "Organization",
                table: "OrgUnits",
                column: "OrgUnitTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgUnits_OrgUnitTypeId1",
                schema: "Organization",
                table: "OrgUnits",
                column: "OrgUnitTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_OrgUnits_ParentId",
                schema: "Organization",
                table: "OrgUnits",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_OrgUnitTypes_Code",
                schema: "Organization",
                table: "OrgUnitTypes",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobGrades",
                schema: "Organization");

            migrationBuilder.DropTable(
                name: "JobTitles",
                schema: "Organization");

            migrationBuilder.DropTable(
                name: "LeadershipPositionHistories",
                schema: "Organization");

            migrationBuilder.DropTable(
                name: "FunctionalGroups",
                schema: "Organization");

            migrationBuilder.DropTable(
                name: "LeadershipPositions",
                schema: "Organization");

            migrationBuilder.DropTable(
                name: "QualitativeGroups",
                schema: "Organization");

            migrationBuilder.DropTable(
                name: "OrgUnits",
                schema: "Organization");

            migrationBuilder.DropTable(
                name: "OrgUnitTypes",
                schema: "Organization");
        }
    }
}
