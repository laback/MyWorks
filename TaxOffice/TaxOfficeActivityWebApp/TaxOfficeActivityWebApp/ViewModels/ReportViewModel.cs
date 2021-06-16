using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaxOfficeActivityWebApp.ViewModels
{
    public class ReportViewModel
    {
        public int Year { get; set; }
        public int Quarter { get; set; }
        [StringLength(50, MinimumLength = 10)]
        public string FullName { get; set; }
        [Range(1900, 2200)]
        public int? StatisticYear { get; set; }
    }
}
