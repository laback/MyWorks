using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace TaxOfficeActivityWebApp
{
    public partial class Activity
    {
        public Activity()
        {
            ActivitiesAccounting = new HashSet<ActivityAccounting>();
        }

        public int ActivityId { get; set; }
        [Display(Name = "Наименование вида деятельности")]
        [StringLength(50, MinimumLength = 5)]
        public string ActivityName { get; set; }
        [Display(Name = "Процент налога")]
        [Range(1.00, 100.00)]
        public decimal? Tax { get; set; }

        public virtual ICollection<ActivityAccounting> ActivitiesAccounting { get; set; }
    }
}
