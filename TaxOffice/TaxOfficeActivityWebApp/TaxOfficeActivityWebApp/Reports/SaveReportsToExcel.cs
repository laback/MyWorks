using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TaxOfficeActivityWebApp.ViewModels;

namespace TaxOfficeActivityWebApp.Reports
{
    public class SaveReportsToExcel
    {
        string path = Directory.GetParent(Directory.GetCurrentDirectory()) + "\\Reports";

        private void FormQuarterReport(List<QuarterReportViewModel> reports, string newPath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage(new FileInfo(newPath));
            package.Workbook.Worksheets.Add("Деятельность");
            if(package.Workbook.Worksheets[0].Dimension == null)
            {
                package.Workbook.Worksheets[0].Cells[1, 1].Value = "ФИО предпринимателя";
                package.Workbook.Worksheets[0].Cells[1, 2].Value = "Вид деятельности";
                package.Workbook.Worksheets[0].Cells[1, 3].Value = "Доход";
                package.Workbook.Worksheets[0].Cells[1, 4].Value = "Оплачен ли налог";
                int columnEnd = package.Workbook.Worksheets[0].Dimension.End.Row;
                foreach(QuarterReportViewModel q in reports)
                {
                    columnEnd++;
                    package.Workbook.Worksheets[0].Cells[columnEnd, 1].Value = q.FullName;
                    package.Workbook.Worksheets[0].Cells[columnEnd, 2].Value = q.ActivityName;
                    package.Workbook.Worksheets[0].Cells[columnEnd, 3].Value = q.Income;
                    package.Workbook.Worksheets[0].Cells[columnEnd, 4].Value = q.IsTaxPaid == true ? "Да"  : "Нет";
                }
                File.WriteAllBytes(newPath, package.GetAsByteArray());
            }
        }

        public void SaveQuarterReport(int year, int quarter, List<QuarterReportViewModel> reports)
        {
            string dirName = year + " - " + quarter;
            string newPath = path + "\\" + dirName;
            if (Directory.Exists(newPath))
            {
                File.Delete(newPath + "\\Деятельность предпринимателей.xlsx");
            }
            else
                Directory.CreateDirectory(newPath);
            newPath += "\\Деятельность предпринимателей.xlsx";
            FormQuarterReport(reports, newPath);
        }

        private void FormEntrepreneurReport(List<EntrepreneurReportViewModel> reports, string newPath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage(new FileInfo(newPath));
            package.Workbook.Worksheets.Add("Деятельность");
            if (package.Workbook.Worksheets[0].Dimension == null)
            {
                package.Workbook.Worksheets[0].Cells[1, 1].Value = "Вид деятельности";
                package.Workbook.Worksheets[0].Cells[1, 2].Value = "Доход";
                package.Workbook.Worksheets[0].Cells[1, 3].Value = "Год";
                package.Workbook.Worksheets[0].Cells[1, 4].Value = "Квартал";
                package.Workbook.Worksheets[0].Cells[1, 5].Value = "Оплачен ли налог";
                int columnEnd = package.Workbook.Worksheets[0].Dimension.End.Row;
                foreach (EntrepreneurReportViewModel q in reports)
                {
                    columnEnd++;
                    package.Workbook.Worksheets[0].Cells[columnEnd, 1].Value = q.ActivityName;
                    package.Workbook.Worksheets[0].Cells[columnEnd, 2].Value = q.Income;
                    package.Workbook.Worksheets[0].Cells[columnEnd, 3].Value = q.Year;
                    package.Workbook.Worksheets[0].Cells[columnEnd, 4].Value = q.Quarter;
                    package.Workbook.Worksheets[0].Cells[columnEnd, 5].Value = q.IsTaxPaid == true ? "Да" : "Нет";
                }
                File.WriteAllBytes(newPath, package.GetAsByteArray());
            }
        }

        public void SaveEntrepreneurReport(string fullName, List<EntrepreneurReportViewModel> reports)
        {
            string dirName = fullName;
            string newPath = path + "\\" + dirName;
            if (Directory.Exists(newPath))
            {
                File.Delete(newPath + "\\Деятельность предпринимателя.xlsx");
            }
            else
                Directory.CreateDirectory(newPath);
            newPath += "\\Деятельность предпринимателя.xlsx";
            FormEntrepreneurReport(reports, newPath);
        }

        private void FormStatistic(StatisticReportViewModel report, string newPath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var package = new ExcelPackage(new FileInfo(newPath));
            package.Workbook.Worksheets.Add("Статистика");
            if (package.Workbook.Worksheets[0].Dimension == null)
            {
                package.Workbook.Worksheets[0].Cells[1, 1].Value = "Оплачен";
                package.Workbook.Worksheets[0].Cells[1, 2].Value = "Не оплачен";
                int columnEnd = package.Workbook.Worksheets[0].Dimension.End.Row;
                columnEnd++;
                package.Workbook.Worksheets[0].Cells[columnEnd, 1].Value = report.IsPaid + " %";
                package.Workbook.Worksheets[0].Cells[columnEnd, 2].Value = report.IsNotPaid + " %";
                File.WriteAllBytes(newPath, package.GetAsByteArray());
            }
        }

        public void SaveStatistic(int year, StatisticReportViewModel report)
        {
            string dirName = "Статистика за " + year + " год";
            string newPath = path + "\\" + dirName;
            if (Directory.Exists(newPath))
            {
                File.Delete(newPath + "\\Cтатистика.xlsx");
            }
            else
                Directory.CreateDirectory(newPath);
            newPath += "\\Cтатистика.xlsx";
            FormStatistic(report, newPath);
        }
    }
}
