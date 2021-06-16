using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxOfficeActivityWebApp.Repositories;

namespace TaxOfficeActivityWebApp.UnitsOfWork
{
    public class UnitOfWork
    {
        private string _connectionString;

        public UnitOfWork(string connectionString)
        {
            _connectionString = connectionString;
        }

        private ActivityAccountingRepository _activityAccountingRepository;
        private ActivityRepository _activityRepository;
        private DistrictRepository _districtRepository;
        private EntrepreneurRepository _entrepreneurRepository;
        private TaxOfficeRepository _taxFfficeRepository;

        public ActivityAccountingRepository ActivitiesAccounting
        {
            get
            {
                if (_activityAccountingRepository == null)
                    _activityAccountingRepository = new ActivityAccountingRepository(_connectionString);
                return _activityAccountingRepository;
            }
        }

        public ActivityRepository Activities
        {
            get
            {
                if (_activityRepository == null)
                    _activityRepository = new ActivityRepository(_connectionString);
                return _activityRepository;
            }
        }

        public DistrictRepository Districts
        {
            get
            {
                if (_districtRepository == null)
                    _districtRepository = new DistrictRepository(_connectionString);
                return _districtRepository;
            }
        }

        public EntrepreneurRepository Entrepreneurs
        {
            get
            {
                if (_entrepreneurRepository == null)
                    _entrepreneurRepository = new EntrepreneurRepository(_connectionString);
                return _entrepreneurRepository;
            }
        }
        
        public TaxOfficeRepository TaxOffices
        {
            get
            {
                if (_taxFfficeRepository == null)
                    _taxFfficeRepository = new TaxOfficeRepository(_connectionString);
                return _taxFfficeRepository;
            }
        }
    }
}
