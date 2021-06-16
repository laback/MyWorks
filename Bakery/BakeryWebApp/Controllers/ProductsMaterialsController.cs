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
    public class ProductsMaterialsController : Controller
    {
        private readonly UnitOfWork unit;
        private IMemoryCache _memoryCache;

        public ProductsMaterialsController(UnitOfWork unit, IMemoryCache cache)
        {
            this.unit = unit;
            _memoryCache = cache;
        }
        [Authorize(Roles = "admin, manager, user")]
        public IActionResult Index(string materialName, string productName, double quantity, SortState sortState, int page = 1)
        {
            IEnumerable<ProductsMaterial> productsMaterials = unit.ProductMaterials.GetAll();
            if (materialName == null)
                _memoryCache.TryGetValue("materialNameProductMaterial", out materialName);
            else
            {
                _memoryCache.Set("materialNameProductMaterial", materialName);
                productsMaterials = productsMaterials.Where(n => n.Material.MaterialName.Contains(materialName));
            }
            if (productName == null)
                _memoryCache.TryGetValue("productNameProductMaterial", out productName);
            else
            {
                _memoryCache.Set("productNameProductMaterial", productName);
                productsMaterials = productsMaterials.Where(n => n.Product.ProductName.Contains(productName));
            }
            if (quantity == 0)
                _memoryCache.TryGetValue("quanityProductMaterial", out quantity);
            else
            {
                _memoryCache.Set("quantityProductMaterial", quantity);
                productsMaterials = productsMaterials.Where(n => n.Quantity == quantity);
            }
            if (sortState == SortState.No)
                _memoryCache.TryGetValue("productMaterialSort", out sortState);
            else
            {
                _memoryCache.Set("productMaterialSort", sortState);
                productsMaterials = Sort(productsMaterials, sortState);
            }
            int count = productsMaterials.Count();
            int pageSize = 15;
            productsMaterials = productsMaterials.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ProductMaterialViewModel viewModel = new ProductMaterialViewModel
            {
                ProductsMaterials = productsMaterials,
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortState),
                MaterialName = materialName,
                ProductionName = productName,
                Quantity = quantity

            };
            return View(viewModel);
        }
        [Authorize(Roles = "admin, manager")]
        public IActionResult Create()
        {
            ViewData["MaterialId"] = new SelectList(unit.Materials.GetAll(), "MaterialId", "MaterialName");
            ViewData["ProductId"] = new SelectList(unit.Products.GetAll(), "ProductId", "ProductName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, manager")]
        public IActionResult Create([Bind("ProductMaterial,MaterialId,ProductId,Quantity")] ProductsMaterial productsMaterial)
        {
            if (ModelState.IsValid)
            {
                unit.ProductMaterials.Create(productsMaterial);
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaterialId"] = new SelectList(unit.Materials.GetAll(), "MaterialId", "MaterialName");
            ViewData["ProductId"] = new SelectList(unit.Products.GetAll(), "ProductId", "ProductName");
            return View(productsMaterial);
        }
        [Authorize(Roles = "admin, manager")]
        public IActionResult Edit(int id)
        {

            var productsMaterial = unit.ProductMaterials.Get(id);
            if (productsMaterial == null)
            {
                return NotFound();
            }
            ViewData["MaterialId"] = new SelectList(unit.Materials.GetAll(), "MaterialId", "MaterialName");
            ViewData["ProductId"] = new SelectList(unit.Products.GetAll(), "ProductId", "ProductName");
            return View(productsMaterial);
        }
        [Authorize(Roles = "admin, manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProductMaterial,MaterialId,ProductId,Quantity")] ProductsMaterial productsMaterial)
        {
            if (id != productsMaterial.ProductMaterial)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    unit.ProductMaterials.Update(productsMaterial);
                }
                catch (DbUpdateConcurrencyException)
                {

                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaterialId"] = new SelectList(unit.Materials.GetAll(), "MaterialId", "MaterialName");
            ViewData["ProductId"] = new SelectList(unit.Products.GetAll(), "ProductId", "ProductName");
            return View(productsMaterial);
        }
        [Authorize(Roles = "admin, manager")]
        public IActionResult Delete(int id)
        {

            var productsMaterial = unit.ProductMaterials.Get(id);
            if (productsMaterial == null)
            {
                return NotFound();
            }

            return View(productsMaterial);
        }
        [Authorize(Roles = "admin, manager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var productsMaterial = unit.ProductMaterials.Get(id);
            unit.ProductMaterials.Delete(productsMaterial);
            return RedirectToAction(nameof(Index));
        }
        private IEnumerable<ProductsMaterial> Sort(IEnumerable<ProductsMaterial> productsMaterials, SortState sortState)
        {
            switch (sortState)
            {
                case SortState.ProductMaterialMaterialAcs:
                    productsMaterials = productsMaterials.OrderBy(m => m.Material.MaterialName);
                    break;
                case SortState.ProductMaterialMaterialDecs:
                    productsMaterials = productsMaterials.OrderByDescending(m => m.Material.MaterialName);
                    break;
                case SortState.ProductMaterialProductAcs:
                    productsMaterials = productsMaterials.OrderBy(p => p.Product.ProductName);
                    break;
                case SortState.ProductMaterialProductDecs:
                    productsMaterials = productsMaterials.OrderBy(p => p.Product.ProductName);
                    break; ;
                case SortState.ProductMaterialQuantityAcs:
                    productsMaterials = productsMaterials.OrderBy(q => q.Quantity);
                    break;
                case SortState.ProductMaterialQuantityDecs:
                    productsMaterials = productsMaterials.OrderBy(q => q.Quantity);
                    break;
            }
            return productsMaterials;
        }

        public IActionResult ClearFilter()
        {
            _memoryCache.Remove("materialNameProductMaterial");
            _memoryCache.Remove("productNameProductMaterial");
            _memoryCache.Remove("quanityProductMaterial");
            _memoryCache.Remove("productMaterialSort");
            return RedirectToAction("Index");
        }
    }
}
