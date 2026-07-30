using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddEvaluation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AchievementScore",
                schema: "HR",
                table: "KpiReports",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DisciplineScore",
                schema: "HR",
                table: "KpiReports",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "EfficiencyScore",
                schema: "HR",
                table: "KpiReports",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "EvaluatorId",
                schema: "HR",
                table: "KpiReports",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                schema: "HR",
                table: "KpiReports",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                schema: "HR",
                table: "KpiReports",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Grievances",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GrievanceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ComplainedDecisionNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ComplainedDecisionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reasons = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    AttachmentPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CommitteeNotes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ResolutionDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grievances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grievances_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HR",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KpiReports_EvaluatorId",
                schema: "HR",
                table: "KpiReports",
                column: "EvaluatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Grievances_EmployeeId",
                schema: "HR",
                table: "Grievances",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Grievances_Status",
                schema: "HR",
                table: "Grievances",
                column: "Status");

            migrationBuilder.AddForeignKey(
                name: "FK_KpiReports_Employees_EvaluatorId",
                schema: "HR",
                table: "KpiReports",
                column: "EvaluatorId",
                principalSchema: "HR",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KpiReports_Employees_EvaluatorId",
                schema: "HR",
                table: "KpiReports");

            migrationBuilder.DropTable(
                name: "Grievances",
                schema: "HR");

            migrationBuilder.DropIndex(
                name: "IX_KpiReports_EvaluatorId",
                schema: "HR",
                table: "KpiReports");

            migrationBuilder.DropColumn(
                name: "AchievementScore",
                schema: "HR",
                table: "KpiReports");

            migrationBuilder.DropColumn(
                name: "DisciplineScore",
                schema: "HR",
                table: "KpiReports");

            migrationBuilder.DropColumn(
                name: "EfficiencyScore",
                schema: "HR",
                table: "KpiReports");

            migrationBuilder.DropColumn(
                name: "EvaluatorId",
                schema: "HR",
                table: "KpiReports");

            migrationBuilder.DropColumn(
                name: "Notes",
                schema: "HR",
                table: "KpiReports");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "HR",
                table: "KpiReports");
        }
    }
}
