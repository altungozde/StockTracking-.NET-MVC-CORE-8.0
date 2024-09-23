#nullable disable

using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using Newtonsoft.Json;
using System.Security.Claims;

namespace MVC.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        const string SESSIONKEY = "favorites";

        int _userId;

        private readonly ISupplierService _supplierService;

        public FavoritesController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        public IActionResult GetFavorites()
        {
            _userId = Convert.ToInt32(User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid).Value);

            var favoritesList = GetSession(_userId);

            return View("Favorites", favoritesList);
        }

        public IActionResult AddToFavorites(int supplierId)
        {
            _userId = Convert.ToInt32(User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid).Value);

            var favoritesList = GetSession(_userId);

            var supplier = _supplierService.Query().SingleOrDefault(s => s.Id == supplierId);

            if (favoritesList.Any(f => f.SupplierId == supplierId && f.UserId == _userId))
            {
                TempData["Message"] = $"\"{supplier.Name}\" already added to favorites!";
            }
            else
            {
                var favoritesItem = new FavoritesModel(supplierId, _userId, supplier.Name);

                favoritesList.Add(favoritesItem);

                SetSession(favoritesList);

                TempData["Message"] = $"\"{supplier.Name}\" added to favorites!";
            }

            return RedirectToAction("Index", "Suppliers");

        }

        public IActionResult RemoveFromFavorites(int supplierId, int userId)
        {
            _userId = Convert.ToInt32(User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid).Value);

            var favoritesList = GetSession(_userId);

            //1.Yöntem
            //var favoritesItem = favoritesList.SingleOrDefault(f => f.SupplierId == supplierId && f.UserId == userId);
            //favoritesList.Remove(favoritesItem);

            //2.Yöntem
            favoritesList.RemoveAll(f => f.SupplierId == supplierId && f.UserId == userId);
            SetSession(favoritesList);

            return RedirectToAction(nameof(GetFavorites));

        }
        public IActionResult ClearFavorites()
        {
            _userId = Convert.ToInt32(User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid).Value);

            var favoritesList = GetSession(_userId);
            favoritesList.RemoveAll(f => f.UserId == _userId);

            SetSession(favoritesList);

            return RedirectToAction(nameof(GetFavorites));
        }

        private List<FavoritesModel> GetSession (int userId)
        {
            var favoritesList = new List<FavoritesModel> ();

            var favoritesJson = HttpContext.Session.GetString(SESSIONKEY);

            if (!string.IsNullOrWhiteSpace(favoritesJson))
            {
               favoritesList =  JsonConvert.DeserializeObject<List<FavoritesModel>>(favoritesJson);
                favoritesList = favoritesList.Where(f => f.UserId == userId).ToList();
            }

            return favoritesList;
        }

        private void SetSession(List<FavoritesModel> favoritesList)
        {
            var favoritesJson = JsonConvert.SerializeObject(favoritesList);

            HttpContext.Session.SetString(SESSIONKEY, favoritesJson);
        }
    }
}
