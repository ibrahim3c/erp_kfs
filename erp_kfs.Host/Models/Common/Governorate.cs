using MyERP.Web.Models;
using MyERP.Web.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace erp_kfs.Host.Models.Common
{
    public class Governorate: BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Code { get; set; }

        // Navigation Property
        public ICollection<CityCenter> CityCenters { get; set; }
    }
}
