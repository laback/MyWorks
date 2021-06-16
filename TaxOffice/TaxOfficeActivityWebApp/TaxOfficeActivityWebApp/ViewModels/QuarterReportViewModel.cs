using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxOfficeActivityWebApp.ViewModels
{
    public class QuarterReportViewModel
    {
        public string FullName { get; set; }
        public string ActivityName { get; set; }
        public decimal Income { get; set; }
        public bool IsTaxPaid { get; set; }
    }
}
