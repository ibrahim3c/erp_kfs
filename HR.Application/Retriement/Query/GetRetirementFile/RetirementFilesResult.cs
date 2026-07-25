using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Retriement.Query.GetRetirementFile
{
    public record RetirementFilesResult(
      List<RetirementFileListItemDto> Files,
      int UnderFinancialReviewCount, int AwaitingSignaturesCount,
      int DeliveredCount, int RejectedCount);
}
