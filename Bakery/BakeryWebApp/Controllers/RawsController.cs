using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BakeryWebApp;
using BakeryWebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using BakeryWebApp.Unit_of_work;

namespace BakeryWebApp.Controllers
{
    public class RawsController : Controller
    {
        private readonly UnitOfWork unit;

        public RawsController(UnitOfWork unit)
        {
            this.unit = unit;
        }

        [Authorize(Roles = "admin, manager, user")]
        public IActionResult Index(string rawName, int page = 1)
        {
            if(rawName == null)
                HttpContext.Request.Cookies.TryGetValue("rawName", out rawName);
            else
                HttpContext.Response.Cookies.Append("rawName", rawName);
            IEnumerable<Raw> raws = unit.Raws.GetAll();
            int pageSize = 15;
            if(rawName != null)
                raws = raws.Where(n => n.RawName.Contains(rawName));
            int count = raws.Count();
            raws = raws.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            RawViewModel viewModel = new RawViewModel
            {
                Raws = raws,
                PageViewModel = new PageViewModel(count, page, pageSize),
                RawName = rawName
            };
            return View(viewModel);
        }
        [Authorize(Roles = "admin, manager")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "admin, manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("RawId,RawName")] Raw raw)
        {
            if (ModelState.IsValid)
            {
                unit.Raws.Create(raw);
                return RedirectToAction(nameof(Index));
            }
            return View(raw);
        }
        [Authorize(Roles = "admin, manager")]
        public IActionResult Edit(int id)
        {

            var raw = unit.Raws.Get(id);
            if (raw == null)
            {
                return NotFound();
            }
            return View(raw);
        }
        [HttpPost]
        [Authorize(Roles = "admin, manager")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("RawId,RawName")] Raw raw)
        {
            if (id != raw.RawId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    unit.Raws.Update(raw);
                }
                catch (DbUpdateConcurrencyException)
                {
                }
                return RedirectToAction(nameof(Index));
            }
            return View(raw);
        }
        [Authorize(Roles = "admin, manager")]
        public IActionResult Delete(int id)
        {

            var raw = unit.Raws.Get(id);
            if (raw == null)
            {
                return NotFound();
            }

            return View(raw);
        }
        [Authorize(Roles = "admin, manager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var raw = unit.Raws.Get(id);
            unit.Raws.Delete(raw);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ClearCache()
        {
            HttpContext.Response.Cookies.Delete("rawName");
            return RedirectToAction("Index");
        }
    }
}
