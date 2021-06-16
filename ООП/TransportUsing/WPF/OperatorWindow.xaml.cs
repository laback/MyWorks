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

namespace WPF
{
    /// <summary>
    /// Логика взаимодействия для OperatorWindow.xaml
    /// </summary>
    public partial class OperatorWindow : Window
    {
        UnitOfWork _unitOfWork;
        User _user;
        private List<Route> RouteList;
        private List<Transport> TransportList;
        public OperatorWindow(UnitOfWork unitOfWork, User user)
        {
            InitializeComponent(); 
            _unitOfWork = unitOfWork;
            _user = user;
            UpdateTablesAndCBS();
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

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (Tabs.SelectedIndex)
                {
                    case 0:
                        DeleteRoute();
                        break;
                    case 1:
                        DeleteTransport();
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

        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (Tabs.SelectedIndex)
                {
                    case 0:
                        UpdateRoute();
                        AddRoute();
                        break;
                    case 1:
                        UpdateTransport();
                        AddTransport();
                        break;
                }
                UpdateTablesAndCBS();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
                UpdateTablesAndCBS();
                MessageBox.Show("Проверьте введённые данные");
            }
        }

        private void UpdateTablesAndCBS()
        {
            TransportsGridInitial();

            RoutesGridInitial();
        }
    }
}
