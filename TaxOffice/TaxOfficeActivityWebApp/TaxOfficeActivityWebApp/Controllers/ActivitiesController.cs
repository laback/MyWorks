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
    public class ActivitiesController : Controller
    {
        private readonly UnitOfWork _unit;
        private IMemoryCache _cache;

        public ActivitiesController(UnitOfWork unit, IMemoryCache cache)
        {
            _unit = unit;
            _cache = cache;
        }

        public IActionResult Index(string activityName, int page = 1)
        {
            if (activityName == null)
                _cache.TryGetValue("activityName", out activityName);
            else
                _cache.Set("activityName", activityName);
            IEnumerable<Activity> activities = _unit.Activities.GetAll().OrderBy(x => x.ActivityName);
            int pageSize = 15;
            if (activityName != null)
                activities = activities.Where(x => x.ActivityName.Contains(activityName));
            int count = activities.Count();
            activities = activities.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ActivityViewModel viewModel = new ActivityViewModel
            {
                Activities = activities,
                PageViewModel = new PageViewModel(count, page, pageSize),
                ActivityName = activityName
            };
            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ActivityId,ActivityName,Tax")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                _unit.Activities.Create(activity);
                return RedirectToAction(nameof(Index));
            }
            return View(activity);
        }

        public IActionResult Edit(int id)
        {
            var activity = _unit.Activities.Get(id);
            if (activity == null)
            {
                return NotFound();
            }
            return View(activity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ActivityId,ActivityName,Tax")] Activity activity)
        {
            if (id != activity.ActivityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unit.Activities.Update(activity);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(activity.ActivityId))
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
            return View(activity);
        }

        public IActionResult Delete(int id)
        {
            var activity = _unit.Activities.Get(id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var activity = _unit.Activities.Get(id);
            _unit.Activities.Delete(activity);
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityExists(int id)
        {
            return _unit.Activities.GetAll().Any(e => e.ActivityId == id);
        }
        public IActionResult ClearCache()
        {
            _cache.Remove("activityName");
            return RedirectToAction("Index");
        }
    }
}
