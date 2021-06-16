using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaxOfficeActivityWebApp.ViewModels
{
    public class ActivityViewModel
    {
        public IEnumerable<Activity> Activities { get; set; }
        public PageViewModel PageViewModel { get; set; }
        [StringLength(50, MinimumLength = 0)]
        public string ActivityName { get; set; }
    }
}
