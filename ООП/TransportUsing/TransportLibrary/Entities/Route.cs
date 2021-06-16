    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportUsing.Entities
{
    public class Route
    {
        public int RouteId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int Distance { get; set; }
        public DateTime Date { get; set; }
        public int TransportId { get; set; }
        public Transport Transport { get; set; }
        public int Passengers { get; set; }
        public int Cargo { get; set; }
    }
}
