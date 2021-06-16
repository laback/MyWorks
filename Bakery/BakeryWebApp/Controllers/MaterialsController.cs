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
    public class MaterialsController : Controller
    {
        private readonly UnitOfWork unit;

        public MaterialsController(UnitOfWork unit)
        {
            this.unit = unit;
        }
        [Authorize(Roles = "admin, manager, user")]
        public IActionResult Index(string materialName, int page = 1)
        {
            if (materialName == null)
                HttpContext.Request.Cookies.TryGetValue("materialName", out materialName);
            else
                HttpContext.Response.Cookies.Append("materialName", materialName);
            IEnumerable<Material> materials = unit.Materials.GetAll();
            int pageSize = 15;
            if (materialName != null)
                materials = materials.Where(n => n.MaterialName.Contains(materialName));
            int count = materials.Count();
            materials = materials.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            MaterialViewModel viewModel = new MaterialViewModel
            {
                Materials = materials,
                PageViewModel = new PageViewModel(count, page, pageSize),
                MaterialName = materialName

            };
            return View(viewModel);
        }
        [Authorize(Roles = "admin, manager")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, manager")]
        public IActionResult Create([Bind("MaterialId,MaterialName")] Material material)
        {
            if (ModelState.IsValid)
            {
                unit.Materials.Create(material);
                return RedirectToAction(nameof(Index));
            }
            return View(material);
        }
        [Authorize(Roles = "admin, manager")]
        public IActionResult Edit(int id)
        {

            var material = unit.Materials.Get(id);
            if (material == null)
            {
                return NotFound();
            }
            return View(material);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, manager")]
        public IActionResult Edit(int id, [Bind("MaterialId,MaterialName")] Material material)
        {
            if (id != material.MaterialId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    unit.Materials.Update(material);
                }
                catch (DbUpdateConcurrencyException)
                {

                }
                return RedirectToAction(nameof(Index));
            }
            return View(material);
        }
        [Authorize(Roles = "admin, manager")]
        public IActionResult Delete(int id)
        {

            var material = unit.Materials.Get(id);
            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, manager")]
        public IActionResult DeleteConfirmed(int id)
        {
            var material = unit.Materials.Get(id);
            unit.Materials.Delete(material);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ClearCache()
        {
            HttpContext.Response.Cookies.Delete("materialName");
            return RedirectToAction("Index");
        }
    }
}
