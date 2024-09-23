using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Areas.Report.Models;

namespace MVC.Areas.Report.Controllers
{
    [Area("Report")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly IReportService _reportService;
        private readonly ICategoryService _categoryService;
        private readonly ISupplierService _supplierService;

        public HomeController(IReportService reportService, ICategoryService categoryService, ISupplierService supplierService)
        {
            _reportService = reportService;
            _categoryService = categoryService;
            _supplierService = supplierService;
        }

        // GET: HomeController
        public ActionResult Index()
        {
            var model = _reportService.GetList(false);
            var viewModel = new HomeIndexViewModel()
            {
                Report = model,
                Categories = new SelectList(_categoryService.Query().ToList(), "Id", "Name"),
                Suppliers = new MultiSelectList(_supplierService.Query().ToList(), "Id","Name"),
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Index(FilterModel filter)
        {
            var model = _reportService.GetList(false, filter);
            //var viewModel = new HomeIndexViewModel()
            //{
            //    Report = model,
            //    Filter = filter
            //};

            return PartialView("_Report", model);
        }
    }
}
