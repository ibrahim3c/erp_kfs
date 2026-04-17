using HR.Application.JobStructures.GetJobGradeList;
using HR.Application.JobStructures.GetJobTitleList;
using HR.Application.JobStructures.GetQualitativeGroupList;


namespace HR.Application.JobStructures
{
    public class JobStructureViewModel
    {
        public List<GetJobGradeListResponse> JobGrades = new List<GetJobGradeListResponse>();
        public List<GetJobTitleListResponse> JobTitles = new List<GetJobTitleListResponse>();
        public List<GetQualitativeGroupListResponse> QualitativeGroups = new List<GetQualitativeGroupListResponse>();
    }
}
