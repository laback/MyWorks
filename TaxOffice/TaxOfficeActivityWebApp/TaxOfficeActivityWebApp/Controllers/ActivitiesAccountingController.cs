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
    [Authorize()]
    public class ActivitiesAccountingController : Controller
    {
        private readonly UnitOfWork _unit;
        private IMemoryCache _cache;

        public ActivitiesAccountingController(UnitOfWork unit, IMemoryCache cache)
        {
            _unit = unit;
            _cache = cache;
        }

        public IActionResult Index(string fullName, string activityName, SortState sortState, int page = 1)
        {
            if (fullName == null)
                _cache.TryGetValue("activityFullName", out fullName);
            else
                _cache.Set("activityFullName", fullName);

            if (activityName == null)
                _cache.TryGetValue("activityAccountingName", out activityName);
            else
                _cache.Set("activityAccountingName", activityName);

            IEnumerable<ActivityAccounting> activitiesAccounting = _unit.ActivitiesAccounting.GetAll();
            int pageSize = 15;

            if (fullName != null)
                activitiesAccounting = activitiesAccounting.Where(x => x.Entrepreneur.FullName.Contains(fullName));

            if (activityName != null)
                activitiesAccounting = activitiesAccounting.Where(x => x.Activity.ActivityName.Contains(activityName));

            activitiesAccounting = Sort(activitiesAccounting, sortState);
            int count = activitiesAccounting.Count();
            activitiesAccounting = activitiesAccounting.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ActivityAccountingViewModel viewModel = new ActivityAccountingViewModel
            {
                ActivitiesAccounting = activitiesAccounting,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortState),
                FullName = fullName,
                ActivityName = activityName
            };
            return View(viewModel);
        }
        [Authorize(Roles = "Главный бухгалтер, Бухгалтер")]
        public IActionResult Create()
        {
            ViewData["ActivityId"] = new SelectList(_unit.Activities.GetAll(), "ActivityId", "ActivityName");
            ViewData["EntrepreneurId"] = new SelectList(_unit.Entrepreneurs.GetAll(), "EntrepreneurId", "FullName");
            return View();
        }
        [Authorize(Roles = "Главный бухгалтер, Бухгалтер")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ActivityAccountingId,EntrepreneurId,ActivityId,Year,Quarter,Income,IsTaxPaid")] ActivityAccounting activityAccounting)
        {
            if (ModelState.IsValid)
            {
                _unit.ActivitiesAccounting.Create(activityAccounting);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActivityId"] = new SelectList(_unit.Activities.GetAll(), "ActivityId", "ActivityName");
            ViewData["EntrepreneurId"] = new SelectList(_unit.Entrepreneurs.GetAll(), "EntrepreneurId", "FullName");
            return View(activityAccounting);
        }
        [Authorize(Roles = "Главный бухгалтер, Бухгалтер")]
        public IActionResult Edit(int id)
        {

            var activityAccounting = _unit.ActivitiesAccounting.Get(id);
            if (activityAccounting == null)
            {
                return NotFound();
            }
            ViewData["ActivityId"] = new SelectList(_unit.Activities.GetAll(), "ActivityId", "ActivityName");
            ViewData["EntrepreneurId"] = new SelectList(_unit.Entrepreneurs.GetAll(), "EntrepreneurId", "FullName");
            return View(activityAccounting);
        }
        [Authorize(Roles = "Главный бухгалтер, Бухгалтер")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ActivityAccountingId,EntrepreneurId,ActivityId,Year,Quarter,Income,IsTaxPaid")] ActivityAccounting activityAccounting)
        {
            if (id != activityAccounting.ActivityAccountingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unit.ActivitiesAccounting.Update(activityAccounting);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityAccountingExists(activityAccounting.ActivityAccountingId))
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
            ViewData["ActivityId"] = new SelectList(_unit.Activities.GetAll(), "ActivityId", "ActivityName");
            ViewData["EntrepreneurId"] = new SelectList(_unit.Entrepreneurs.GetAll(), "EntrepreneurId", "FullName");
            return View(activityAccounting);
        }
        [Authorize(Roles = "Главный бухгалтер, Бухгалтер")]
        public IActionResult Delete(int id)
        {
            var activityAccounting = _unit.ActivitiesAccounting.Get(id);
            if (activityAccounting == null)
            {
                return NotFound();
            }

            return View(activityAccounting);
        }
        [Authorize(Roles = "Главный бухгалтер, Бухгалтер")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var activityAccounting = _unit.ActivitiesAccounting.Get(id);
            _unit.ActivitiesAccounting.Delete(activityAccounting);
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityAccountingExists(int id)
        {
            return _unit.ActivitiesAccounting.GetAll().Any(e => e.ActivityAccountingId == id);
        }

        private IEnumerable<ActivityAccounting> Sort(IEnumerable<ActivityAccounting> activitiesAccounting, SortState sortState)
        {
            switch (sortState)
            {
                case SortState.YearAcs:
                    activitiesAccounting = activitiesAccounting.OrderBy(d => d.Year);
                    break;
                case SortState.YearDecs:
                    activitiesAccounting = activitiesAccounting.OrderByDescending(d => d.Year);
                    break;
                case SortState.QuarterAcs:
                    activitiesAccounting = activitiesAccounting.OrderBy(p => p.Quarter);
                    break;
                case SortState.QuarterDecs:
                    activitiesAccounting = activitiesAccounting.OrderByDescending(p => p.Quarter);
                    break;
                case SortState.IncomeAcs:
                    activitiesAccounting = activitiesAccounting.OrderBy(p => p.Income);
                    break;
                case SortState.IncomeDecs:
                    activitiesAccounting = activitiesAccounting.OrderByDescending(p => p.Income);
                    break;
            }
            return activitiesAccounting;
        }

        public IActionResult ClearCache()
        {
            _cache.Remove("activityFullName");
            _cache.Remove("activityAccountingName");
            return RedirectToAction("Index");
        }
    }
}
