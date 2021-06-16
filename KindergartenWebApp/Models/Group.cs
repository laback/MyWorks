using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KindergartenWebApp.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public int StaffId { get; set; }
        public int CountOfChildren { get; set; }
        public int YearOfCreation { get; set; }
        public int GroupType_Id { get; set; }
        public Staff Staff { get; set; }
        public GroupType GroupType { get; set; }
    }
}