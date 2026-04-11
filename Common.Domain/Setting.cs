using Modules.Shared.Domain;

namespace Common.Domain
{
    public class Setting: Entity
    {
        public string Name { get; set; }
        public string Logo { get; set; }

        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

        public DateTime? DeletedAt { get; set; }
        public int? DeletedBy { get; set; }
    }
}
