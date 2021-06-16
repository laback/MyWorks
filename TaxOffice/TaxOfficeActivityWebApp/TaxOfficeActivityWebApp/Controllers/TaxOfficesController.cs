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
    [Authorize(Roles = "Директор")]
    public class TaxOfficesController : Controller
    {
        private readonly UnitOfWork _unit;
        private IMemoryCache _cache;

        public TaxOfficesController(UnitOfWork unit, IMemoryCache cache)
        {
            _unit = unit;
            _cache = cache;
        }

        public IActionResult Index(string districtName, string taxOfficeName, int page = 1)
        {
            if (districtName == null)
                _cache.TryGetValue("taxDistrictName", out districtName);
            else
                _cache.Set("taxDistrictName", districtName);

            if (taxOfficeName == null)
                _cache.TryGetValue("taxOfficeName", out taxOfficeName);
            else
                _cache.Set("taxOfficeName", taxOfficeName);

            IEnumerable<TaxOffice> taxOffices = _unit.TaxOffices.GetAll().OrderBy(x => x.TaxOfficeName);
            int pageSize = 15;

            if (districtName != null)
                taxOffices = taxOffices.Where(x => x.District.DistrictName.Contains(districtName));

            if (taxOfficeName != null)
                taxOffices = taxOffices.Where(x => x.TaxOfficeName.Contains(taxOfficeName));

            int count = taxOffices.Count();
            taxOffices = taxOffices.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            TaxOfficeViewModel viewModel = new TaxOfficeViewModel
            {
                TaxOffices = taxOffices,
                PageViewModel = new PageViewModel(count, page, pageSize),
                DistrictName = districtName,
                TaxOfficeName = taxOfficeName
            };
            return View(viewModel);
        }

        public IActionResult Create()
        {
            ViewData["DistrictId"] = new SelectList(_unit.Districts.GetAll(), "DistrictId", "DistrictName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("TaxOfficeId,DistrictId,TaxOfficeName")] TaxOffice taxOffice)
        {
            if (ModelState.IsValid)
            {
                _unit.TaxOffices.Create(taxOffice);
                return RedirectToAction(nameof(Index));
            }
            ViewData["DistrictId"] = new SelectList(_unit.Districts.GetAll(), "DistrictId", "DistrictName");
            return View(taxOffice);
        }

        public IActionResult Edit(int id)
        {
            var taxOffice = _unit.TaxOffices.Get(id);
            if (taxOffice == null)
            {
                return NotFound();
            }
            ViewData["DistrictId"] = new SelectList(_unit.Districts.GetAll(), "DistrictId", "DistrictName");
            return View(taxOffice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("TaxOfficeId,DistrictId,TaxOfficeName")] TaxOffice taxOffice)
        {
            if (id != taxOffice.TaxOfficeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unit.TaxOffices.Update(taxOffice);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaxOfficeExists(taxOffice.TaxOfficeId))
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
            ViewData["DistrictId"] = new SelectList(_unit.Districts.GetAll(), "DistrictId", "DistrictName");
            return View(taxOffice);
        }

        public IActionResult Delete(int id)
        {

            var taxOffice = _unit.TaxOffices.Get(id);
            if (taxOffice == null)
            {
                return NotFound();
            }

            return View(taxOffice);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var taxOffice = _unit.TaxOffices.Get(id);
            _unit.TaxOffices.Delete(taxOffice);
            return RedirectToAction(nameof(Index));
        }

        private bool TaxOfficeExists(int id)
        {
            return _unit.TaxOffices.GetAll().Any(e => e.TaxOfficeId == id);
        }

        public IActionResult ClearCache()
        {
            _cache.Remove("taxDistrictName");
            _cache.Remove("taxOfficeName");
            return RedirectToAction("Index");
        }
    }
}
