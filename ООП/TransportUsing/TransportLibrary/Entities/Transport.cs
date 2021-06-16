using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportUsing.Entities
{
    public class Transport
    {
        public int TransportId { get; set; }
        public int TransportTypeId { get; set; }
        public TransportType TransportType { get; set; }
        public int MarkId { get; set; }
        public Mark Mark { get; set; }
    }
}
