using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class addTerminationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceTerminationRequests",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "ServiceTerminationTypes",
                schema: "HR");

            migrationBuilder.CreateTable(
                name: "TerminationDecisions",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DecisionNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    DecisionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastWorkingDay = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LegalBasis = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AttachmentPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    CancellationReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TerminationDecisions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TerminationDecisions_DecisionNumber",
                schema: "HR",
                table: "TerminationDecisions",
                column: "DecisionNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TerminationDecisions_EmployeeId",
                schema: "HR",
                table: "TerminationDecisions",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TerminationDecisions",
                schema: "HR");

            migrationBuilder.CreateTable(
                name: "ServiceTerminationTypes",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RequiresNoticePeriod = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTerminationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTerminationRequests",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    IssuedTo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RequestStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ServiceTerminationTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTerminationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceTerminationRequests_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HR",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceTerminationRequests_ServiceTerminationTypes_ServiceTerminationTypeId",
                        column: x => x.ServiceTerminationTypeId,
                        principalSchema: "HR",
                        principalTable: "ServiceTerminationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTerminationRequests_EmployeeId",
                schema: "HR",
                table: "ServiceTerminationRequests",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTerminationRequests_ServiceTerminationTypeId",
                schema: "HR",
                table: "ServiceTerminationRequests",
                column: "ServiceTerminationTypeId");
        }
    }
}
