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
    public class NormsController : Controller
    {
        private readonly UnitOfWork unit;
        private IMemoryCache _memoryCache;

        public NormsController(UnitOfWork unit, IMemoryCache cache)
        {
            this.unit = unit;
            _memoryCache = cache;
        }
        [Authorize(Roles = "admin, manager, user")]
        public IActionResult Index(string rawName, string productName, double quantity, SortState sortState, int page = 1)
        {
            IEnumerable<Norm> norms = unit.Norms.GetAll();
            if (rawName == null)
                _memoryCache.TryGetValue("rawNameNorm", out rawName);
            else
            {
                _memoryCache.Set("rawNameNorm", rawName);
                norms = norms.Where(n => n.Raw.RawName.Contains(rawName));
            }
            if (productName == null)
                _memoryCache.TryGetValue("productNameNorm", out productName);
            else
            {
                _memoryCache.Set("productNameNorm", productName);
                norms = norms.Where(n => n.Product.ProductName.Contains(productName));
            }
            if (quantity == 0)
                _memoryCache.TryGetValue("quanityNorm", out quantity);
            else
            {
                _memoryCache.Set("quantityNorm", quantity);
                norms = norms.Where(n => n.Quantity == quantity);
            }
            if (sortState == SortState.No)
                _memoryCache.TryGetValue("normSort", out sortState);
            else
            {
                _memoryCache.Set("normSort", sortState);
                norms = Sort(norms, sortState);
            }
            int count = norms.Count();
            int pageSize = 15;
            norms = norms.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            NormViewModel viewModel = new NormViewModel
            {
                Norms = norms,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortState),
                RawName = rawName,
                ProductionName = productName,
                Quantity = quantity

            };
            return View(viewModel);
        }
        [Authorize(Roles = "admin, manager")]
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(unit.Products.GetAll(), "ProductId", "ProductName");
            ViewData["RowId"] = new SelectList(unit.Raws.GetAll(), "RawId", "RawName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, manager")]
        public IActionResult Create([Bind("NormId,RawId,ProductId,Quantity")] Norm norm)
        {

            if (norm.Quantity > 0)
            {
                unit.Norms.Create(norm);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(unit.Products.GetAll(), "ProductId", "ProductName");
            ViewData["RowId"] = new SelectList(unit.Raws.GetAll(), "RawId", "RawName");
            return View(norm);
        }

        [Authorize(Roles = "admin, manager")]
        public IActionResult Edit(int id)
        {

            var norm = unit.Norms.Get(id);
            if (norm == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(unit.Products.GetAll(), "ProductId", "ProductName");
            ViewData["RowId"] = new SelectList(unit.Raws.GetAll(), "RawId", "RawName");
            return View(norm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, manager")]
        public ActionResult Edit(int id, [Bind("NormId,RawId,ProductId,Quantity")] Norm norm)
        {
            if (id != norm.NormId)
            {
                return NotFound();
            }

            if (norm.Quantity > 0)
            {
                try
                {
                    unit.Norms.Update(norm);
                }
                catch (DbUpdateConcurrencyException)
                {
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(unit.Products.GetAll(), "ProductId", "ProductName");
            ViewData["RowId"] = new SelectList(unit.Raws.GetAll(), "RawId", "RawName");
            return View(norm);
        }
        [Authorize(Roles = "admin, manager")]
        public IActionResult Delete(int id)
        {

            var norm = unit.Norms.Get(id);
            if (norm == null)
            {
                return NotFound();
            }

            return View(norm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, manager")]
        public IActionResult DeleteConfirmed(int id)
        {
            var norm = unit.Norms.Get(id);
            unit.Norms.Delete(norm);
            return RedirectToAction(nameof(Index));
        }

        private IEnumerable<Norm> Sort(IEnumerable<Norm> norms, SortState sortState)
        {
            switch (sortState)
            {
                case SortState.NormRawNameAcs:
                    norms = norms.OrderBy(r => r.Raw.RawName);
                    break;
                case SortState.NormRawNameDecs:
                    norms = norms.OrderByDescending(r => r.Raw.RawName);
                    break;
                case SortState.NormProductNameAcs:
                    norms = norms.OrderBy(p => p.Product.ProductName);
                    break;
                case SortState.NormProductNameDecs:
                    norms = norms.OrderByDescending(p => p.Product.ProductName);
                    break;
                case SortState.NormQuantityAcs:
                    norms = norms.OrderBy(q => q.Quantity);
                    break;
                case SortState.NormQuantityDecs:
                    norms = norms.OrderByDescending(q => q.Quantity);
                    break;
            }
            return norms;
        }

        public IActionResult ClearFilter()
        {
            _memoryCache.Remove("rawNameNorm");
            _memoryCache.Remove("productNameNorm");
            _memoryCache.Remove("quanityNorm");
            _memoryCache.Remove("normSort");
            return RedirectToAction("Index");
        }
    }
}
