using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TransportUsing.Entities;

namespace WPF.Admin
{
    public partial class AdminWindow
    {
        private void RoutesGridInitial()
        {
            RouteList = (List<Route>)_unitOfWork.Routes.Get();
            RoutesGrid.AutoGenerateColumns = false;
            RoutesGrid.SelectionMode = DataGridSelectionMode.Single;
            RoutesGrid.ItemsSource = RouteList;

            routeDriveCB.DisplayMemberPath = "FIO";
            routeDriveCB.SelectedValuePath = "UserId";
            routeDriveCB.ItemsSource = _unitOfWork.Users.Get().Where(x => x.TypeId == 3);

            routeTransportCB.DisplayMemberPath = "TransportId";
            routeTransportCB.SelectedValuePath = "TransportId";
            routeTransportCB.ItemsSource = _unitOfWork.Transports.Get();
        }
        private void DeleteRoute()
        {
            Route route = (Route)RoutesGrid.SelectedItem;
            _unitOfWork.Routes.Delete(route.RouteId);
        }

        private void AddRoute()
        {
            List<Transport> transports = (List<Transport>)_unitOfWork.Transports.Get();
            List<User> users = (List<User>)_unitOfWork.Users.Get();

            int count = _unitOfWork.Routes.Get().Count();
            for (int i = count; i < RouteList.Count; i++)
            {
                Transport transport = transports.Where(x => x.TransportId == RouteList[i].TransportId).FirstOrDefault(); ;
                RouteList[i].Transport = transport;
                User user = users.Where(x => x.UserId == RouteList[i].UserId).FirstOrDefault();
                RouteList[i].User = user;
                if (CheckUserAndTransportAvailability(RouteList[i]))
                    _unitOfWork.Routes.Create(RouteList[i]);
            }
        }

        private void UpdateRoute()
        {
            List<User> users = (List<User>)_unitOfWork.Users.Get();
            Route route = (Route)RoutesGrid.SelectedItem;
            User user = users.Where(x => x.UserId == route.UserId).FirstOrDefault();
            route.User = user;
            Transport transport = _unitOfWork.Transports.Get(route.TransportId);
            route.Transport = transport;
            if (CheckUserAndTransportAvailability(route))
                _unitOfWork.Routes.Update(route);
        }

        private bool CheckUserAndTransportAvailability(Route route)
        {
            bool isAvailable = true;
            DateTime date = route.Date;
            var routes = _unitOfWork.Routes.Get().Where(x => x.Date.Equals(date) && x.RouteId != route.RouteId);
            string message = "Для маршрута " + route.User.FIO + ";" + route.Distance + ";" + route.Date.ToString("d") + ";" + route.TransportId + ";" + route.Passengers + ";" + route.Cargo + "\n";
            var driver = routes.Where(x => x.UserId == route.UserId).FirstOrDefault();
            if(driver != null)
            {
                message += "Водитель уже занят.\n";
                isAvailable = false;
            }
            var transport = routes.Where(x => x.TransportId == route.TransportId).FirstOrDefault();
            if(transport != null)
            {
                message += "Транспорт уже занят.\n";
                isAvailable = false;
            }
            isAvailable = CheckTransportType(route, message, isAvailable);
            return isAvailable;
        }

        private bool CheckTransportType(Route route, string message, bool isTrue)
        {
            if (route.Transport.TransportTypeId == 1 && route.Cargo != 0)
            {
                isTrue = false;
                message += "Транспорт пассажирского типа не может перевозить грузы.";
            }
            else if(route.Transport.TransportTypeId == 2 && route.Passengers != 0)
            {
                isTrue = false;
                message += "Транспорт грузового типа не может перевозить пассажиров.";
            }
            if (!isTrue)
                MessageBox.Show(message);
            return isTrue;
        }
    }
}
