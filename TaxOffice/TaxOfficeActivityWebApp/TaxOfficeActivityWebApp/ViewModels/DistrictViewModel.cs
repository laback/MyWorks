using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaxOfficeActivityWebApp.ViewModels
{
    public class DistrictViewModel
    {
        public IEnumerable<District> Districts { get; set; }
        public PageViewModel PageViewModel { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public string DistrictName { get; set; }
    }
}
