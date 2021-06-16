using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaxOfficeActivityWebApp.Reports;
using TaxOfficeActivityWebApp.UnitsOfWork;
using TaxOfficeActivityWebApp.ViewModels;

namespace TaxOfficeActivityWebApp.Controllers
{
    [Authorize(Roles = "Главный бухгалтер")]
    public class ReportController : Controller
    {
        private UnitOfWork _unit;
        private SaveReportsToExcel _save = new SaveReportsToExcel();

        public ReportController(UnitOfWork unit)
        {
            _unit = unit;
        }
        public IActionResult Index(int year, int quarter, string fullName, int statisticYear)
        {
            ViewData["EntrepreneurId"] = new SelectList(_unit.Entrepreneurs.GetAll(), "EntrepreneurId", "FullName");
            if (year > 0 && quarter > 0)
                GetQuarterReport(year, quarter);
            if (!string.IsNullOrEmpty(fullName))
                GetEntrepreneurReport(fullName);
            if (statisticYear != 0)
                GetStatistic(statisticYear);
            return View();
        }

        public void GetQuarterReport(int year, int quarter)
        {
            if(year >= 1800 && year <= 2200 && quarter >= 1 && quarter <= 4)
            {
                List<QuarterReportViewModel> reports = new List<QuarterReportViewModel>();
                List<ActivityAccounting> list = _unit.ActivitiesAccounting.GetAll().Where(x => x.Year == year && x.Quarter == quarter).ToList();
                foreach (ActivityAccounting aa in list)
                {
                    QuarterReportViewModel report = new QuarterReportViewModel
                    {
                        FullName = aa.Entrepreneur.FullName,
                        ActivityName = aa.Activity.ActivityName,
                        Income = (decimal)aa.Income,
                        IsTaxPaid = aa.IsTaxPaid
                    };
                    reports.Add(report);
                }
                _save.SaveQuarterReport(year, quarter, reports);
            }
        }

        public void GetEntrepreneurReport(string fullName)
        {
            int entrepreneurId = int.Parse(fullName);
            List<EntrepreneurReportViewModel> reports = new List<EntrepreneurReportViewModel>();
            List<ActivityAccounting> list = _unit.ActivitiesAccounting.GetAll().Where(x => x.EntrepreneurId == entrepreneurId).ToList();
            foreach (ActivityAccounting aa in list)
            {
                EntrepreneurReportViewModel report = new EntrepreneurReportViewModel
                {
                    ActivityName = aa.Activity.ActivityName,
                    Income = (decimal)aa.Income,
                    Year = (int)aa.Year,
                    Quarter = (int)aa.Quarter,
                    IsTaxPaid = aa.IsTaxPaid
                };
                reports.Add(report);
            }
            fullName = _unit.Entrepreneurs.Get(entrepreneurId).FullName;
            _save.SaveEntrepreneurReport(fullName, reports);
        }

        public void GetStatistic(int year)
        {
            if(year >= 1900 && year <= 2200)
            {
                var activities = _unit.ActivitiesAccounting.GetAll().Where(x => x.Year == year);
                double count = activities.Count();
                double paidCount = activities.Where(x => x.IsTaxPaid == true).Count();
                double notPaidCount = activities.Where(x => x.IsTaxPaid == false).Count();
                StatisticReportViewModel report = new StatisticReportViewModel
                {
                    IsPaid = paidCount * 100 / count,
                    IsNotPaid = notPaidCount * 100 / count
                };
                _save.SaveStatistic(year, report);
            }
        }
    }
}
