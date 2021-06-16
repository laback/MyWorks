using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace TaxOfficeActivityWebApp
{
    public partial class Entrepreneur
    {
        public Entrepreneur()
        {
            ActivitiesAccounting = new HashSet<ActivityAccounting>();
        }

        public int EntrepreneurId { get; set; }
        public int? TaxOfficeId { get; set; }
        [Display(Name = "ФИО предпринимателя")]
        [StringLength(50, MinimumLength = 10)]
        public string FullName { get; set; }

        public virtual TaxOffice TaxOffice { get; set; }
        public virtual ICollection<ActivityAccounting> ActivitiesAccounting { get; set; }
    }
}
