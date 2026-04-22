# AGENTS.md - erp_kfs Development Guidelines

## Overview

This is an ASP.NET Core 8 ERP system for governorate management with modular Clean Architecture (Domain/Application/Infrastructure layers).

## Build Commands

### Solution & Project Building

```bash
# Build entire solution
dotnet build

# Build specific project
dotnet build <ProjectName>/<ProjectName>.csproj

# Build with specific configuration
dotnet build -c Release
```

### Running the Application

```bash
# Run the host project
dotnet run --project erp_kfs.Host

# Run with custom port
dotnet run --project erp_kfs.Host --urls "http://localhost:5000"
```

### Database Migrations

```bash
# Add migration (from erp_kfs.Host directory)
dotnet ef migrations add <MigrationName>

# Update database
dotnet ef database update

# Remove last migration
dotnet ef migrations remove
```

### Package Management

```bash
# Restore packages
dotnet restore

# Restore for specific project
dotnet restore <ProjectName>/<ProjectName>.csproj
```

## Code Style Guidelines

### General Conventions

1. **Implicit Usings**: Enabled globally - do NOT add explicit `using` statements for:
  - `System`, `System.Collections.Generic`, `System.Linq`, `System.Threading`, `System.Threading.Tasks`
2. **Nullable Reference Types**: Enabled - always use proper null annotations
3. **File-Scoped Namespaces**: NOT used - use block namespace style:
  ```csharp
   namespace Identity.Domain
   {
       public class AppRole : IdentityRole<Guid>
       {
       }
   }
  ```
4. **Target Framework**: All projects target `.NET 8.0`

### Naming Conventions


| Element            | Convention | Example                      |
| ------------------ | ---------- | ---------------------------- |
| Classes/Interfaces | PascalCase | `AppRole`, `IAuthService`    |
| Methods            | PascalCase | `GetUserById`                |
| Properties         | PascalCase | `UserName`, `IsActive`       |
| Private fields     | camelCase  | `_userService`, `_dbContext` |
| Parameters         | camelCase  | `userId`, `roleName`         |
| Constants          | PascalCase | `DefaultRole`, `MaxRetries`  |
| Enums              | PascalCase | `UserStatus`, `LeaveType`    |


### Project Structure

Each module follows this pattern:

```
ModuleName.Domain/       - Entities, Value Objects, Domain Events
ModuleName.Application/ - Services, DTOs, Interfaces
ModuleName.Infrastructure/ - DbContext, Repositories, External Services
```

### Architecture Patterns

1. **Domain Layer**: Entities inherit from base types, no infrastructure dependencies
2. **Application Layer**: Depends only on Domain and Modules.Shared.Application
3. **Infrastructure Layer**: EF Core, external service implementations
4. **Host (erp_kfs.Host)**: Web API, Controllers, ViewModels

### Entity Guidelines

- Use `Guid` for all primary keys (not `int`)
- Implement soft delete patterns where appropriate
- Use navigation properties with proper lazy loading configuration

### API Controller Guidelines

- Use attribute routing `[Route("api/[controller]")]`
- Return appropriate HTTP status codes
- Use `[FromBody]`, `[FromQuery]` attributes explicitly

### Error Handling

- Use try-catch for operations that may fail
- Log exceptions with appropriate level
- Return meaningful error messages to clients

### Dependency Injection

- Register services in appropriate lifetime (Scoped, Transient, Singleton)
- Use constructor injection primarily

### Working with This Codebase

1. **Key Technologies**:
  - ASP.NET Core 8
  - Entity Framework Core 8
  - SQL Server
  - MediatR
  - FluentValidation
  - ASP.NET Core Identity
2. **Module Dependencies**:
  - Domain projects depend on Modules.Shared.Domain
  - Application projects depend on their Domain + Modules.Shared.Application
  - Infrastructure projects depend on their Application
  - Host depends on all Application + Infrastructure modules
3. **Common Tasks**:
  - Add new entity: Create in Domain, add DbSet in Infrastructure, create service in Application
  - Add new API endpoint: Create controller in erp_kfs.Host/Areas/{Module}
  - Add migration: Run from erp_kfs.Host directory

## Important Notes

- No test projects exist currently
- Some files use inconsistent formatting (older code)
- Arabic comments exist in some files (legacy)
- Primary language is Arabic (Egyptian context) but code is in English
- Uses legacy ASP.NET MVC views combined with modern patterns

# AGENT.md — erp_kfs Developer Reference

> **Project:** ERP System for Egyptian Government (Governorate-level)
> **Type:** ASP.NET Core 8 | Modular Clean Architecture (Modular Monolith)
> **Entry Point:** `erp_kfs.Host` (ASP.NET Core MVC)

---

## Table of Contents

1. [Architecture Overview](#architecture-overview)
2. [Solution Structure](#solution-structure)
3. [Modules Breakdown](#modules-breakdown)
4. [Database Schema (ERD)](#database-schema-erd)
5. [Workflow — Recruitment to Employment](#workflow--recruitment-to-employment)
6. [Technology Stack](#technology-stack)
7. [Conventions & Patterns](#conventions--patterns)
8. [Migration Commands](#migration-commands)
9. [Key Business Rules](#key-business-rules)

---

## Architecture Overview

The system follows **Modular Monolithic + Clean Architecture + CQRS**.

```
Presentation (MVC Host)
        │
        ▼
┌──────────────────────────────┐
│  Module (e.g., HR)           │
│  ├── Domain      (Entities, Value Objects, Domain Events, Interfaces)
│  ├── Application (Commands, Queries, DTOs, Interfaces, Handlers)
│  └── Infrastructure (DbContext, Repositories, External Services)
└──────────────────────────────┘
        │
        ▼
   Shared Kernel (Modules.Shared.*)
```

**Key principles:**

- Each module is **self-contained** with its own DbContext, repositories, and domain
- Modules communicate via **interfaces (sync)** or **domain events via MediatR (async)**
- All modules share a **single RDBMS** (SQL Server)
- Modules are grouped under `Low Coupling / High Cohesion`
- Architecture can evolve into microservices: each module is already a logical microservice

---

## Solution Structure

```
erp_kfs.sln
│
├── erp_kfs.Host                        ← ASP.NET Core MVC entry point
│   ├── Areas/
│   │   └── HR/
│   │       ├── Controllers/
│   │       ├── ViewModels/
│   │       └── Views/
│   ├── wwwroot/
│   │   └── hr/                        ← HR-specific CSS/JS
│   └── Program.cs                     ← MediatR + DbContext registration
│
├── Modules/
│   ├── HR/
│   │   ├── ERP.HR.Domain
│   │   ├── HR.Application
│   │   └── HR.Infrastructure
│   ├── Organization/
│   ├── Geography/
│   ├── Decisions/
│   ├── Identity/
│   └── Common/
│
└── Shared/
    ├── Modules.Shared.Domain
    ├── Modules.Shared.Application
    └── Modules.Shared.Infrastructure
```

---

## Modules Breakdown

### Module: HR — Employee Lifecycle

All tables related to employees from recruitment to termination.


| Table                          | Purpose                                                |
| ------------------------------ | ------------------------------------------------------ |
| `employees`                    | Core employee record (most important table)            |
| `employee_files`               | Physical document paths (military, ID, certificate...) |
| `employee_families`            | Family members of each employee                        |
| `employee_qualifications`      | Academic qualifications per employee                   |
| `employment_types`             | Civil service / contract types                         |
| `qualification_types`          | Degree types (Bachelor, Master...)                     |
| `qualitative_groups`           | Top-level job classification groups                    |
| `functional_groups`            | Sub-groups under qualitative groups                    |
| `job_titles`                   | Specific job titles under functional groups            |
| `job_grades`                   | Grade levels with promotion year counts                |
| `candidates`                   | Applicants before becoming employees                   |
| `nomination_files`             | Nomination/appointment file per candidate              |
| `decision_types`               | Types of admin decisions (promotion, transfer...)      |
| `decision_authorities`         | Who can issue which decisions                          |
| `decisions`                    | Official administrative decisions                      |
| `employee_decisions`           | Links decisions to specific employees                  |
| `academic_incentive_types`     | Types of academic incentives                           |
| `academic_incentive_requests`  | Employee requests for academic incentives              |
| `service_termination_types`    | Types of service ending (resignation, retirement...)   |
| `service_termination_requests` | Employee termination requests                          |


**Domain Aggregates:**

```
Candidates (Aggregate 1)
  ├── Candidate.cs
  ├── NominationFile.cs
  ├── NominationStatus.cs (Enum)
  └── ICandidateRepository.cs

Employees (Aggregate 2)
  ├── Employee.cs             ← Aggregate Root
  ├── EmployeeFile.cs
  ├── EmployeeFamily.cs
  ├── EmployeeQualification.cs
  ├── EmployeeDecision.cs
  ├── AcademicIncentiveRequest.cs
  ├── LeadershipPositionHistory.cs
  ├── IEmployeeRepository.cs
  ├── EmploymentType.cs
  └── AcademicIncentiveStatus.cs

Events/
  └── EmployeeHiredDomainEvent.cs
```

**Application Layer Commands/Queries:**

```
Candidates/
  Commands/
    ├── CreateCandidateCommand + Handler
    ├── UploadEFileDocumentCommand + Handler
    ├── AddReviewCommand + Handler        ← HR/Legal review
    └── ApproveCandidateCommand + Handler ← Final approval
  Queries/
    ├── GetPendingCandidatesQuery + Handler
    └── GetCandidateEFileQuery + Handler
Interfaces/
  ├── IHRUnitOfWork.cs
  └── IEFileStorageService.cs
```

---

### Module: Organization — Administrative Structure


| Table                           | Purpose                                                     |
| ------------------------------- | ----------------------------------------------------------- |
| `org_unit_types`                | Types: Ministry / General Dept / Section (with level_order) |
| `org_units`                     | Self-referencing tree of organizational units               |
| `leadership_positions`          | Leadership posts linked to an org unit + job title          |
| `leadership_position_histories` | Who held each leadership position over time                 |


**org_units is a self-referencing tree:**

```
parent_id → org_units.id (NULL = root)

Example:
id=1  name="ديوان المحافظة"         parent_id=NULL
id=2  name="إدارة تكنولوجيا المعلومات"  parent_id=1
id=3  name="قسم تطوير الأنظمة"         parent_id=2
```

---

### Module: Geography — Administrative Divisions

Represents Egypt's administrative geography hierarchy.

```
governorates         ← محافظات
    └── city_centers ← مراكز / أقسام (type: مدينة | مركز)
          └── local_units ← وحدات محلية
                └── villages ← قرى
```


| Table          | Key Columns                         |
| -------------- | ----------------------------------- |
| `governorates` | id, name, code (e.g., CAI, GIZ)     |
| `city_centers` | governorate_id, name, type          |
| `local_units`  | city_center_id, name                |
| `villages`     | city_center_id, local_unit_id, name |


---

### Module: Common — Shared Services


| Table           | Purpose                                                 |
| --------------- | ------------------------------------------------------- |
| `settings`      | Single-row system settings (org name, logo)             |
| `notifications` | sent_to / sent_by (both FK to employees)                |
| `documents`     | Polymorphic document store (model_type + model_id)      |
| `reviews`       | Polymorphic review record (model_type + model_id)       |
| `audit_logs`    | Full audit trail of all CREATE/UPDATE/DELETE operations |


**Polymorphic pattern** used in `documents` and `reviews`:

```sql
model_type = 'candidate' | 'nomination_file' | 'employee' | ...
model_id   = the FK to that model's table
```

---

### Module: Identity


| Entity     | Purpose                                 |
| ---------- | --------------------------------------- |
| User       | System users (HR officers, managers...) |
| Role       | Permission groupings                    |
| Permission | Granular access control                 |


---

## Database Schema (ERD)

### Job Structure Hierarchy

```
qualitative_groups
    └── functional_groups
              └── job_titles
                       └── (assigned to employees with) job_grades
```

### Key Foreign Key Chains

```sql
-- Geographic chain
governorates → city_centers → local_units → villages

-- Job classification chain
qualitative_groups → functional_groups → job_titles

-- Org hierarchy (self-referencing)
org_units.parent_id → org_units.id

-- Employee core FKs
employees → city_centers
employees → villages
employees → qualification_types
employees → employment_types
employees → job_titles
employees → job_grades
employees → functional_groups
employees → org_units

-- Recruitment chain
candidates → nomination_files → (approved) → employees

-- Decision chain
decision_types → decisions → employee_decisions → employees
```

### Notable ERD Design Decisions

- `nomination_files.receive_date` has an incorrect FK pointing to itself — treat as a plain `date` column
- `employment_types.is_civil_service_law` differentiates civil service law employees from contract workers
- `decision_types` has flags: `affects_employment_type`, `affects_salary`, `affects_position` — use these to drive business logic
- `audit_logs` uses XML `change_details` column to store field-level diffs

---

## Workflow — Recruitment to Employment

### Phase 1: Receive Nominations

**Source:** Central Organization & Administration Agency (JCOA) or another government body

**Tables used:** `candidates`, `nomination_files`, `documents`, `qualification_types`, `city_centers`, `villages`

```sql
-- 1. Insert candidate
INSERT INTO candidates (full_name, national_id, phone, email, qualification_type_id, city_center_id, village_id)

-- 2. Upload documents to `documents` table (polymorphic)
-- model_type='candidate', model_id=candidate.id
-- Documents: مؤهل، شهادة جيش، ميلاد، فيش جنائي، بطاقة أمام/خلف، قسيمة زواج، صورة شخصية

-- 3. Create nomination file
INSERT INTO nomination_files (candidate_id, receive_date, expected_end_date, status, reference_number)
-- status = 'تحت المراجعة'
```

### Phase 2: Review & Verify Documents

**Roles involved:** HR Officer, Legal Department

**Tables used:** `reviews`, `documents`, `notifications`

```sql
-- HR/Legal creates a review record
INSERT INTO reviews (reviewer_id, model_type, model_id, start_date, status)
-- model_type='nomination_file', status='pending' → 'approved'|'rejected'

-- On rejection → send notification
INSERT INTO notifications (sent_to, sent_by, text)
```

### Phase 3: Convert Candidate → Employee

**Tables used:** `employees`, `employee_files`, `employee_qualifications`, `employment_types`, `job_titles`, `job_grades`, `functional_groups`, `org_units`

```sql
-- 1. Create employee record
INSERT INTO employees (name, national_id, hire_date, employment_type_id, job_title_id, job_grade_id, functional_group_id, org_unit_id, ...)

-- 2. Record qualification
INSERT INTO employee_qualifications (employee_id, qualification_type_id, specialization, university, graduation_year, grade)

-- 3. Upload official files
INSERT INTO employee_files (employee_id, military_file, qualification_file, birth_certificate_file, ...)
```

### Phase 4: Issue Appointment Decision

**Tables used:** `decisions`, `decision_types`, `decision_authorities`, `employee_decisions`, `documents`

```sql
-- 1. Ensure decision type exists
-- decision_types: 'قرار تعيين'

-- 2. Create decision
INSERT INTO decisions (number, decision_type_id, decision_authority_id, decision_date, subject, status)

-- 3. Link to employee
INSERT INTO employee_decisions (decision_id, employee_id, valid_from, status)
```

### Phase 5: Notify Departments

**Tables used:** `notifications`, `employees`, `decisions`

```sql
-- Auto-notify: payroll, fingerprint, promotions departments
INSERT INTO notifications (sent_to, sent_by, text)
-- "تم تعيين موظف جديد"
```

### Full Scenario Flow

```
محمد submits application
    ↓ candidates
Create nomination file
    ↓ nomination_files
Upload documents
    ↓ documents
Review file
    ↓ reviews
Approve candidate
    ↓ employees  (candidate becomes employee)
Register qualification
    ↓ employee_qualifications
Issue appointment decision
    ↓ decisions
Link decision to employee
    ↓ employee_decisions
Send notifications
    ↓ notifications
```

---

## Technology Stack


| Layer                    | Technology                                                       |
| ------------------------ | ---------------------------------------------------------------- |
| Web Framework            | ASP.NET Core 8 MVC                                               |
| ORM                      | Entity Framework Core (Code First)                               |
| Database                 | SQL Server                                                       |
| Authentication           | JWT (Identity module)                                            |
| Messaging (intra-module) | MediatR (CQRS + Domain Events)                                   |
| Frontend                 | HTML, CSS, JS, Bootstrap                                         |
| Architecture             | Modular Monolith → Clean Arch → CQRS + Repository + Unit of Work |


---

## Conventions & Patterns

### Naming

- Tables: `snake_case` (plural nouns)
- FKs: `{table_singular}_id`
- Soft delete columns: `deleted_at` (datetime) + `deleted_by` (int FK to user)
- Audit columns on most tables: `created_at`, `created_by`, `updated_at`, `updated_by`

### Soft Deletes

Tables with soft delete support have: `deleted_at datetime` + `deleted_by int`
Always filter with `WHERE deleted_at IS NULL` unless querying history.

### Polymorphic Tables

`documents` and `reviews` are polymorphic:

```csharp
// Example EF config
modelBuilder.Entity<Document>()
    .HasIndex(d => new { d.ModelType, d.ModelId });
```

### CQRS Pattern

```csharp
// Command (write)
public record CreateCandidateCommand(...) : ICommand<int>;
public class CreateCandidateCommandHandler : ICommandHandler<CreateCandidateCommand, int> { }

// Query (read)
public record GetPendingCandidatesQuery() : IQuery<List<CandidateDto>>;
public class GetPendingCandidatesQueryHandler : IQueryHandler<GetPendingCandidatesQuery, List<CandidateDto>> { }
```

### Inter-module Communication

```csharp
// Sync: via interface (HR module exposes IHRService to Recruitment)
public interface IHRService {
    Employee AddEmployee(EmployeeDto dto);
}

// Async: via domain events (MediatR)
public class EmployeeHiredDomainEvent : IDomainEvent {
    public int EmployeeId { get; }
}
```

### Repository + Unit of Work

```csharp
public interface IHRUnitOfWork {
    ICandidateRepository Candidates { get; }
    IEmployeeRepository Employees { get; }
    Task<int> SaveChangesAsync();
}
```

### Shared Kernel Base Types

```csharp
// All entities inherit from:
Entity.cs          // Id, domain events collection
Result.cs          // Result<T> for error handling without exceptions
Error.cs           // Structured error type
IDomainEvent.cs    // Marker interface for domain events
```

---

## Migration Commands

Each module has its **own DbContext** and **own migrations folder**.

```bash
# HR Module
Add-Migration Init_HR -Context HRDbContext -OutputDir Persistance\Migrations

# Organization Module
Add-Migration Init_Organization -Context OrganizationDbContext -OutputDir Persistance\Migrations

# Geography Module
Add-Migration Init_Geography -Context GeographyDbContext -OutputDir Persistance\Migrations
```

Run from Package Manager Console with the correct **Default Project** set to the Infrastructure project of each module.

---

## Key Business Rules

### Employment Types (`employment_types`)


| Flag                   | Meaning                                              |
| ---------------------- | ---------------------------------------------------- |
| `is_civil_service_law` | Subject to civil service law (قانون الخدمة المدنية)  |
| `has_contract_period`  | Has a fixed contract duration (`duration_in_months`) |
| `has_pension`          | Entitled to pension                                  |
| `has_annual_increase`  | Gets annual salary increment                         |


### Job Grade (`job_grades`)

- `grade_level` = integer ranking (higher = more senior)
- `years_no` = years required before promotion to next grade

### Decision Types (`decision_types`)

Flags drive automatic downstream effects:

- `affects_employment_type` → update `employees.employment_type_id`
- `affects_salary` → trigger payroll module
- `affects_position` → update `employees.job_title_id` or `org_unit_id`
- `has_end_date` → decision expires; schedule re-evaluation

### Leadership Positions

A `leadership_position` is a post (e.g., "مدير عام") tied to an org unit.
`leadership_position_histories` tracks every person who ever held it:

```sql
-- Current holder = record with end_date IS NULL
SELECT e.name
FROM leadership_position_histories lph
JOIN employees e ON e.id = lph.employee_id
WHERE lph.leadership_position_id = ? AND lph.end_date IS NULL
```

### Academic Incentive Requests

- Linked to an existing `employee_qualifications` record
- `academic_incentive_types.is_percentage` vs `is_fixed_value` determines calculation method
- Requires a linked `decision_id` (the incentive is formalized by a decision)

### Service Termination

- `requires_notice_period` flag on `service_termination_types` triggers notice period logic
- After approval → set `employees.termination_date`

### Audit Logging

`audit_logs` captures every mutation:

```sql
table_name      -- which table changed
record_id       -- PK of the changed row
action_type     -- 'CREATE' | 'UPDATE' | 'DELETE'
change_details  -- XML: before/after field values
changed_by_user_id
change_timestamp
```

Implement via EF Core `SaveChanges` override or DB triggers.

---

## Quick Reference — Table Ownership by Module


| Module           | Tables                                                                                                                                                                                                                                                                                                                                                                                         |
| ---------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **HR**           | employees, employee_files, employee_families, employee_qualifications, employment_types, qualification_types, qualitative_groups, functional_groups, job_titles, job_grades, candidates, nomination_files, decision_types, decision_authorities, decisions, employee_decisions, academic_incentive_types, academic_incentive_requests, service_termination_types, service_termination_requests |
| **Organization** | org_unit_types, org_units, leadership_positions, leadership_position_histories                                                                                                                                                                                                                                                                                                                 |
| **Geography**    | governorates, city_centers, local_units, villages                                                                                                                                                                                                                                                                                                                                              |
| **Common**       | settings, notifications, documents, reviews, audit_logs                                                                                                                                                                                                                                                                                                                                        |


