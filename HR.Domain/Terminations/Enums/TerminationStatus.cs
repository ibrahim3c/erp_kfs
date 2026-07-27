namespace HR.Domain.Terminations.Enums
{
    public enum TerminationStatus
    {
        Executed = 1,   // تم التنفيذ (الموظف موقوف بالفعل)
        Cancelled = 2   // تم إلغاء القرار (نادر، لو حصل خطأ إداري)
    }
}
