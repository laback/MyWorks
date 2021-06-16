using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TaxOfficeActivityWebApp;
using TaxOfficeActivityWebApp.UnitsOfWork;
using TaxOfficeActivityWebApp.ViewModels;

namespace TaxOfficeActivityWebApp.Controllers
{
    [Authorize(Roles = "Директор, Главный бухгалтер")]
    public class EntrepreneursController : Controller
    {
        private readonly UnitOfWork _unit;
        private IMemoryCache _cache;

        public EntrepreneursController(UnitOfWork unit, IMemoryCache cache)
        {
            _unit = unit;
            _cache = cache;
        }

        public IActionResult Index(string taxOfficeName, string fullName, int page = 1)
        {
            if (taxOfficeName == null)
                _cache.TryGetValue("entrepreneurTaxOfficeName", out taxOfficeName);
            else
                _cache.Set("entrepreneurTaxOfficeName", taxOfficeName);

            if (fullName == null)
                _cache.TryGetValue("fullName", out fullName);
            else
                _cache.Set("fullName", fullName);

            IEnumerable<Entrepreneur> entrepreneurs = _unit.Entrepreneurs.GetAll().OrderBy(x => x.FullName);
            int pageSize = 15;

            if (taxOfficeName != null)
                entrepreneurs = entrepreneurs.Where(x => x.TaxOffice.TaxOfficeName.Contains(taxOfficeName));

            if (fullName != null)
                entrepreneurs = entrepreneurs.Where(x => x.FullName.Contains(fullName));

            int count = entrepreneurs.Count();
            entrepreneurs = entrepreneurs.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            EntrepreneurViewModel viewModel = new EntrepreneurViewModel
            {
                Entrepreneurs = entrepreneurs,
                PageViewModel = new PageViewModel(count, page, pageSize),
                TaxOfficeName = taxOfficeName,
                FullName = fullName
            };
            return View(viewModel);
        }
        [Authorize(Roles = "Главный бухгалтер")]
        public IActionResult Create()
        {
            ViewData["TaxOfficeId"] = new SelectList(_unit.TaxOffices.GetAll(), "TaxOfficeId", "TaxOfficeName");
            return View();
        }
        [Authorize(Roles = "Главный бухгалтер")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("EntrepreneurId,TaxOfficeId,FullName")] Entrepreneur entrepreneur)
        {
            if (ModelState.IsValid)
            {
                _unit.Entrepreneurs.Create(entrepreneur);
                return RedirectToAction(nameof(Index));
            }
            ViewData["TaxOfficeId"] = new SelectList(_unit.TaxOffices.GetAll(), "TaxOfficeId", "TaxOfficeName");
            return View(entrepreneur);
        }
        [Authorize(Roles = "Главный бухгалтер")]
        public IActionResult Edit(int id)
        {

            var entrepreneur = _unit.Entrepreneurs.Get(id);
            if (entrepreneur == null)
            {
                return NotFound();
            }
            ViewData["TaxOfficeId"] = new SelectList(_unit.TaxOffices.GetAll(), "TaxOfficeId", "TaxOfficeName");
            return View(entrepreneur);
        }
        [Authorize(Roles = "Главный бухгалтер")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("EntrepreneurId,TaxOfficeId,FullName")] Entrepreneur entrepreneur)
        {
            if (id != entrepreneur.EntrepreneurId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unit.Entrepreneurs.Update(entrepreneur);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntrepreneurExists(entrepreneur.EntrepreneurId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TaxOfficeId"] = new SelectList(_unit.TaxOffices.GetAll(), "TaxOfficeId", "TaxOfficeName");
            return View(entrepreneur);
        }
        [Authorize(Roles = "Главный бухгалтер")]
        public IActionResult Delete(int id)
        {
            var entrepreneur = _unit.Entrepreneurs.Get(id);
            if (entrepreneur == null)
            {
                return NotFound();
            }

            return View(entrepreneur);
        }
        [Authorize(Roles = "Главный бухгалтер")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var entrepreneur = _unit.Entrepreneurs.Get(id);
            _unit.Entrepreneurs.Delete(entrepreneur);
            return RedirectToAction(nameof(Index));
        }

        private bool EntrepreneurExists(int id)
        {
            return _unit.Entrepreneurs.GetAll().Any(e => e.EntrepreneurId == id);
        }

        public IActionResult ClearCache()
        {
            _cache.Remove("entrepreneurTaxOfficeName");
            _cache.Remove("fullName");
            return RedirectToAction("Index");
        }
    }
}
