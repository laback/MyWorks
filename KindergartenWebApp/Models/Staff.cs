using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KindergartenWebApp.Models
{
    public class Staff
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Adress { get; set; }
        public int Phone { get; set; }
        public int PositionId { get; set; }
        public string Info { get; set; }
        public string Reward { get; set; }
        public Position Position { get; set; }
        public ICollection<Group> Groups { get; set; }
    }
}