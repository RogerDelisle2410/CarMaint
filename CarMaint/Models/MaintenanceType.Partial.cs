using System.ComponentModel.DataAnnotations.Schema;

namespace CarMaint.Models
{
    public partial class MaintenanceType
    {
        [NotMapped]
        public string TaskName { get; set; }
    }
}
