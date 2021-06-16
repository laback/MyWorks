using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace TaxOfficeActivityWebApp
{
    public partial class ActivityAccounting
    {
        public int ActivityAccountingId { get; set; }
        public int? EntrepreneurId { get; set; }
        public int? ActivityId { get; set; }
        [Display(Name = "Год")]
        [Range(1900, 2200)]
        public int? Year { get; set; }
        [Display(Name = "Квартал")]
        [Range(1, 4)]
        public int? Quarter { get; set; }
        [Display(Name = "Доход предпринимателя")]
        [Range(1, 100000)]
        public decimal? Income { get; set; }
        [Display(Name = "Уплачен ли налог")]
        public bool IsTaxPaid { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual Entrepreneur Entrepreneur { get; set; }
    }
}
