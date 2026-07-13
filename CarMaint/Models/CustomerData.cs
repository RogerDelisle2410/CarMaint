
namespace CarMaint.Models
{

using System;
    using System.Collections.Generic;
    
public partial class CustomerData
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public CustomerData()
    {

        this.MaintenanceHistories = new HashSet<MaintenanceHistory>();

        this.CarDatas = new HashSet<CarData>();

    }


    public int CustomerId { get; set; }

    public string Name { get; set; }

    public string Address { get; set; }

    public string City { get; set; }

    public string PostalCode { get; set; }

    public string Phone { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<MaintenanceHistory> MaintenanceHistories { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<CarData> CarDatas { get; set; }

}

}
