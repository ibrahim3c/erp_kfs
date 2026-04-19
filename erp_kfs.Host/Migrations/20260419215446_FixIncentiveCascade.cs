using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyERP.Web.Migrations
{
    /// <inheritdoc />
    public partial class FixIncentiveCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicIncentiveRequests_AcademicIncentiveTypes_AcademicIncentiveTypeId",
                table: "AcademicIncentiveRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AcademicIncentiveRequests_EmployeeQualifications_QualificationId",
                table: "AcademicIncentiveRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AcademicIncentiveRequests_Employees_EmployeeId",
                table: "AcademicIncentiveRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Employees_SentBy",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Employees_SentTo",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Employees_ReviewerId",
                table: "Reviews");

            migrationBuilder.AlterColumn<string>(
                name: "ReviewerId",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "SentTo",
                table: "Notifications",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "SentBy",
                table: "Notifications",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId1",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnnualLeaveBalance",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "AcademicIncentiveTypeId1",
                table: "AcademicIncentiveRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId1",
                table: "AcademicIncentiveRequests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeQualificationId",
                table: "AcademicIncentiveRequests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LeaveTypes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxDays = table.Column<int>(type: "int", nullable: false),
                    RequiresApproval = table.Column<bool>(type: "bit", nullable: false),
                    AutoRenewDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsGenderSpecific = table.Column<bool>(type: "bit", nullable: false),
                    SalaryPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsAnnualBasedOnService = table.Column<bool>(type: "bit", nullable: false),
                    IsCasual = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LegalDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LegalReference = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PermissionId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPermissions",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PermissionId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermissions", x => new { x.UserId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_UserPermissions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ManagerId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Departments_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeAdmins",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDisabled = table.Column<bool>(type: "bit", nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Qualification = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FinancialGrade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GradeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AppointmentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BasicSalary2019 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GrossSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InsuranceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HasFellowshipFund = table.Column<bool>(type: "bit", nullable: false),
                    HasMutualAid = table.Column<bool>(type: "bit", nullable: false),
                    ServiceYears = table.Column<int>(type: "int", nullable: false),
                    ServiceMonths = table.Column<int>(type: "int", nullable: false),
                    ServiceDays = table.Column<int>(type: "int", nullable: false),
                    IsTerminated = table.Column<bool>(type: "bit", nullable: false),
                    TerminationReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TerminationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProfileImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InitialLeaveBalance = table.Column<int>(type: "int", nullable: false),
                    SelectedDepartmentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeAdmins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeAdmins_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeAdmins_Departments_SelectedDepartmentId",
                        column: x => x.SelectedDepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GlobalLeadershipPositions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalLeadershipPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GlobalLeadershipPositions_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeaveRequests",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LeaveTypeId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DaysRequested = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicalReportPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeaveTypeId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaveRequests_EmployeeAdmins_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "EmployeeAdmins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaveRequests_LeaveTypes_LeaveTypeId",
                        column: x => x.LeaveTypeId,
                        principalTable: "LeaveTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LeaveRequests_LeaveTypes_LeaveTypeId1",
                        column: x => x.LeaveTypeId1,
                        principalTable: "LeaveTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ServicePeriodAdditions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PeriodType = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Years = table.Column<int>(type: "int", nullable: false),
                    Months = table.Column<int>(type: "int", nullable: false),
                    Days = table.Column<int>(type: "int", nullable: false),
                    DocumentPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicePeriodAdditions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServicePeriodAdditions_EmployeeAdmins_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "EmployeeAdmins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LeadershipAssignments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PositionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false),
                    HijriDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GlobalLeadershipPositionId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadershipAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeadershipAssignments_EmployeeAdmins_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "EmployeeAdmins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeadershipAssignments_GlobalLeadershipPositions_GlobalLeadershipPositionId",
                        column: x => x.GlobalLeadershipPositionId,
                        principalTable: "GlobalLeadershipPositions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LeadershipAssignments_GlobalLeadershipPositions_PositionId",
                        column: x => x.PositionId,
                        principalTable: "GlobalLeadershipPositions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TerminationRequests",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReasonType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TerminationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServicePeriodAdditionId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TerminationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TerminationRequests_EmployeeAdmins_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "EmployeeAdmins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TerminationRequests_ServicePeriodAdditions_ServicePeriodAdditionId",
                        column: x => x.ServicePeriodAdditionId,
                        principalTable: "ServicePeriodAdditions",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "ManagerId", "Name", "ParentId", "Type" },
                values: new object[,]
                {
                    { "engineering", null, "الشئون الهندسية", null, "General" },
                    { "finance", null, "الإدارة العامة للشئون المالية", null, "General" },
                    { "governorate", null, "ديوان عام المحافظة", null, "General" },
                    { "hr", null, "الإدارة العامة للشئون الوظيفية", null, "General" },
                    { "it", null, "الإدارة العامة لنظم المعلومات والتحول الرقمي", null, "General" }
                });

            migrationBuilder.InsertData(
                table: "LeaveTypes",
                columns: new[] { "Id", "AutoRenewDate", "DisplayName", "IsAnnualBasedOnService", "IsCasual", "IsGenderSpecific", "MaxDays", "Name", "RequiresApproval", "SalaryPercentage" },
                values: new object[,]
                {
                    { "leave-annual", "07-01", "الإجازة الاعتيادية", true, false, false, 50, "Annual", true, 100m },
                    { "leave-casual", "07-01", "الإجازة العارضة", false, true, false, 2, "Casual", false, 100m },
                    { "leave-maternity", null, "إجازة الوضع", false, false, true, 120, "Maternity", true, 100m },
                    { "leave-sick", null, "الإجازة المرضية", false, false, false, 180, "Sick", true, 100m }
                });

            migrationBuilder.InsertData(
                table: "GlobalLeadershipPositions",
                columns: new[] { "Id", "DepartmentId", "DisplayName", "IsActive", "Level", "Title" },
                values: new object[,]
                {
                    { "pos-chief-secretary", "governorate", "السكرتير العام", true, 3, "ChiefSecretary" },
                    { "pos-deputy-chief", "governorate", "السكرتير العام المساعد", true, 4, "DeputyChiefSecretary" },
                    { "pos-deputy-governor", "governorate", "نائب المحافظ", true, 2, "DeputyGovernor" },
                    { "pos-governor", "governorate", "المحافظ", true, 1, "Governor" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_EmployeeId",
                table: "Notifications",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_EmployeeId1",
                table: "Notifications",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicIncentiveRequests_AcademicIncentiveTypeId1",
                table: "AcademicIncentiveRequests",
                column: "AcademicIncentiveTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicIncentiveRequests_EmployeeId1",
                table: "AcademicIncentiveRequests",
                column: "EmployeeId1");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicIncentiveRequests_EmployeeQualificationId",
                table: "AcademicIncentiveRequests",
                column: "EmployeeQualificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ManagerId",
                table: "Departments",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ParentId",
                table: "Departments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAdmins_ApplicationUserId",
                table: "EmployeeAdmins",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeAdmins_SelectedDepartmentId",
                table: "EmployeeAdmins",
                column: "SelectedDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalLeadershipPositions_DepartmentId",
                table: "GlobalLeadershipPositions",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadershipAssignments_EmployeeId",
                table: "LeadershipAssignments",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadershipAssignments_GlobalLeadershipPositionId",
                table: "LeadershipAssignments",
                column: "GlobalLeadershipPositionId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadershipAssignments_PositionId",
                table: "LeadershipAssignments",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_EmployeeId",
                table: "LeaveRequests",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_LeaveTypeId",
                table: "LeaveRequests",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequests_LeaveTypeId1",
                table: "LeaveRequests",
                column: "LeaveTypeId1");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePeriodAdditions_EmployeeId",
                table: "ServicePeriodAdditions",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_TerminationRequests_EmployeeId",
                table: "TerminationRequests",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_TerminationRequests_ServicePeriodAdditionId",
                table: "TerminationRequests",
                column: "ServicePeriodAdditionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_PermissionId",
                table: "UserPermissions",
                column: "PermissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicIncentiveRequests_AcademicIncentiveTypes_AcademicIncentiveTypeId",
                table: "AcademicIncentiveRequests",
                column: "AcademicIncentiveTypeId",
                principalTable: "AcademicIncentiveTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicIncentiveRequests_AcademicIncentiveTypes_AcademicIncentiveTypeId1",
                table: "AcademicIncentiveRequests",
                column: "AcademicIncentiveTypeId1",
                principalTable: "AcademicIncentiveTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicIncentiveRequests_EmployeeQualifications_EmployeeQualificationId",
                table: "AcademicIncentiveRequests",
                column: "EmployeeQualificationId",
                principalTable: "EmployeeQualifications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicIncentiveRequests_EmployeeQualifications_QualificationId",
                table: "AcademicIncentiveRequests",
                column: "QualificationId",
                principalTable: "EmployeeQualifications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicIncentiveRequests_Employees_EmployeeId",
                table: "AcademicIncentiveRequests",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicIncentiveRequests_Employees_EmployeeId1",
                table: "AcademicIncentiveRequests",
                column: "EmployeeId1",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_EmployeeAdmins_SentBy",
                table: "Notifications",
                column: "SentBy",
                principalTable: "EmployeeAdmins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_EmployeeAdmins_SentTo",
                table: "Notifications",
                column: "SentTo",
                principalTable: "EmployeeAdmins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Employees_EmployeeId",
                table: "Notifications",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Employees_EmployeeId1",
                table: "Notifications",
                column: "EmployeeId1",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_EmployeeAdmins_ReviewerId",
                table: "Reviews",
                column: "ReviewerId",
                principalTable: "EmployeeAdmins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_EmployeeAdmins_ManagerId",
                table: "Departments",
                column: "ManagerId",
                principalTable: "EmployeeAdmins",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AcademicIncentiveRequests_AcademicIncentiveTypes_AcademicIncentiveTypeId",
                table: "AcademicIncentiveRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AcademicIncentiveRequests_AcademicIncentiveTypes_AcademicIncentiveTypeId1",
                table: "AcademicIncentiveRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AcademicIncentiveRequests_EmployeeQualifications_EmployeeQualificationId",
                table: "AcademicIncentiveRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AcademicIncentiveRequests_EmployeeQualifications_QualificationId",
                table: "AcademicIncentiveRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AcademicIncentiveRequests_Employees_EmployeeId",
                table: "AcademicIncentiveRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AcademicIncentiveRequests_Employees_EmployeeId1",
                table: "AcademicIncentiveRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_EmployeeAdmins_SentBy",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_EmployeeAdmins_SentTo",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Employees_EmployeeId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Employees_EmployeeId1",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_EmployeeAdmins_ReviewerId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Departments_EmployeeAdmins_ManagerId",
                table: "Departments");

            migrationBuilder.DropTable(
                name: "LeadershipAssignments");

            migrationBuilder.DropTable(
                name: "LeaveRequests");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "TerminationRequests");

            migrationBuilder.DropTable(
                name: "UserPermissions");

            migrationBuilder.DropTable(
                name: "GlobalLeadershipPositions");

            migrationBuilder.DropTable(
                name: "LeaveTypes");

            migrationBuilder.DropTable(
                name: "ServicePeriodAdditions");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "EmployeeAdmins");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_EmployeeId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_EmployeeId1",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_AcademicIncentiveRequests_AcademicIncentiveTypeId1",
                table: "AcademicIncentiveRequests");

            migrationBuilder.DropIndex(
                name: "IX_AcademicIncentiveRequests_EmployeeId1",
                table: "AcademicIncentiveRequests");

            migrationBuilder.DropIndex(
                name: "IX_AcademicIncentiveRequests_EmployeeQualificationId",
                table: "AcademicIncentiveRequests");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "AnnualLeaveBalance",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AcademicIncentiveTypeId1",
                table: "AcademicIncentiveRequests");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                table: "AcademicIncentiveRequests");

            migrationBuilder.DropColumn(
                name: "EmployeeQualificationId",
                table: "AcademicIncentiveRequests");

            migrationBuilder.AlterColumn<int>(
                name: "ReviewerId",
                table: "Reviews",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "SentTo",
                table: "Notifications",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "SentBy",
                table: "Notifications",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicIncentiveRequests_AcademicIncentiveTypes_AcademicIncentiveTypeId",
                table: "AcademicIncentiveRequests",
                column: "AcademicIncentiveTypeId",
                principalTable: "AcademicIncentiveTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicIncentiveRequests_EmployeeQualifications_QualificationId",
                table: "AcademicIncentiveRequests",
                column: "QualificationId",
                principalTable: "EmployeeQualifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AcademicIncentiveRequests_Employees_EmployeeId",
                table: "AcademicIncentiveRequests",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Employees_SentBy",
                table: "Notifications",
                column: "SentBy",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Employees_SentTo",
                table: "Notifications",
                column: "SentTo",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Employees_ReviewerId",
                table: "Reviews",
                column: "ReviewerId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
