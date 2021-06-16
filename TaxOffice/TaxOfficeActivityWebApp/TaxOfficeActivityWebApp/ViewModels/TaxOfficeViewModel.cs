using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaxOfficeActivityWebApp.ViewModels
{
    public class TaxOfficeViewModel
    {
        public IEnumerable<TaxOffice> TaxOffices { get; set; }
        public PageViewModel PageViewModel { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public string DistrictName { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public string TaxOfficeName { get; set; }
    }
}
