using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Permissions
{
    public class GetMonthlyStatsDto
    {
        public int Count { get; set; }
        public int TotalMinutes { get; set; }
    }
}
