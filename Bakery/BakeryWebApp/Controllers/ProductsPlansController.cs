using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BakeryWebApp;
using Microsoft.Extensions.Caching.Memory;
using BakeryWebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using BakeryWebApp.Unit_of_work;

namespace BakeryWebApp.Controllers
{
    [Authorize(Roles = "admin, manager")]
    public class ProductsPlansController : Controller
    {
        private readonly UnitOfWork unit;
        private IMemoryCache _memoryCache;

        public ProductsPlansController(UnitOfWork unit, IMemoryCache cache)
        {
            this.unit = unit;
            _memoryCache = cache;
        }

        public IActionResult Index(DateTime planDate, SortState sortState, int countOfProd, string productName, int page = 1)
        {
            IEnumerable<ProductsPlan> productsPlans = unit.ProductsPlans.GetAll();
            if (planDate == default)
                _memoryCache.TryGetValue("productPlanDate", out planDate);
            else
            {
                _memoryCache.Set("productPlanDate", planDate);
                productsPlans = productsPlans.Where(p => p.DayPlan.Date.Equals(planDate));
            }
            if (productName == null)
                _memoryCache.TryGetValue("productNamePlan", out productName);
            else
            {
                _memoryCache.Set("productNamePlan", productName);
                productsPlans = productsPlans.Where(n => n.Product.ProductName.Contains(productName));
            }
            if (countOfProd == 0)
                _memoryCache.TryGetValue("countProductPlan", out countOfProd);
            else
            {
                _memoryCache.Set("countProductPlan", countOfProd);
                productsPlans = productsPlans.Where(p => p.Count == countOfProd);
            }
            if (sortState == SortState.No)
                _memoryCache.TryGetValue("productPlanSort", out sortState);
            else
            {
                _memoryCache.Set("productPlanSort", sortState);
                productsPlans = Sort(productsPlans, sortState);
            }
            int count = productsPlans.Count();
            int pageSize = 15;
            productsPlans = productsPlans.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ProductsPlansViewModel viewModel = new ProductsPlansViewModel
            {
                ProductsPlans = productsPlans,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortState),
                PlanDate = planDate,
                ProductName = productName,
                Count = countOfProd

            };
            return View(viewModel);
        }

        public IActionResult Create()
        {
            ViewData["DayPlanId"] = new SelectList(unit.DayPlans.GetAll(), "DayPlanId", "Date");
            ViewData["ProductId"] = new SelectList(unit.Products.GetAll(), "ProductId", "ProductName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductsPlansViewModel productsPlansViewModel)
        {
            DayPlan dayPlans = unit.DayPlans.GetAll().Where(d => d.Date.Equals(productsPlansViewModel.Date)).FirstOrDefault();
            if (dayPlans == null)
            {
                unit.DayPlans.Create(new DayPlan { Date = productsPlansViewModel.Date });
                dayPlans = unit.DayPlans.GetAll().Where(d => d.Date.Equals(productsPlansViewModel.Date)).FirstOrDefault();
            }
            ProductsPlan productsPlan = productsPlansViewModel.ProductsPlan;
            productsPlan.DayPlanId = dayPlans.DayPlanId;
            if (ModelState.IsValid)
            {
                unit.ProductsPlans.Create(productsPlan);
                return RedirectToAction(nameof(Index));
            }
            ViewData["DayPlanId"] = new SelectList(unit.DayPlans.GetAll(), "DayPlanId", "Date");
            ViewData["ProductId"] = new SelectList(unit.Products.GetAll(), "ProductId", "ProductName");
            return View(productsPlan);
        }

        public IActionResult Edit(int id)
        {

            var productsPlan = unit.ProductsPlans.Get(id);
            ProductsPlansViewModel productsPlansViewModel = new ProductsPlansViewModel
            {
                ProductsPlan = productsPlan
            };
            if (productsPlan == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(unit.Products.GetAll(), "ProductId", "ProductName");
            return View(productsPlansViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ProductsPlansViewModel productsPlansViewModel)
        {
            if (id != productsPlansViewModel.ProductsPlan.ProductPlan)
            {
                return NotFound();
            }
            DayPlan dayPlans = unit.DayPlans.GetAll().Where(d => d.Date.Equals(productsPlansViewModel.Date)).FirstOrDefault();
            if (dayPlans == null)
            {
                unit.DayPlans.Create(new DayPlan { Date = productsPlansViewModel.Date });
                dayPlans = unit.DayPlans.GetAll().Where(d => d.Date.Equals(productsPlansViewModel.Date)).FirstOrDefault();
            }
            ProductsPlan productsPlan = productsPlansViewModel.ProductsPlan;
            productsPlan.DayPlanId = dayPlans.DayPlanId;
            if (ModelState.IsValid)
            {
                try
                {
                    unit.ProductsPlans.Update(productsPlan);
                }
                catch (DbUpdateConcurrencyException)
                {
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DayPlanId"] = new SelectList(unit.DayPlans.GetAll(), "DayPlanId", "Date");
            ViewData["ProductId"] = new SelectList(unit.Products.GetAll(), "ProductId", "ProductName");
            return View(productsPlan);
        }

        public IActionResult Delete(int id)
        {

            var productsPlan = unit.ProductsPlans.Get(id);
            if (productsPlan == null)
            {
                return NotFound();
            }

            return View(productsPlan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var productsPlan = unit.ProductsPlans.Get(id);
            unit.ProductsPlans.Delete(productsPlan);
            return RedirectToAction(nameof(Index));
        }
        private IEnumerable<ProductsPlan> Sort(IEnumerable<ProductsPlan> productsPlans, SortState sortState)
        {
            switch (sortState)
            {
                case SortState.ProductPlanDateAcs:
                    productsPlans = productsPlans.OrderBy(d => d.DayPlan.Date);
                    break;
                case SortState.ProductPlanDateDecs:
                    productsPlans = productsPlans.OrderByDescending(d => d.DayPlan.Date);
                    break;
                case SortState.ProductPlanProductAcs:
                    productsPlans = productsPlans.OrderBy(p => p.Product.ProductName);
                    break;
                case SortState.ProductPlanProductDecs:
                    productsPlans = productsPlans.OrderByDescending(p => p.Product.ProductName);
                    break;
                case SortState.ProductPlanCountAcs:
                    productsPlans = productsPlans.OrderBy(c => c.Count);
                    break;
                case SortState.ProductPlanCountDecs:
                    productsPlans = productsPlans.OrderByDescending(c => c.Count);
                    break;
            }
            return productsPlans;
        }

        public IActionResult ClearFilter()
        {
            _memoryCache.Remove("productPlanDate");
            _memoryCache.Remove("productNamePlan");
            _memoryCache.Remove("countProductPlan");
            _memoryCache.Remove("productPlanSort");
            return RedirectToAction("Index");
        }
    }
}
