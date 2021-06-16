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
using System.Windows.Shapes;
using TransportLibrary.Reports;
using TransportUsing.Entities;
using TransportUsing.UnitsOfWork;

namespace WPF.Admin.Reports
{
    /// <summary>
    /// Логика взаимодействия для ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        UnitOfWork _unitOfWork;
        FormingReports fr;
        string path = Directory.GetCurrentDirectory();
        public ReportWindow(UnitOfWork unitOfWork)
        {
            InitializeComponent();
            _unitOfWork = unitOfWork;
            fr = new FormingReports(_unitOfWork);
            SetCB();
            PathLabel.Content = PathLabel.Content + " " + path + "\\report";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
            Owner.Visibility = Visibility.Visible;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Owner.Visibility = Visibility.Visible;
        }

        private void SetCB()
        {
            FIOCB.DisplayMemberPath = "FIO";
            FIOCB.SelectedValuePath = "UserId";
            FIOCB.ItemsSource = _unitOfWork.Users.Get().Where(x => x.TypeId == 3);

            TransportCB.DisplayMemberPath = "TransportId";
            TransportCB.SelectedValuePath = "TransportId";
            TransportCB.ItemsSource = _unitOfWork.Transports.Get();

            MarkCB.DisplayMemberPath = "MarkName";
            MarkCB.SelectedValuePath = "MarkId";
            MarkCB.ItemsSource = _unitOfWork.Marks.Get();

            TypeCB.DisplayMemberPath = "TransportTypeName";
            TypeCB.SelectedValuePath = "TransportTypeId";
            TypeCB.ItemsSource = _unitOfWork.TransportTypes.Get();
        }

        private void SaveDriverReportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime beginDate = (DateTime)BeginDatePicker.SelectedDate;
                DateTime endDate = (DateTime)EndDatePicker.SelectedDate;

                User driver = (User)FIOCB.SelectedItem;
                int id = driver.UserId;

                DriverReport driverReport = fr.GetDriverReport(id, beginDate, endDate);

                SaveToXML.SaveDriverReport(driverReport);

                MessageBox.Show("Отчёт успешно сохранён");
            }
            catch
            {
                MessageBox.Show("Проверьте выбранные данные");
            }
        }

        private void SaveTransportRepostButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime beginDate = (DateTime)BeginDatePicker.SelectedDate;
                DateTime endDate = (DateTime)EndDatePicker.SelectedDate;

                Transport transport = (Transport)TransportCB.SelectedItem;
                int id = transport.TransportId;

                TransportReport transportReport = fr.GetTransportReport(id, beginDate, endDate);

                SaveToXML.SaveTransportReport(transportReport);

                MessageBox.Show("Отчёт успешно сохранён");
            }
            catch
            {
                MessageBox.Show("Проверьте выбранные данные");
            }
        }

        private void SaveMarkReportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime beginDate = (DateTime)BeginDatePicker.SelectedDate;
                DateTime endDate = (DateTime)EndDatePicker.SelectedDate;

                Mark mark = (Mark)MarkCB.SelectedItem;
                int id = mark.MarkId;

                MarkReport markReport = fr.GetMarkReport(id, beginDate, endDate);

                SaveToXML.SaveMarkReport(markReport);

                MessageBox.Show("Отчёт успешно сохранён");
            }
            catch
            {
                MessageBox.Show("Проверьте выбранные данные");
            }
        }

        private void SaveTypeReportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime beginDate = (DateTime)BeginDatePicker.SelectedDate;
                DateTime endDate = (DateTime)EndDatePicker.SelectedDate;

                TransportType type = (TransportType)TypeCB.SelectedItem;
                int id = type.TransportTypeId;

                TypeReport typeReport = fr.GetTypeReport(id, beginDate, endDate);

                SaveToXML.SaveTypeReport(typeReport);

                MessageBox.Show("Отчёт успешно сохранён");
            }
            catch
            {
                MessageBox.Show("Проверьте выбранные данные");
            }
            
        }
    }
}
