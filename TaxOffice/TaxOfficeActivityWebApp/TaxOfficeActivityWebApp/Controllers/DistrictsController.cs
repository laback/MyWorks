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
    public class DistrictsController : Controller
    {
        private readonly UnitOfWork _unit;
        private IMemoryCache _cache;
        public DistrictsController(UnitOfWork unit, IMemoryCache cache)
        {
            _unit = unit;
            _cache = cache;
        }

        public IActionResult Index(string districtName, int page = 1)
        {
            if (districtName == null)
                _cache.TryGetValue("districtName", out districtName);
            else
                _cache.Set("districtName", districtName);
            IEnumerable<District> districts = _unit.Districts.GetAll().OrderBy(x => x.DistrictName);
            int pageSize = 15;
            if (districtName != null)
                districts = districts.Where(x => x.DistrictName.Contains(districtName));
            int count = districts.Count();
            districts = districts.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            DistrictViewModel viewModel = new DistrictViewModel
            {
                Districts = districts,
                PageViewModel = new PageViewModel(count, page, pageSize),
                DistrictName = districtName
            };
            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("DistrictId,DistrictName")] District district)
        {
            if (ModelState.IsValid)
            {
                _unit.Districts.Create(district);
                return RedirectToAction(nameof(Index));
            }
            return View(district);
        }

        public IActionResult Edit(int id)
        {
            var district = _unit.Districts.Get(id);
            if (district == null)
            {
                return NotFound();
            }
            return View(district);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("DistrictId,DistrictName")] District district)
        {
            if (id != district.DistrictId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unit.Districts.Update(district);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DistrictExists(district.DistrictId))
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
            return View(district);
        }

        public IActionResult Delete(int id)
        {
            var district = _unit.Districts.Get(id);
            if (district == null)
            {
                return NotFound();
            }

            return View(district);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var district = _unit.Districts.Get(id);
            _unit.Districts.Delete(district);
            return RedirectToAction(nameof(Index));
        }

        private bool DistrictExists(int id)
        {
            return _unit.Districts.GetAll().Any(e => e.DistrictId == id);
        }

        public IActionResult ClearCache()
        {
            _cache.Remove("districtName");
            return RedirectToAction("Index");
        }
    }
}
