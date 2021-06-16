using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace TaxOfficeActivityWebApp
{
    public partial class TaxOffice
    {
        public TaxOffice()
        {
            Entrepreneurs = new HashSet<Entrepreneur>();
        }

        public int TaxOfficeId { get; set; }
        public int? DistrictId { get; set; }
        [Display(Name = "Наименование налогового офиса")]
        [StringLength(50, MinimumLength = 5)]
        public string TaxOfficeName { get; set; }

        public virtual District District { get; set; }
        public virtual ICollection<Entrepreneur> Entrepreneurs { get; set; }
    }
}
