
namespace CarMaint.Models
{

using System;
    using System.Collections.Generic;
    
public partial class MaintenanceType
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public MaintenanceType()
    {

        this.MaintenanceHistories = new HashSet<MaintenanceHistory>();

    }


    public int MaintId { get; set; }

    public string Cost { get; set; }

    public bool Gas { get; set; }

    public bool Diesel { get; set; }

    public bool Electric { get; set; }

    public string TaskName_EN { get; set; }

    public string TaskName_FR { get; set; }

    public string TaskName_ES { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<MaintenanceHistory> MaintenanceHistories { get; set; }

}

}
