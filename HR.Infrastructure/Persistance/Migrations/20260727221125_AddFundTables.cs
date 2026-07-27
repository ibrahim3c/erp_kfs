using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddFundTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FundClaims",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AttachmentPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CommitteeNotes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    PaymentOrderNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FundClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FundClaims_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HR",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FundSubscriptions",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubscriptionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FundType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DeductionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BankAgreement = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FundSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FundSubscriptions_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HR",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FundClaims_ClaimType",
                schema: "HR",
                table: "FundClaims",
                column: "ClaimType");

            migrationBuilder.CreateIndex(
                name: "IX_FundClaims_EmployeeId",
                schema: "HR",
                table: "FundClaims",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_FundClaims_Status",
                schema: "HR",
                table: "FundClaims",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_FundSubscriptions_EmployeeId",
                schema: "HR",
                table: "FundSubscriptions",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_FundSubscriptions_FundType",
                schema: "HR",
                table: "FundSubscriptions",
                column: "FundType");

            migrationBuilder.CreateIndex(
                name: "IX_FundSubscriptions_Status",
                schema: "HR",
                table: "FundSubscriptions",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FundClaims",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "FundSubscriptions",
                schema: "HR");
        }
    }
}
