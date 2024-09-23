#nullable disable

using Business.Models;
using Business.Services;
using Core.Results.Bases;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    [Authorize(Roles = "Admin")]

    public class SuppliersController : Controller
    {
        private readonly ISupplierService _supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        // GET: Suppliers
        [AllowAnonymous]
        public IActionResult Index()
        {
            List<SupplierModel> suppliers = _supplierService.Query().ToList();
            return View(suppliers);
        }

        // GET: Suppliers/Details/5
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            SupplierModel supplier = _supplierService.Query().SingleOrDefault(s => s.Id == id);
            if (id == null)
            {
                return View("_Error", "Suplier not found!");
            }

            return View(supplier);
        }

        // GET: Suppliers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SupplierModel supplier)
        {
            if(ModelState.IsValid)
            {
                Result result = _supplierService.Add(supplier);
                
                if(result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            return View(supplier);
            
        }

        // GET: Suppliers/Edit/5
        public IActionResult Edit(int id)
        {
            SupplierModel supplier = _supplierService.Query().SingleOrDefault(s => s.Id == id);
            if(supplier == null)
            {
                return View("_Error", "Supplier not found!");
            }
            return View(supplier);
        }

        // POST: Suppliers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SupplierModel supplier)
        {
            if (ModelState.IsValid)
            {
                var result = _supplierService.Update(supplier);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            return View(supplier);
        }

        // GET: Suppliers/Delete/5
        public IActionResult Delete (int id)
        {
            Result result = _supplierService.Delete(id);

            TempData["Message"] = result.Message;

            return RedirectToAction (nameof(Index));
        }

        //public IActionResult Delete(int id)
        //{
        //    SupplierModel supplier = _supplierService.Query().SingleOrDefault(s => s.Id == id);

        //    if(supplier == null)
        //    {
        //        return View("_Error", "Supplier not found!");
        //    }
        //    return View(supplier);
        //}

        //// POST: Suppliers/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeleteConfirmed(int id)
        //{
        //    Result result = _supplierService.Delete(id);

        //    TempData["Message"] = result.Message;

        //    return RedirectToAction(nameof(Index));
        //}
    }
}
