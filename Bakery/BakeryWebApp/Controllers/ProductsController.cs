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
    public class ProductsController : Controller
    {
        private readonly UnitOfWork unit;

        public ProductsController(UnitOfWork unit)
        {
            this.unit = unit;
        }

        public IActionResult Index(string productName, int page = 1)
        {
            if (productName == null)
                HttpContext.Request.Cookies.TryGetValue("productName", out productName);
            else
                HttpContext.Response.Cookies.Append("productName", productName);
            IEnumerable<Product> products = unit.Products.GetAll();
            int pageSize = 15;
            if (productName != null)
                products = products.Where(n => n.ProductName.Contains(productName));
            int count = products.Count();
            products = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ProductViewModel viewModel = new ProductViewModel
            {
                Products = products,
                PageViewModel = new PageViewModel(count, page, pageSize),
                ProductName = productName
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
        public IActionResult Create([Bind("ProductId,ProductName")] Product product)
        {
            if (ModelState.IsValid)
            {
                unit.Products.Create(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        [Authorize(Roles = "admin, manager")]
        public IActionResult Edit(int id)
        {

            var product = unit.Products.Get(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, manager")]
        public IActionResult Edit(int id, [Bind("ProductId,ProductName")] Product product)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    unit.Products.Update(product);
                }
                catch (DbUpdateConcurrencyException)
                {

                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        [Authorize(Roles = "admin, manager")]
        public IActionResult Delete(int id)
        {

            var product = unit.Products.Get(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, manager")]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = unit.Products.Get(id);
            unit.Products.Delete(product);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ClearCache()
        {
            HttpContext.Response.Cookies.Delete("productName");
            return RedirectToAction("Index");
        }
    }
}
