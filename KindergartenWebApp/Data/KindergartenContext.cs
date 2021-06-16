using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using KindergartenWebApp.Models;

namespace KindergartenWebApp.Data
{
    public class KindergartenContext : DbContext
    {
        public KindergartenContext()
               : base("name=KindergartenConnectionString")
        {
        }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<GroupType> GroupTypes { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Staff> Staffs { get; set; }
    }
}