using BakeryWebApp.Models;
using BakeryWebApp.SaveReports;
using BakeryWebApp.Unit_of_work;
using BakeryWebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryWebApp.Controllers
{
    [Authorize(Roles = "admin, manager")]
    public class ReportController : Controller
    {
        private UnitOfWork unit;
        private SaveReportToExcel save = new SaveReportToExcel();
        public ReportController(UnitOfWork unit)
        {
            this.unit = unit;
        }
        public IActionResult Index(DateTime dailyDate, DateTime beginDate, DateTime endDate)
        {
            if (dailyDate != default) 
            {
                save.FormDailyReport(dailyDate, GetDailyRawReport(dailyDate), GetDailyMaterialReport(dailyDate));
            }
            if(beginDate != default && endDate != default)
            {
                save.FormPeriodReport(beginDate, endDate, GetPeriodRawReport(beginDate, endDate), GetPeriodMatreialReport(beginDate, endDate));
            }
            return View();
        }

        public List<MaterialReportViewModel> GetDailyMaterialReport(DateTime date)
        {
            List<ProductsMaterial> globalMat = new List<ProductsMaterial>();
            var dayPlan = unit.DayPlans.GetAll().Where(d => d.Date.Equals(date)).FirstOrDefault();
            if(dayPlan == null)
            {
                unit.DayPlans.Create(new DayPlan { Date = date });
                dayPlan = unit.DayPlans.GetAll().Where(d => d.Date.Equals(date)).FirstOrDefault();
            }
            var productsPlans = unit.ProductsPlans.GetAll().Where(d => d.DayPlanId == dayPlan.DayPlanId).ToList();
            foreach (ProductsPlan pp in productsPlans)
            {
                IEnumerable<ProductsMaterial> productsMaterials = unit.ProductMaterials.GetAll().Where(p => p.ProductId == pp.ProductId).ToList();
                foreach (ProductsMaterial pm in productsMaterials)
                {
                    pm.Quantity *= pp.Count;
                    if (globalMat.Contains(pm))
                        globalMat.Where(g => g.MaterialId == pm.MaterialId).FirstOrDefault().Quantity += pm.Quantity;
                    else
                        globalMat.Add(pm);
                }
            }
            var newGlobalMat = globalMat.GroupBy(m => m.MaterialId).Select(m => new { Id = m.Key, Sum = m.Sum(q => q.Quantity) }).ToDictionary(m => m.Id, m => m.Sum);
            List<MaterialReportViewModel> dailyMaterialPlanViewModels = new List<MaterialReportViewModel>();
            foreach (KeyValuePair<int?, double?> keyValue in newGlobalMat)
            {
                MaterialReportViewModel d = new MaterialReportViewModel
                {
                    MaterialName = unit.Materials.GetAll().Where(m => m.MaterialId == keyValue.Key).FirstOrDefault().MaterialName,
                    MaterialQuantity = (double)keyValue.Value
                };
                dailyMaterialPlanViewModels.Add(d);
            }
            return dailyMaterialPlanViewModels;
        }

        public List<RawReportViewModel> GetDailyRawReport(DateTime date)
        {
            List<Norm> globalNorm = new List<Norm>();
            var dayPlan = unit.DayPlans.GetAll().Where(d => d.Date.Equals(date)).FirstOrDefault();
            if (dayPlan == null)
            {
                unit.DayPlans.Create(new DayPlan { Date = date });
                dayPlan = unit.DayPlans.GetAll().Where(d => d.Date.Equals(date)).FirstOrDefault();
            }
            var productsPlans = unit.ProductsPlans.GetAll().Where(d => d.DayPlanId == dayPlan.DayPlanId).ToList();
            foreach (ProductsPlan pp in productsPlans)
            {
                var norms = unit.Norms.GetAll().Where(n => n.ProductId == pp.ProductId);
                foreach (Norm n in norms)
                {
                    n.Quantity *= pp.Count;
                    if (globalNorm.Contains(n))
                        globalNorm.Where(g => g.RawId == n.RawId).FirstOrDefault().Quantity += n.Quantity;
                    else
                        globalNorm.Add(n);
                }
            }
            var newGlobalNorm = globalNorm.GroupBy(n => n.RawId).Select(n => new { Id = n.Key, Sum = n.Sum(q => q.Quantity) }).ToDictionary(n => n.Id, n => n.Sum);
            List<RawReportViewModel> dailyRawPlanViewModels = new List<RawReportViewModel>(newGlobalNorm.Count);
            foreach (KeyValuePair<int?, double?> keyValue in newGlobalNorm)
            {
                RawReportViewModel d = new RawReportViewModel
                {
                    RawName = unit.Raws.GetAll().Where(m => m.RawId == keyValue.Key).FirstOrDefault().RawName,
                    RawQuantity = (double)keyValue.Value
                };
                dailyRawPlanViewModels.Add(d);
            }
            return dailyRawPlanViewModels;
        }

        public List<RawReportViewModel> GetPeriodRawReport(DateTime beginDate = default, DateTime endTime = default)
        {
            List<Norm> globalNorm = new List<Norm>();
            List<RawReportViewModel> periodRawPlanViewModels = new List<RawReportViewModel>();
            IEnumerable<DayPlan> dayPlans;
            if (beginDate != default && endTime != default)
                dayPlans = unit.DayPlans.GetAll().Where(d => d.Date >= beginDate && d.Date <= endTime);
            else
                dayPlans = unit.DayPlans.GetAll();
            foreach (DayPlan dayPlan in dayPlans)
            {
                var productsPlans = unit.ProductsPlans.GetAll().Where(d => d.DayPlanId == dayPlan.DayPlanId).ToList();
                foreach (ProductsPlan pp in productsPlans)
                {
                    var norms = unit.Norms.GetAll().Where(n => n.ProductId == pp.ProductId);
                    foreach (Norm n in norms)
                    {
                        double quantity = (double)n.Quantity;
                        quantity *= (double)pp.Count;
                        if (globalNorm.Contains(n))
                            globalNorm.Where(g => g.RawId == n.RawId).FirstOrDefault().Quantity += quantity;
                        else
                        {
                            Norm norm = new Norm
                            {
                                RawId = n.RawId,
                                Quantity = quantity
                            };
                            globalNorm.Add(norm);
                        }
                    }
                }
            }

            var newGlobalNorm = globalNorm.GroupBy(n => n.RawId).Select(n => new { Id = n.Key, Sum = n.Sum(q => q.Quantity) }).ToDictionary(n => n.Id, n => n.Sum);

            foreach (KeyValuePair<int?, double?> keyValue in newGlobalNorm)
            {
                RawReportViewModel d = new RawReportViewModel
                {
                    RawName = unit.Raws.GetAll().Where(m => m.RawId == keyValue.Key).FirstOrDefault().RawName,
                    RawQuantity = (double)keyValue.Value
                };
                bool flag = true;
                foreach(RawReportViewModel rr in periodRawPlanViewModels)
                {
                    if (rr.RawName.Equals(d.RawName))
                    {
                        rr.RawQuantity += d.RawQuantity;
                        flag = false;
                    }
                }
                if(flag)
                    periodRawPlanViewModels.Add(d);
            }
            return periodRawPlanViewModels;
        }
        public List<MaterialReportViewModel> GetPeriodMatreialReport(DateTime beginDate = default, DateTime endTime = default)
        {
            List<ProductsMaterial> globalMaterial = new List<ProductsMaterial>();
            List<MaterialReportViewModel> periodMaterialPlanViewModels = new List<MaterialReportViewModel>();
            IEnumerable<DayPlan> dayPlans;
            if (beginDate != default && endTime != default)
                dayPlans = unit.DayPlans.GetAll().Where(d => d.Date >= beginDate && d.Date <= endTime);
            else
                dayPlans = unit.DayPlans.GetAll();
            foreach (DayPlan dayPlan in dayPlans)
            {
                var productsPlans = unit.ProductsPlans.GetAll().Where(d => d.DayPlanId == dayPlan.DayPlanId).ToList();
                foreach (ProductsPlan pp in productsPlans)
                {
                    var materials = unit.ProductMaterials.GetAll().Where(n => n.ProductId == pp.ProductId);
                    foreach (ProductsMaterial pm in materials)
                    {
                        double quantity = (double)pm.Quantity;
                        quantity *= (double)pp.Count;
                        if (globalMaterial.Contains(pm))
                            globalMaterial.Where(g => g.MaterialId == pm.MaterialId).FirstOrDefault().Quantity += quantity;
                        else
                        {
                            ProductsMaterial material = new ProductsMaterial
                            {
                                MaterialId = pm.MaterialId,
                                Quantity = quantity
                            };
                            globalMaterial.Add(material);
                        }
                    }
                }
            }

            var newGlobalNorm = globalMaterial.GroupBy(n => n.MaterialId).Select(n => new { Id = n.Key, Sum = n.Sum(q => q.Quantity) }).ToDictionary(n => n.Id, n => n.Sum);

            foreach (KeyValuePair<int?, double?> keyValue in newGlobalNorm)
            {
                MaterialReportViewModel d = new MaterialReportViewModel
                {
                    MaterialName = unit.Materials.GetAll().Where(m => m.MaterialId == keyValue.Key).FirstOrDefault().MaterialName,
                    MaterialQuantity = (double)keyValue.Value
                };
                bool flag = true;
                foreach (MaterialReportViewModel mr in periodMaterialPlanViewModels)
                {
                    if (mr.MaterialName.Equals(d.MaterialName))
                    {
                        mr.MaterialQuantity += d.MaterialQuantity;
                        flag = false;
                    }
                }
                if (flag)
                    periodMaterialPlanViewModels.Add(d);
            }
            return periodMaterialPlanViewModels;
        }

        public IActionResult Statistic()
        {
            save.FormStatistic(GetPeriodRawReport(), GetPeriodMatreialReport());
            return RedirectToAction("Index");
        }
    }
}
