using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class RetriementEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RetirementFiles",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReferralDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Stage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ResponsibleEmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    JoinPeriodsAdded = table.Column<bool>(type: "bit", nullable: false),
                    SpecialLeavesReviewed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RetirementFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RetirementSalaryRecords",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(type: "int", nullable: false),
                    BasicInsuredSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RetirementFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RetirementSalaryRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RetirementSalaryRecords_RetirementFiles_RetirementFileId",
                        column: x => x.RetirementFileId,
                        principalSchema: "HR",
                        principalTable: "RetirementFiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RetirementFiles_EmployeeId",
                schema: "HR",
                table: "RetirementFiles",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_RetirementSalaryRecords_RetirementFileId",
                schema: "HR",
                table: "RetirementSalaryRecords",
                column: "RetirementFileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RetirementSalaryRecords",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "RetirementFiles",
                schema: "HR");
        }
    }
}
