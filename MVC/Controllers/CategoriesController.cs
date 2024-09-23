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
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: Categories
        [AllowAnonymous]
        public IActionResult Index()
        {
           List<CategoryModel> categories = _categoryService.Query().ToList();
            return View(categories);
        }

        // GET: Categories/Details/5
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            CategoryModel category = _categoryService.Query().SingleOrDefault(c => c.Id == id);
            if (id == null)
            {
                return View("_Error", "Category not found!");
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                Result result = _categoryService.Add(category);
                if (result.IsSuccessful)//işlem başarılıysa
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public IActionResult Edit(int id)
        {
            CategoryModel category = _categoryService.Query().SingleOrDefault(c => c.Id == id);
            if (category == null)
            {
                return View("_Error", "Category not found!");
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CategoryModel category)
        {
            if (ModelState.IsValid)
            {
                var result = _categoryService.Update(category);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", result.Message);
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public IActionResult Delete(int id) //Alertify.js uygularken delete içine deleteconfirmed te yazdığımız yönlendirme kodunu yazıyoruz
        {
            Result result = _categoryService.Delete(id);

            TempData["Message"] = result.Message;

            return RedirectToAction(nameof(Index));
        }
        //public IActionResult Delete(int id)
        //{
        //    CategoryModel category = _categoryService.Query().SingleOrDefault(p => p.Id == id);

        //    if (category == null)
        //    {
        //        return View("_Error", "Category not found!");
        //    }

        //    return View(category);
        //}

        //// POST: Categories/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeleteConfirmed(int id)
        //{
        //    Result result = _categoryService.Delete(id);

        //    TempData["Message"] = result.Message;

        //    return RedirectToAction(nameof(Index));
        //}

    }
}
