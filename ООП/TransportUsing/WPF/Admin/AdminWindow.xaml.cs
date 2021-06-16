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
using WPF.Admin.Reports;

namespace WPF.Admin
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        UnitOfWork _unitOfWork;
        User _user;
        private List<Mark> MarkList;
        private List<Transport> TransportList;
        private List<User> UserList;
        private List<Route> RouteList;
        public AdminWindow(UnitOfWork unitOfWork, User user)
        {
            InitializeComponent();
            _unitOfWork = unitOfWork;
            _user = user;
            UpdateTablesAndCBS();
        }

        private void Window_Closed_1(object sender, EventArgs e)
        {
            Owner.Visibility = Visibility.Visible;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (Tabs.SelectedIndex)
                {
                    case 0:
                        DeleteRoute();
                        break;
                    case 1:
                        DeleteUser();
                        break;
                    case 2:
                        DeleteTransport();
                        break;
                    case 3:
                        DeleteMark();
                        break;
                }
                UpdateTablesAndCBS();
            }
            catch
            {
                UpdateTablesAndCBS();
                MessageBox.Show("Выберите запись для удаления");
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch(Tabs.SelectedIndex)
                {
                    case 0:
                        UpdateRoute();
                        AddRoute();
                        break;
                    case 1:
                        UpdateUser();
                        AddUser();
                        break;
                    case 2:
                        UpdateTransport();
                        AddTransport();
                        break;
                    case 3:
                        UpdateMark();
                        AddMark();
                        break;
                }
                UpdateTablesAndCBS();
            }
            catch(Exception exp)
            {
                UpdateTablesAndCBS();
                MessageBox.Show("Проверьте введённые данные");
            }
        }

        private void UpdateTablesAndCBS()
        {
            MarksGridInitial();

            TransportsGridInitial();

            UsersGridInitial();

            RoutesGridInitial();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Owner.Visibility = Visibility.Visible;
        }

        private void Reports_Click(object sender, RoutedEventArgs e)
        {
            ReportWindow reportWindow = new ReportWindow(_unitOfWork);
            reportWindow.Owner = this;
            Visibility = Visibility.Collapsed;
            reportWindow.Show();
        }

        private void RoutesGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
