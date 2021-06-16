using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TransportUsing.UnitsOfWork;
using Microsoft.Extensions.Configuration;
using WPF.Admin;
using WPF.Driver;

namespace WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UnitOfWork unitOfWork;
        public MainWindow()
        {
            InitializeComponent();
            InitializeUnitOfWork();
        }

        private string GetConnectionString()
        {
            var curDir = Directory.GetCurrentDirectory();

            var config = new ConfigurationBuilder().AddJsonFile($"{curDir}\\config.json").Build();

            string connection = config["ConnectionStrings:DefaultConnection"];

            return connection;
        }

        private void InitializeUnitOfWork()
        {
            unitOfWork = new UnitOfWork(GetConnectionString());
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginText.Text;
            string password = Password.Password;
            var users = unitOfWork.Users.Get();
            var user = users.Where(x => x.UserName.Equals(login)).FirstOrDefault();
            if(user == null)
            {
                MessageBox.Show("Такого пользователя не существует");
                LoginText.Clear();
                Password.Clear();
            }
            else if(!user.Password.Equals(password))
            {
                MessageBox.Show("Неверный пароль");
                LoginText.Clear();
                Password.Clear();
            }
            else
            {
                switch (user.TypeId)
                {
                    case 1:
                        LoginText.Clear();
                        Password.Clear();
                        AdminWindow adminWindow = new AdminWindow(unitOfWork, user);
                        adminWindow.Owner = this;
                        Visibility = Visibility.Collapsed;
                        adminWindow.Show();
                        break;
                    case 2:
                        LoginText.Clear();
                        Password.Clear();
                        OperatorWindow operatorWindow = new OperatorWindow(unitOfWork, user);
                        operatorWindow.Owner = this;
                        Visibility = Visibility.Collapsed;
                        operatorWindow.Show();
                        break;
                    case 3:
                        LoginText.Clear();
                        Password.Clear();
                        DriverWindow driverWindow = new DriverWindow(unitOfWork, user);
                        driverWindow.Owner = this;
                        Visibility = Visibility.Collapsed;
                        driverWindow.Show();
                        break;
                }
            }
        }
    }
}
