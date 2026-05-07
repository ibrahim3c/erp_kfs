// public apis so the modules can be used by other modules without any dependency issues.
we will use Synchronous Method Calls via a Shared Interface (simpler way)
for example when creating employee we need to create user too HR Module needs Identity module
1- Create an Interface in the Shared Module
Modules.Shared.Application/Interfaces/IIdentityService.cs
2- Implement it in the Identity Module and register it
Modules.Identity.Infrastructure/Services/EmployeeIdentityService.cs
3- Call it Directly from the HR Module

// In a modular monolith with multiple DbContexts, operations across modules can cause data inconsistency if one succeeds and the other fails.
Example: Employee created but User creation fails → inconsistent data.
Three main solutions:
1-Outbox Pattern (Best architecture)
Save the event with the employee in the same transaction, then a background job publishes it later.
→ Eventual consistency, reliable but more complex.
2-Shared DbContext (Pragmatic for monoliths)
Use one DbContext for cross-module operations and commit everything in one transaction.
→ Simple and atomic but slightly reduces module isolation.
3-TransactionScope
Wrap multiple DbContexts in one transaction.
→ Atomic but may require MSDTC (which is slow and make cuase errors) and adds complexity.
Practical advice:
Use Shared DbContext for tightly coupled operations (like Employee + User creation).
Use Outbox Pattern for cross-module events (notifications, integrations).
but i will use Compensating Action for simplicity in this case,
if user creation fails i will delete the employee that was created to maintain consistency.
but still what if deleting employee fails? and so on
i hope in the future i can implement the outbox pattern to make it more robust and scalable or saga pattern in microservices.

// solve qualification path exist in employee qualification and employeeFiles

// Validation - MVC Integration
FluentValidation works seamlessly with asp-validation-for tags just like Data Annotations.
You only need to register the auto-validation and client-side adapters in your Program.cs. - Clean Architecture Separation:
Place basic UI validation (like NotEmpty) inside your MVC ViewModels,
and keep your core business validation rules inside your MediatR Commands in the Application Layer. - MediatR Validation Pipeline:
Use an IPipelineBehavior with ValidateAsync to automatically validate commands before they reach the handler.
If validation fails, throw a custom ValidationException (Fail-Fast) rather than trying to cast generic result types, which causes runtime crashes. - Error Handling:
Do not use Global Exception Middleware for MVC validation, because it redirects the user and erases their form data.
Instead, use a global IAsyncActionFilter. It catches the ValidationException in the background, maps the errors to the ModelState, and returns the View so the user can see the errors and fix their inputs without losing their data.

// Qaulification File Path
i don't know how to deal with file path in employee files and employee qualifications
because they both have file path and they are related to employee
EmployeeFile.QualificationFile → General HR document for the employee.
EmployeeQualification.FilePath → File attached to a specific qualification.
Cardinality
EmployeeFile.QualificationFile → one per employee.
EmployeeQualification.FilePath → one per qualification.
When used
EmployeeFile.QualificationFile → during onboarding / HR intake.
EmployeeQualification.FilePath → when adding a qualification.
Important rule
Do not store the same file path in two places.
If they represent the same file
Remove EmployeeFile.QualificationFile.
Use only EmployeeQualification.FilePath.

// when delete employee u can use background service to delete the employee files or using domain events to trigger the deletion of files after the employee is deleted.

// TODO:
check validations in create employee



// for attendance
📌 Queries
GetDailyAttendance
→ returns all employees attendance for a specific day (table data)
GetDailyAttendanceStatsQuery
→ returns dashboard widgets (present, absent, late, total)
GetEmployeesLookupQuery
→ returns employees list for the dropdown (manual entry modal)
GetAbsenceReportQuery
→ returns absence report data for printing/export
📌 Commands
CreateManualAttendanceCommand
→ manual check-in / check-out from modal
UpdateAttendanceRecordCommand
→ edit attendance time or notes
ImportAttendanceFromDeviceCommand
→ import data from fingerprint machine
ConvertLateToPermissionCommand
→ convert late case into permission request
ConvertAbsenceToVacationCommand
→ convert absence into vacation/sick leave