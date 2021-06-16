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
    public class ProductsProductionsController : Controller
    {
        private readonly UnitOfWork unit;
        private IMemoryCache _memoryCache;

        public ProductsProductionsController(UnitOfWork unit, IMemoryCache cache)
        {
            this.unit = unit;
            _memoryCache = cache;
        }

        public IActionResult Index(DateTime productionDate, string productName, SortState sortState, int countOfProd, int page = 1)
        {
            IEnumerable<ProductsProduction> productsProductions = unit.ProductsProductions.GetAll();
            if (productionDate == default)
                _memoryCache.TryGetValue("productProductionDate", out productionDate);
            else
            {
                _memoryCache.Set("productProductionDate", productionDate);
                productsProductions = productsProductions.Where(p => p.DayProduction.Date.Equals(productionDate));
            }
            if (productName == null)
                _memoryCache.TryGetValue("productProductionName", out productName);
            else
            {
                _memoryCache.Set("productProductionName", productName);
                productsProductions = productsProductions.Where(n => n.Product.ProductName.Contains(productName));
            }
            if (countOfProd == 0)
                _memoryCache.TryGetValue("countProductProduction", out countOfProd);
            else
            {
                _memoryCache.Set("countProductProduction", countOfProd);
                productsProductions = productsProductions.Where(p => p.Count == countOfProd);
            }
            if (sortState == SortState.No)
                _memoryCache.TryGetValue("productProductionSort", out sortState);
            else
            {
                _memoryCache.Set("productProductionSort", sortState);
                productsProductions = Sort(productsProductions, sortState);
            }
            int count = productsProductions.Count();
            int pageSize = 15;
            productsProductions = productsProductions.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ProductProdactionViewModel viewModel = new ProductProdactionViewModel
            {
                ProductsProductions = productsProductions,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortState),
                ProductionDate = productionDate,
                ProductName = productName,
                Count = countOfProd

            };
            return View(viewModel);
        }

        public IActionResult Create()
        {
            ViewData["DayProductionId"] = new SelectList(unit.DayProductions.GetAll(), "DayProductionId", "Date");
            ViewData["ProductId"] = new SelectList(unit.Products.GetAll(), "ProductId", "ProductName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductProdactionViewModel productProdactionViewModel)
        {
            DayProduction dayProduction = unit.DayProductions.GetAll().Where(d => d.Date.Equals(productProdactionViewModel.Date)).FirstOrDefault();
            if (dayProduction == null)
            {
                unit.DayProductions.Create(new DayProduction { Date = productProdactionViewModel.Date });
                dayProduction = unit.DayProductions.GetAll().Where(d => d.Date.Equals(productProdactionViewModel.Date)).FirstOrDefault();
            }
            ProductsProduction productsProduction = productProdactionViewModel.ProductsProduction;
            productsProduction.DayProductionId = dayProduction.DayProductionId;
            unit.ProductsProductions.Create(productsProduction);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var productProduction = unit.ProductsProductions.Get(id);
            ProductProdactionViewModel productProdactionViewModel = new ProductProdactionViewModel
            {
                ProductsProduction = productProduction
            };
            var productsProduction = unit.ProductsProductions.Get(id);
            if (productsProduction == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(unit.Products.GetAll(), "ProductId", "ProductName");
            return View(productProdactionViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ProductProdactionViewModel productProdactionViewModel)
        {
            if (id != productProdactionViewModel.ProductsProduction.ProductProductionId)
            {
                return NotFound();
            }
            DayProduction dayProduction = unit.DayProductions.GetAll().Where(d => d.Date.Equals(productProdactionViewModel.Date)).FirstOrDefault();
            if (dayProduction == null)
            {
                unit.DayProductions.Create(new DayProduction { Date = productProdactionViewModel.Date });
                dayProduction = unit.DayProductions.GetAll().Where(d => d.Date.Equals(productProdactionViewModel.Date)).FirstOrDefault();
            }
            ProductsProduction productsProduction = productProdactionViewModel.ProductsProduction;
            productsProduction.DayProductionId = dayProduction.DayProductionId;
            try
            {
                unit.ProductsProductions.Update(productsProduction);
            }
            catch (DbUpdateConcurrencyException)
            {

            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {

            var productsProduction = unit.ProductsProductions.Get(id);
            if (productsProduction == null)
            {
                return NotFound();
            }

            return View(productsProduction);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var productsProduction = unit.ProductsProductions.Get(id);
            unit.ProductsProductions.Delete(productsProduction);
            return RedirectToAction(nameof(Index));
        }

        private IEnumerable<ProductsProduction> Sort(IEnumerable<ProductsProduction> productsProductions, SortState sortState)
        {
            switch (sortState)
            {
                case SortState.ProductProductionDateAcs:
                    productsProductions = productsProductions.OrderBy(d => d.DayProduction.Date);
                    break;
                case SortState.ProductProductionDateDecs:
                    productsProductions = productsProductions.OrderByDescending(d => d.DayProduction.Date);
                    break;
                case SortState.ProductProductionNameAcs:
                    productsProductions = productsProductions.OrderBy(p => p.Product.ProductName);
                    break;
                case SortState.ProductProductionNameDecs:
                    productsProductions = productsProductions.OrderByDescending(p => p.Product.ProductName);
                    break;
                case SortState.ProductProductionCountAcs:
                    productsProductions = productsProductions.OrderBy(c => c.Count);
                    break;
                case SortState.ProductProductionCountDecs:
                    productsProductions = productsProductions.OrderByDescending(c => c.Count);
                    break;
            }
            return productsProductions;
        }
        public IActionResult ClearFilter()
        {
            _memoryCache.Remove("productProductionDate");
            _memoryCache.Remove("productProductionName");
            _memoryCache.Remove("countProductProduction");
            _memoryCache.Remove("productProductionSort");
            return RedirectToAction("Index");
        }
    }
}
