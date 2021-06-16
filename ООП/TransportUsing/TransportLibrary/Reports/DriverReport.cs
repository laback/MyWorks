using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportLibrary.Reports
{
    [Serializable]
    public class DriverReport
    {
        public string FIO { get; set; }
        public int Count { get; set; }
        public double AverageDistance { get; set; }
        [DisplayFormat(DataFormatString = "d",
           ApplyFormatInEditMode = true)]
        public DateTime BeginDate { get; set; }
        [DisplayFormat(DataFormatString = "d",
           ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        public DriverReport() { }
    }
}
