using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportLibrary.Reports
{
    [Serializable]
    public class MarkReport
    {
        public string MarkName { get; set; }
        public double Distance { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
           ApplyFormatInEditMode = true)]
        public DateTime BeginDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
           ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
    }
}
