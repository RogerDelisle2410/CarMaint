using System.Collections.Generic;

namespace CarMaint.Models
{
    public class CustomerDetailsViewModel
    {
        public CustomerData Customer { get; set; }
        public List<CarData> Cars { get; set; }
    }
}
