using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Terminations.Query.List
{
    public record TerminationsResult(
      List<TerminationListItemDto> Decisions,
      int ResignationCount, int DismissalCount, int AbsenceCount, int DeathCount);

}
