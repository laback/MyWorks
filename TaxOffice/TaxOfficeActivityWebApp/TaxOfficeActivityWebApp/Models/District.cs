using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace TaxOfficeActivityWebApp
{
    public partial class District
    {
        public District()
        {
            TaxOffices = new HashSet<Entrepreneur>();
        }

        public int DistrictId { get; set; }
        [Display(Name = "Наименование района")]
        [StringLength(50, MinimumLength = 5)]
        public string DistrictName { get; set; }

        public virtual ICollection<Entrepreneur> TaxOffices { get; set; }
    }
}
