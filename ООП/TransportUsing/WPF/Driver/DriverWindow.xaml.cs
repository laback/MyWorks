using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TransportUsing.Entities;
using TransportUsing.UnitsOfWork;

namespace WPF.Driver
{
    /// <summary>
    /// Логика взаимодействия для DriverWindow.xaml
    /// </summary>
    public partial class DriverWindow : Window
    {
        UnitOfWork _unitOfWork;
        User _user;
        public DriverWindow(UnitOfWork unitOfWork, User user)
        {
            _unitOfWork = unitOfWork;
            _user = user;
            InitializeComponent();
            ReadDriverRoutes();
        }

        private void ReadDriverRoutes()
        {
            RoutesGrid.AutoGenerateColumns = false;
            RoutesGrid.SelectionMode = DataGridSelectionMode.Single;
            RoutesGrid.ItemsSource = _unitOfWork.Routes.Get().Where(x => x.UserId == _user.UserId);

            routeDriveCB.DisplayMemberPath = "FIO";
            routeDriveCB.SelectedValuePath = "UserId";
            routeDriveCB.ItemsSource = _unitOfWork.Users.Get().Where(x => x.TypeId == 3);

            routeTransportCB.DisplayMemberPath = "TransportId";
            routeTransportCB.SelectedValuePath = "TransportId";
            routeTransportCB.ItemsSource = _unitOfWork.Transports.Get();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Owner.Visibility = Visibility.Visible;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Owner.Visibility = Visibility.Visible;
        }
    }
}
