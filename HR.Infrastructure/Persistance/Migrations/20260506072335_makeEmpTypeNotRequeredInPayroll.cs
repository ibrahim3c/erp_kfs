using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HR.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class makeEmpTypeNotRequeredInPayroll : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayrollCycles_EmploymentTypes_EmploymentTypeId",
                schema: "HR",
                table: "PayrollCycles");

            migrationBuilder.AlterColumn<Guid>(
                name: "EmploymentTypeId",
                schema: "HR",
                table: "PayrollCycles",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "EmploymentTypeId1",
                schema: "HR",
                table: "PayrollCycles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PayrollCycles_EmploymentTypeId1",
                schema: "HR",
                table: "PayrollCycles",
                column: "EmploymentTypeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollCycles_EmploymentTypes_EmploymentTypeId",
                schema: "HR",
                table: "PayrollCycles",
                column: "EmploymentTypeId",
                principalSchema: "HR",
                principalTable: "EmploymentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollCycles_EmploymentTypes_EmploymentTypeId1",
                schema: "HR",
                table: "PayrollCycles",
                column: "EmploymentTypeId1",
                principalSchema: "HR",
                principalTable: "EmploymentTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayrollCycles_EmploymentTypes_EmploymentTypeId",
                schema: "HR",
                table: "PayrollCycles");

            migrationBuilder.DropForeignKey(
                name: "FK_PayrollCycles_EmploymentTypes_EmploymentTypeId1",
                schema: "HR",
                table: "PayrollCycles");

            migrationBuilder.DropIndex(
                name: "IX_PayrollCycles_EmploymentTypeId1",
                schema: "HR",
                table: "PayrollCycles");

            migrationBuilder.DropColumn(
                name: "EmploymentTypeId1",
                schema: "HR",
                table: "PayrollCycles");

            migrationBuilder.AlterColumn<Guid>(
                name: "EmploymentTypeId",
                schema: "HR",
                table: "PayrollCycles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollCycles_EmploymentTypes_EmploymentTypeId",
                schema: "HR",
                table: "PayrollCycles",
                column: "EmploymentTypeId",
                principalSchema: "HR",
                principalTable: "EmploymentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
