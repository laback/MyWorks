using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportUsing.Entities;

namespace TransportUsing.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Mark> Marks { get;}
        IRepository<Route> Routes { get;}
        IRepository<Transport> Transports { get;}
        IRepository<TransportType> TransportTypes { get;}
        IRepository<User> Users { get;}
        IRepository<UserType> UserTypes { get;}
    }
}
