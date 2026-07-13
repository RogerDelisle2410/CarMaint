namespace CarMaint.Models
{

using System;
    using System.Collections.Generic;
    
public partial class CarData
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public CarData()
    {
        this.MaintenanceHistories = new HashSet<MaintenanceHistory>();
    }

    public int CarId { get; set; }

    public int CustomerId { get; set; }

    public string Manufacturer { get; set; }

    public string Model { get; set; }

    public string Year { get; set; }

    public string EngineType { get; set; }

    public string Mileage { get; set; }



    public virtual CustomerData CustomerData { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<MaintenanceHistory> MaintenanceHistories { get; set; }

}

}
