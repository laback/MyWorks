using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KindergartenWebApp.Models
{
    public class Position
    {
        public int Id { get; set; }
        public string PositionName { get; set; }
        public ICollection<Staff> Staffs { get; set; }
    }
}