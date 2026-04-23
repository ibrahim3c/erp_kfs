using Dapper;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;


namespace HR.Application.Permissions.GetAttendanceLog
{
    public sealed class GetAttendanceLogQueryHandler
        : IQueryHandler<GetAttendanceLogQuery, GetAttendanceLogResponse>
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public GetAttendanceLogQueryHandler(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<Result<GetAttendanceLogResponse>> Handle(
            GetAttendanceLogQuery request,
            CancellationToken cancellationToken)
        {
            using var connection = _connectionFactory.CreateConnection();

            // ─── Summary ───────────────────────────────────────────

            // Query بيحسب إجمالي الدقائق (Permissions + Late)
            var summarySql = """
                    SELECT 
                        -- مجموع دقائق الأذونات
                        COALESCE(SUM(CASE WHEN src = 'P' THEN dur ELSE 0 END), 0) AS TotalPermissionMinutes,

                        -- مجموع دقائق التأخير
                        COALESCE(SUM(CASE WHEN src = 'L' THEN dur ELSE 0 END), 0) AS TotalLateMinutes

                    FROM ( 
                        -- الجزء الأول: بيانات الأذونات
                        SELECT 
                            'P' AS src,                    -- نحدد نوع الصف (Permission)
                            "DurationMinutes" AS dur       -- عدد الدقائق
                        FROM "HR"."PermissionRequests" 
                        WHERE EXTRACT(MONTH FROM "Date") = @Month   -- فلترة بالشهر
                          AND EXTRACT(YEAR  FROM "Date") = @Year    -- فلترة بالسنة

                        UNION ALL 

                        -- الجزء الثاني: بيانات التأخير
                        SELECT 
                            'L' AS src,                    -- نحدد نوع الصف (Late)
                            "LateMinutes" AS dur           -- عدد دقائق التأخير
                        FROM "HR"."LateEntries" 
                        WHERE EXTRACT(MONTH FROM "Date") = @Month 
                          AND EXTRACT(YEAR  FROM "Date") = @Year 
                    ) combined;                           -- جدول مؤقت مدمج
                """;

            // تنفيذ الـ query وإرجاع أول row (فيه الإجماليات)
            var summary = await connection.QueryFirstAsync(
                summarySql, new { request.Month, request.Year });


            // ─── عدد الموظفين اللي اتعاقبوا 

            var exceededSql = """
                    SELECT COUNT(DISTINCT "EmployeeId")   -- عدد موظفين بدون تكرار
                    FROM "HR"."LateEntries"
                    WHERE EXTRACT(MONTH FROM "Date") = @Month 
                      AND EXTRACT(YEAR  FROM "Date") = @Year 
                      AND "IsTransferredToPenalty" = true;  -- بس اللي اتحولوا لجزاء
                """;

            // تنفيذ query وإرجاع رقم واحد (int)
            var exceeded = await connection.ExecuteScalarAsync<int>(
                exceededSql, new { request.Month, request.Year });


            // ─── Log Items (Permissions + Late مع بعض)

            var logSql = """
                    -- ================= Permissions =================
                    SELECT 
                        pr."Id"                                     AS Id,             -- ID العملية
                        pr."Date"                                   AS Date,           -- التاريخ
                        e."Name"                                    AS EmployeeName,   -- اسم الموظف

                        'Permission'                                AS Type,           -- نوع العملية
                        pr."PermissionType"                         AS SubType,        -- نوع الإذن

                        -- عرض الوقت بشكل readable
                        TO_CHAR(pr."FromTime", 'HH12:MI AM') 
                            || ' : ' 
                            || TO_CHAR(pr."ToTime", 'HH12:MI AM')  AS TimeRange,

                        pr."DurationMinutes"                        AS DurationMinutes, -- عدد الدقائق
                        pr."Notes"                                  AS Notes,           -- ملاحظات

                        -- تحويل النوع لنص مفهوم
                        CASE pr."PermissionType" 
                            WHEN 'Personal' THEN 'مخصوم من الرصيد'
                            WHEN 'Official' THEN 'عمل رسمي'
                            WHEN 'Medical'  THEN 'إذن مرضي'
                        END                                         AS StatusLabel,

                        false                                       AS IsTransferred   -- مفيش تحويل لجزاء هنا

                    FROM "HR"."PermissionRequests" pr 
                    INNER JOIN "HR"."Employees" e 
                        ON pr."EmployeeId" = e."Id"                 -- ربط الموظف

                    WHERE EXTRACT(MONTH FROM pr."Date") = @Month 
                      AND EXTRACT(YEAR  FROM pr."Date") = @Year 


                    UNION ALL 


                    -- ================= Late =================
                    SELECT 
                        le."Id"                                     AS Id,
                        le."Date"                                   AS Date,
                        e."Name"                                    AS EmployeeName,

                        'Late'                                      AS Type,           -- نوع العملية
                        'تأخير صباحي'                              AS SubType,        -- ثابت

                        -- عرض وقت الحضور
                        'حضور ' 
                            || TO_CHAR(le."ActualArrivalTime", 'HH12:MI AM') AS TimeRange,

                        le."LateMinutes"                            AS DurationMinutes,
                        le."Notes"                                  AS Notes,

                        -- حالة التأخير
                        CASE le."IsTransferredToPenalty" 
                            WHEN true THEN 'مرحل للجزاء'
                            ELSE 'قيد التجميع'
                        END                                         AS StatusLabel,

                        le."IsTransferredToPenalty"                 AS IsTransferred  -- هل اتحول لجزاء

                    FROM "HR"."LateEntries" le 
                    INNER JOIN "HR"."Employees" e 
                        ON le."EmployeeId" = e."Id"

                    WHERE EXTRACT(MONTH FROM le."Date") = @Month 
                      AND EXTRACT(YEAR  FROM le."Date") = @Year 

                    -- ترتيب النتائج من الأحدث للأقدم
                    ORDER BY Date DESC; 
                """;

            var items = (await connection.QueryAsync<AttendanceLogItem>(logSql, new { request.Month, request.Year })).ToList();

            var response = new GetAttendanceLogResponse
            {
                TotalPermissionMinutes = (int)summary.TotalPermissionMinutes,
                TotalLateMinutes = (int)summary.TotalLateMinutes,
                EmployeesExceededLimit = exceeded,
                Items = items
            };

            return Result<GetAttendanceLogResponse>.Success(response);
        }
    }
}
