using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class addSoftDeleteByActivation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employees_Code_IsDeleted",
                schema: "HR",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "HR",
                table: "ServiceTerminationTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "HR",
                table: "ServiceTerminationTypes");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "HR",
                table: "ServiceTerminationRequests");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "HR",
                table: "ServiceTerminationRequests");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "HR",
                table: "QualificationTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "HR",
                table: "QualificationTypes");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "HR",
                table: "PermissionRequests");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "HR",
                table: "PermissionRequests");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "HR",
                table: "PenaltyRecords");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "HR",
                table: "PenaltyRecords");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "HR",
                table: "PayrollEntries");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "HR",
                table: "PayrollEntries");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "HR",
                table: "PayrollCycles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "HR",
                table: "PayrollCycles");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "HR",
                table: "PayrollAdjustments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "HR",
                table: "PayrollAdjustments");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "HR",
                table: "NominationFiles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "HR",
                table: "NominationFiles");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "HR",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "HR",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "HR",
                table: "LoanInstallments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "HR",
                table: "LoanInstallments");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "HR",
                table: "LateEntries");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "HR",
                table: "LateEntries");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "HR",
                table: "InsurancePeriodPurchases");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "HR",
                table: "InsurancePeriodPurchases");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "HR",
                table: "EmploymentTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "HR",
                table: "EmploymentTypes");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "HR",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "HR",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "HR",
                table: "EmployeeQualifications");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "HR",
                table: "EmployeeQualifications");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "HR",
                table: "EmployeeFinancials");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "HR",
                table: "EmployeeFinancials");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "HR",
                table: "EmployeeFiles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "HR",
                table: "EmployeeFiles");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "HR",
                table: "EmployeeFamilies");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "HR",
                table: "EmployeeFamilies");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "HR",
                table: "EmployeeDecisions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "HR",
                table: "EmployeeDecisions");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "HR",
                table: "DecisionTypes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "HR",
                table: "DecisionTypes");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "HR",
                table: "Decisions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "HR",
                table: "Decisions");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "HR",
                table: "DecisionAuthorities");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "HR",
                table: "DecisionAuthorities");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "HR",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "HR",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "HR",
                table: "AcademicIncentiveRequests");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "HR",
                table: "AcademicIncentiveRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "HR",
                table: "ServiceTerminationTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "HR",
                table: "ServiceTerminationTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "HR",
                table: "ServiceTerminationRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "HR",
                table: "ServiceTerminationRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "HR",
                table: "QualificationTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "HR",
                table: "QualificationTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "HR",
                table: "PermissionRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "HR",
                table: "PermissionRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "HR",
                table: "PenaltyRecords",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "HR",
                table: "PenaltyRecords",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "HR",
                table: "PayrollEntries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "HR",
                table: "PayrollEntries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "HR",
                table: "PayrollCycles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "HR",
                table: "PayrollCycles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "HR",
                table: "PayrollAdjustments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "HR",
                table: "PayrollAdjustments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "HR",
                table: "NominationFiles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "HR",
                table: "NominationFiles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "HR",
                table: "Loans",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "HR",
                table: "Loans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "HR",
                table: "LoanInstallments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "HR",
                table: "LoanInstallments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "HR",
                table: "LateEntries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "HR",
                table: "LateEntries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "HR",
                table: "InsurancePeriodPurchases",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "HR",
                table: "InsurancePeriodPurchases",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "HR",
                table: "EmploymentTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "HR",
                table: "EmploymentTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "HR",
                table: "Employees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "HR",
                table: "Employees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "HR",
                table: "EmployeeQualifications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "HR",
                table: "EmployeeQualifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "HR",
                table: "EmployeeFinancials",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "HR",
                table: "EmployeeFinancials",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "HR",
                table: "EmployeeFiles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "HR",
                table: "EmployeeFiles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "HR",
                table: "EmployeeFamilies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "HR",
                table: "EmployeeFamilies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "HR",
                table: "EmployeeDecisions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "HR",
                table: "EmployeeDecisions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "HR",
                table: "DecisionTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "HR",
                table: "DecisionTypes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "HR",
                table: "Decisions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "HR",
                table: "Decisions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "HR",
                table: "DecisionAuthorities",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "HR",
                table: "DecisionAuthorities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "HR",
                table: "Candidates",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "HR",
                table: "Candidates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "HR",
                table: "AcademicIncentiveRequests",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "HR",
                table: "AcademicIncentiveRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Code_IsDeleted",
                schema: "HR",
                table: "Employees",
                columns: new[] { "Code", "IsDeleted" },
                unique: true,
                filter: "[IsDeleted] = 0");
        }
    }
}
