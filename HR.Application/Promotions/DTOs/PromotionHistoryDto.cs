using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Promotions.DTOs
{

    public class PromotionHistoryDto
    {
        public Guid Id { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string MovementType { get; set; } = string.Empty;
        public string FromGrade { get; set; } = string.Empty;
        public string ToGrade { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public Guid? LinkedDecisionId { get; set; }
    }

   
}
