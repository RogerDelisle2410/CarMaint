namespace CarMaint.Models
{

using System;
    using System.Collections.Generic;
    
public partial class MaintenanceHistory
{

    public int HistoryId { get; set; }

    public int CarId { get; set; }

    public int MaintId { get; set; }

    public System.DateTime Date { get; set; }

    public int CustId { get; set; }

    public Nullable<decimal> Cost { get; set; } 

    public virtual MaintenanceType MaintenanceType { get; set; }

    public virtual CustomerData CustomerData { get; set; }

    public virtual CarData CarData { get; set; }

}

}
