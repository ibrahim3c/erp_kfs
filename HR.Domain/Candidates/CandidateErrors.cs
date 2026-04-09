using Modules.Shared.Domain;

namespace HR.Domain.Candidates
{
    public static class CourseErrors
    {
        public static readonly Error CodeEmpty = new("Course.CodeEmpty", "Course code cannot be empty.");
        public static readonly Error CodeInvalidFormat = new("Course.CodeInvalid", "Course code must follow format 'ABC 123' (3 Letters + 3 Digits).");

    }
}