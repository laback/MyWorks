using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxOfficeActivityWebApp.ViewModels
{
    public class EntrepreneurReportViewModel
    {
        public string ActivityName { get; set; }
        public decimal Income { get; set; }
        public int Year { get; set; }
        public int Quarter { get; set; }
        public bool IsTaxPaid { get; set; }
    }
}
