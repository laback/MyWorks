using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportUsing.Entities;
using TransportUsing.Interfaces;
using TransportUsing.Repositories;

namespace TransportUsing.UnitsOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly string _connectionString;
        public UnitOfWork(string connectionString)
        {
            _connectionString = connectionString;
        }

        private MarkRepository _marks;
        private RouteRepository _routes;
        private TransportRepository _transports;
        private TransportTypeRepository _transportTypes;
        private UserRepository _users;
        private UserTypeRepository _userTypes;

        public IRepository<Mark> Marks
        {
            get
            {
                if (_marks == null)
                    _marks = new MarkRepository(_connectionString);
                return _marks;
            }
        }

        public IRepository<Route> Routes
        {
            get
            {
                if (_routes == null)
                    _routes = new RouteRepository(_connectionString);
                return _routes;
            }
        }

        public IRepository<Transport> Transports
        {
            get
            {
                if (_transports == null)
                    _transports = new TransportRepository(_connectionString);
                return _transports;
            }
        }

        public IRepository<TransportType> TransportTypes
        {
            get
            {
                if (_transportTypes == null)
                    _transportTypes = new TransportTypeRepository(_connectionString);
                return _transportTypes;
            }
        }

        public IRepository<User> Users
        {
            get
            {
                if (_users == null)
                    _users = new UserRepository(_connectionString);
                return _users;
            }
        }

        public IRepository<UserType> UserTypes
        {
            get
            {
                if (_userTypes == null)
                    _userTypes = new UserTypeRepository(_connectionString);
                return _userTypes;
            }
        }
    }
}
