using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCrossModuleForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_EmploymentTypes_EmploymentTypeId",
                schema: "HR",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_FunctionalGroup_FunctionalGroupId",
                schema: "HR",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_JobGrade_JobGradeId",
                schema: "HR",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_JobTitle_JobTitleId",
                schema: "HR",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "JobGrade",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "JobTitle",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "FunctionalGroup",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "QualitativeGroup",
                schema: "HR");

            migrationBuilder.DropIndex(
                name: "IX_Employees_EmploymentTypeId",
                schema: "HR",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_FunctionalGroupId",
                schema: "HR",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_JobGradeId",
                schema: "HR",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_JobTitleId",
                schema: "HR",
                table: "Employees");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobGrade",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GradeLevel = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YearsNo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobGrade", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QualitativeGroup",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualitativeGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FunctionalGroup",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QualitativeGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FunctionalGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FunctionalGroup_QualitativeGroup_QualitativeGroupId",
                        column: x => x.QualitativeGroupId,
                        principalSchema: "HR",
                        principalTable: "QualitativeGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobTitle",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FunctionalGroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobTitle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobTitle_FunctionalGroup_FunctionalGroupId",
                        column: x => x.FunctionalGroupId,
                        principalSchema: "HR",
                        principalTable: "FunctionalGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmploymentTypeId",
                schema: "HR",
                table: "Employees",
                column: "EmploymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_FunctionalGroupId",
                schema: "HR",
                table: "Employees",
                column: "FunctionalGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_JobGradeId",
                schema: "HR",
                table: "Employees",
                column: "JobGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_JobTitleId",
                schema: "HR",
                table: "Employees",
                column: "JobTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionalGroup_QualitativeGroupId",
                schema: "HR",
                table: "FunctionalGroup",
                column: "QualitativeGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_JobTitle_FunctionalGroupId",
                schema: "HR",
                table: "JobTitle",
                column: "FunctionalGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_EmploymentTypes_EmploymentTypeId",
                schema: "HR",
                table: "Employees",
                column: "EmploymentTypeId",
                principalSchema: "HR",
                principalTable: "EmploymentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_FunctionalGroup_FunctionalGroupId",
                schema: "HR",
                table: "Employees",
                column: "FunctionalGroupId",
                principalSchema: "HR",
                principalTable: "FunctionalGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_JobGrade_JobGradeId",
                schema: "HR",
                table: "Employees",
                column: "JobGradeId",
                principalSchema: "HR",
                principalTable: "JobGrade",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_JobTitle_JobTitleId",
                schema: "HR",
                table: "Employees",
                column: "JobTitleId",
                principalSchema: "HR",
                principalTable: "JobTitle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
