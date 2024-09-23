#nullable disable

using Business.Models;
using Business.Services;
using Core.Results.Bases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly IProductService _productService;

        public OrdersController(IOrderService orderService, IUserService userService = null, IProductService productService = null)
        {
            _orderService = orderService;
            _userService = userService;
            _productService = productService;
        }
        // GET: Orders

        //[Authorize] //Identity cookie var mı diye kontrol eder 
        //[Authorize(Roles = "Admin")] //Hem Identity cookie var mı diye hemde Identity cookie Admin rolü var mı diye kontrol eder 
        [AllowAnonymous]
        public IActionResult Index()
        {
            List<OrderModel> orderList = _orderService.Query().ToList(); 
            return View(orderList);
        }

        // GET: Orders/Details/5
        //[Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            OrderModel order = _orderService.Query().SingleOrDefault(o => o.Id == id);
            if (order == null)
            {
                return View("_Error", "Order not found!");
            }
            return View(order);
        }


        // GET: Orders/Create
        //[Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            //var userId = User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid).Value;
            //ViewData["UserId"] = new SelectList(_userService.Query().ToList(), "Id", "Name", userId);
            ViewData["Products"] = new MultiSelectList(_productService.Query().ToList(),"Id","Name");

            var model = new OrderModel()
            {
                UserId = Convert.ToInt32(User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid).Value)
            };
            return View(model);
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]

        public IActionResult Create(OrderModel order)
        {
            if (ModelState.IsValid)
            {
                order.UserId = Convert.ToInt32(User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid).Value);
                Result result = _orderService.Add(order);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", result.Message);
            }
            ViewData["UserId"] = new SelectList(_userService.Query().ToList(), "Id", "Name", order.UserId);
            ViewData["Products"] = new MultiSelectList(_productService.Query().ToList(), "Id", "Name", order.ProductIds);
            return View(order);
        }

        // GET: Orders/Edit/5
        //[Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            OrderModel order = _orderService.Query().SingleOrDefault(o => o.Id == id);
            if (order == null)
            {
                return View("_Error", "Order not found!");
            }


            ViewData["UserId"] = new SelectList(_userService.Query().ToList(), "Id", "Name", order.UserId);
            ViewData["Products"] = new MultiSelectList(_productService.Query().ToList(), "Id", "Name", order.ProductIds);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public IActionResult Edit(OrderModel order)
        {
            if (ModelState.IsValid)
            {
                var result = _orderService.Update(order);

                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", result.Message);
            }

             ViewData["UserId"] = new SelectList(_userService.Query().ToList(), "Id", "Name", order.UserId);
             ViewData["Products"] = new MultiSelectList(_productService.Query().ToList(), "Id", "Name", order.ProductIds);
             return View(order);
        }

        // GET: Orders/Delete/5
        //[Authorize(Roles = "Admin")]
        public IActionResult Delete (int id)
        {
            Result result = _orderService.Delete(id);

            TempData["Message"] = result.Message;

            return RedirectToAction (nameof(Index));
        }

        //public IActionResult Delete(int id)
        //{
        //    OrderModel order = _orderService.Query().SingleOrDefault(o => o.Id == id);
            
        //    if(order == null)
        //    {
        //        return View("_Error", "Order not found!s");
        //    }
        //    return View(order);
        //}

        //// POST: Orders/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        ////[Authorize(Roles = "Admin")]
        //public IActionResult DeleteConfirmed(int id)
        //{
        //    Result result = _orderService.Delete(id);

        //    TempData["Message"] = result.Message;
        //    return RedirectToAction(nameof(Index));
        //}
    }
}
