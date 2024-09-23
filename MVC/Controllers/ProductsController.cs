#nullable disable

using Business.Models;
using Business.Services;
using Core.Results;
using Core.Results.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Settings;

namespace MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    //entityden değil model kullanıyoruz
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ISupplierService _supplierService;
        public ProductsController(IProductService productService, ICategoryService categoryService, ISupplierService supplierService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _supplierService = supplierService;
        }

        // GET: Products
        [AllowAnonymous]
        public IActionResult Index()
        {
            List<ProductModel> products = _productService.Query().ToList();
            return View(products);
        }

        //get: products/details/5
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            ProductModel product = _productService.Query().SingleOrDefault(p => p.Id == id);
            if (product == null)
            {
                return View("_Error", "Product Not Found!");
            }
            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_categoryService.Query().ToList(), "Id", "Name");
            ViewData["SupplierId"] = new MultiSelectList(_supplierService.Query().ToList(), "Id", "Name");

            return View();
        }

        //POST: Products/Create
        //To protect from overposting attacks, enable the specific properties you want to bind to.
        //For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //Bu aksiyon üzerinden sunucu tarafından validasyon(server side validation) yapılmaktadır.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductModel product, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                Result result;

                //image yükleme
                result = UpdateImage(product, image);
                if (result.IsSuccessful)
                {
                    result = _productService.Add(product);
                    if (result.IsSuccessful)//işlem başarılıysa
                    {
                        TempData["Message"] = result.Message;
                        return RedirectToAction(nameof(Index));
                    }
                }
                //Aksiyondan view a validasyon hatası mesajı taşıma 1.yntm:
                //ViewBag.CreateMessage = result.Message;//işlem başarısızsa

                //Aksiyondan view a validasyon hatası mesajı taşıma 2.yntm:
                ModelState.AddModelError("", result.Message);
            }
            ViewData["CategoryId"] = new SelectList(_categoryService.Query().ToList(), "Id", "Name");
            ViewData["SupplierId"] = new MultiSelectList(_supplierService.Query().ToList(), "Id", "Name");
            return View(product);
        }

        private Result UpdateImage(ProductModel product, IFormFile image)
        {
            if (image is not null && image.Length > 0)
            {
                #region Dosya uzantı ve boyut validasyonları
                string fileName = image.FileName;//asusrog.jpg
                string extansion = Path.GetExtension(fileName); // .jpg

                if (!AppSettings.AcceptedImageExtensions.Split(',').Any(i => i.ToLower().Trim() == extansion.ToLower()))
                {
                    return new ErrorResult("Image can't be uploaded because image extansion is not in \" "+AppSettings.AcceptedImageExtensions +" \"!");
                }

                // 1 byte = 8 bits
                // 1 kb = 1024 bytes
                // 1 Mb = 1024 Kb = 1024*1024 bytes = 1.048.576 bytes

                double acceptedFileLength = AppSettings.AcceptedImageLength; // Mb
                double acceptedFileLengthInBytes = acceptedFileLength * Math.Pow(1024, 2);

                if (image.Length > acceptedFileLengthInBytes)
                {
                    return new ErrorResult("Image can't be uploaded because image file length is greater than " + acceptedFileLength.ToString("N1") + "mega bytes!");
                }

                #endregion

                #region Model içerisindeki Image ve ImageExtansion özellikeri güncellemesi
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    image.CopyTo(memoryStream);
                    product.Image = memoryStream.ToArray();
                    product.ImageExtension = extansion;
                }
                #endregion
            }
            return new SuccessResult();
        }

        // GET: Products/Edit/5
        public IActionResult Edit(int id)
        {
            ProductModel product = _productService.Query().SingleOrDefault(p => p.Id == id);
            if (product == null)
            {
                return View("_Error", "Product not found!");
            }


            ViewData["CategoryId"] = new SelectList(_categoryService.Query().ToList(), "Id", "Name");
            ViewData["SupplierId"] = new MultiSelectList(_supplierService.Query().ToList(), "Id", "Name");
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //Bu aksiyon üzerinden sunucu tarafından validasyon(server side validation) yapılmaktadır.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductModel product, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                Result result = UpdateImage(product, image);

                if (result.IsSuccessful)
                {

                    result = _productService.Update(product);

                    if (result.IsSuccessful)
                    {
                        TempData["Message"] = result.Message;
                        return RedirectToAction(nameof(Index));
                    }
                }
                ModelState.AddModelError("", result.Message);
            }
            ViewData["CategoryId"] = new SelectList(_categoryService.Query().ToList(), "Id", "Name", product.CategoryId);
            ViewData["SupplierId"] = new SelectList(_supplierService.Query().ToList(), "Id", "Name", product.SupplierId);
            return View(product);
        }

        // GET: Products/Delete/5
        public IActionResult Delete(int id) //Alertify.js uygularken delete içine deleteconfirmed te yazdığımız yönlendirme kodunu yazıyoruz
        {
            Result result = _productService.Delete(id);

            TempData["Message"] = result.Message;

            return RedirectToAction(nameof(Index));
        }

        //Silme işlemini gerçekleştirirken bu sekilde uygulama yaparız ama alertify.js uyg. yukardaki şekilde işlem yapmamız gerekiyor
        //// GET: Products/Delete/5
        //public IActionResult Delete(int id)
        //{
        //    OrderModel order = _productService.Query().SingleOrDefault(o => o.Id == id);

        //    if (order == null)
        //    {
        //        return View("_Error", "Product not found!s");
        //    }
        //    return View(product);
        //}

        //// POST: Products/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeleteConfirmed(int id)
        //{
        //    Result result = _productService.Delete(id);

        //    TempData["Message"] = result.Message;
        //    return RedirectToAction(nameof(Index));
        //}

    }

}
