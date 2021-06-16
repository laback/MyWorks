using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaxOfficeActivityWebApp.ViewModels
{
    public class CreateUserViewModel
    {
        [StringLength(50, MinimumLength = 5)]
        public string Email { get; set; }
        [StringLength(50, MinimumLength = 5)]
        public string Password { get; set; }
        public int Year { get; set; }
    }
}
