using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportUsing.Entities;
using TransportUsing.UnitsOfWork;

namespace TransportLibrary.Reports
{
    public class FormingReports
    {
        UnitOfWork _unitOfWork;
        public FormingReports(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public DriverReport GetDriverReport(int id, DateTime beginDate, DateTime endDate)
        {
            string FIO = _unitOfWork.Users.Get(id).FIO;

            var routes = _unitOfWork.Routes.Get();

            int count = routes.Where(x => x.UserId == id && x.Date <= endDate && x.Date >= beginDate).Count();

            double average = 0;

            if(count != 0)
                average = routes.Where(x => x.UserId == id && x.Date <= endDate && x.Date >= beginDate).Sum(x => x.Distance) / count;

            DriverReport driverReport = new DriverReport
            {
                FIO = FIO,
                Count = count,
                AverageDistance = average,
                BeginDate = beginDate,
                EndDate = endDate
            };

            return driverReport;
        }

        public TransportReport GetTransportReport(int id, DateTime beginDate, DateTime endDate)
        {
            var routes = _unitOfWork.Routes.Get();
            int distance = routes.Where(x => x.TransportId == id && x.Date <= endDate && x.Date >= beginDate).Sum(x => x.Distance);

            TransportReport transportReport = new TransportReport
            {
                TransportId = id,
                Distance = distance,
                BeginDate = beginDate,
                EndDate = endDate
            };

            return transportReport;
        }

        public MarkReport GetMarkReport(int id, DateTime beginDate, DateTime endDate)
        {
            var routes = _unitOfWork.Routes.Get();

            var transports = _unitOfWork.Transports.Get();

            foreach(var route in routes)
            {
                route.Transport = transports.Where(x => x.TransportId == route.TransportId).FirstOrDefault();
            }

            var result = routes.Where(x => x.Transport.MarkId == id && x.Date <= endDate && x.Date >= beginDate).Sum(x => x.Distance);

            string markName = _unitOfWork.Marks.Get(id).MarkName;

            MarkReport markReport = new MarkReport
            {
                MarkName = markName,
                Distance = result,
                BeginDate = beginDate,
                EndDate = endDate
            };

            return markReport;
        }

        public TypeReport GetTypeReport(int id, DateTime beginDate, DateTime endDate)
        {
            var routes = _unitOfWork.Routes.Get();

            var transports = _unitOfWork.Transports.Get();

            foreach (var route in routes)
            {
                route.Transport = transports.Where(x => x.TransportId == route.TransportId).FirstOrDefault();
            }

            var passengers = routes.Where(x => x.Transport.TransportTypeId == id && x.Date <= endDate && x.Date >= beginDate).Sum(x => x.Passengers);
            var cargos = routes.Where(x => x.Transport.TransportTypeId == id && x.Date <= endDate && x.Date >= beginDate).Sum(x => x.Cargo);

            TypeReport typeReport = new TypeReport
            {
                TypeName = _unitOfWork.UserTypes.Get().Where(x => x.TypeId == id).FirstOrDefault().TypeName,
                Passengers = passengers,
                Cargos = cargos,
                BeginDate = beginDate,
                EndDate = endDate
            };

            return typeReport;
        }
    }
}
