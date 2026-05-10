using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddPromotion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "HR",
                table: "DecisionTypes",
                newName: "Code");

            migrationBuilder.RenameIndex(
                name: "IX_DecisionTypes_Name",
                schema: "HR",
                table: "DecisionTypes",
                newName: "IX_DecisionTypes_Code");

            migrationBuilder.CreateTable(
                name: "KpiReports",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Score = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    Grade = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KpiReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KpiReports_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HR",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PromotionCycles",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    EligibilityDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MinKpiScore = table.Column<int>(type: "int", nullable: false),
                    MaxPenaltyDays = table.Column<int>(type: "int", nullable: false),
                    KpiYearsToCheck = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionCycles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EligibilityResults",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PromotionCycleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ExclusionReason = table.Column<int>(type: "int", nullable: false),
                    CurrentGradeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentGradeCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CurrentGradeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CurrentGradeLevel = table.Column<int>(type: "int", nullable: false),
                    ProposedGradeLevel = table.Column<int>(type: "int", nullable: true),
                    AvgKpiScore = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    PenaltyDays = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    YearsInCurrentGrade = table.Column<decimal>(type: "decimal(4,1)", precision: 4, scale: 1, nullable: false),
                    IsSelected = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EligibilityResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EligibilityResults_PromotionCycles_PromotionCycleId",
                        column: x => x.PromotionCycleId,
                        principalSchema: "HR",
                        principalTable: "PromotionCycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PromotionHistory",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromGradeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToGradeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MovementType = table.Column<int>(type: "int", nullable: false),
                    PromotionCycleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LinkedDecisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromotionHistory_PromotionCycles_PromotionCycleId",
                        column: x => x.PromotionCycleId,
                        principalSchema: "HR",
                        principalTable: "PromotionCycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityResults_PromotionCycleId_EmployeeId",
                schema: "HR",
                table: "EligibilityResults",
                columns: new[] { "PromotionCycleId", "EmployeeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityResults_Status",
                schema: "HR",
                table: "EligibilityResults",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_KpiReports_EmployeeId_Year",
                schema: "HR",
                table: "KpiReports",
                columns: new[] { "EmployeeId", "Year" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PromotionHistory_EmployeeId_EffectiveDate",
                schema: "HR",
                table: "PromotionHistory",
                columns: new[] { "EmployeeId", "EffectiveDate" });

            migrationBuilder.CreateIndex(
                name: "IX_PromotionHistory_PromotionCycleId",
                schema: "HR",
                table: "PromotionHistory",
                column: "PromotionCycleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EligibilityResults",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "KpiReports",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "PromotionHistory",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "PromotionCycles",
                schema: "HR");

            migrationBuilder.RenameColumn(
                name: "Code",
                schema: "HR",
                table: "DecisionTypes",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_DecisionTypes_Code",
                schema: "HR",
                table: "DecisionTypes",
                newName: "IX_DecisionTypes_Name");
        }
    }
}
