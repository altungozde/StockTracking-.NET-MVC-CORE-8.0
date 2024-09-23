#nullable disable

using Business.Models;
using Business.Services;
using Core.Results.Bases;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MVC.Areas.Account.Controllers
{
    [Area("Account")]
    public class UsersController : Controller
    {
        private readonly IAccountService _accountService;

        public UsersController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        //// GET: Account/Users
        //public IActionResult Index()
        //{
        //    return View();
        //}

        //// GET: Account/Users/Details/5
        //public IActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    return View();
        //}

        // GET: Account/Users/Login
        public IActionResult Login(string returnUrl)
        {
            AccountLoginModel model = new AccountLoginModel()
            {
                ReturnUrl = returnUrl,
            };
            return View(model);
        }

        // POST: Account/Users/Login
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AccountLoginModel model) //giriş
        {
            if (ModelState.IsValid)
            {
                UserModel userResultModel = new UserModel()
                {
                    Role = new RoleModel()
                };
                // UserModel tipindeki userResult'ı burada tanımlayıp new'liyoruz ki Login methodu  
                // başarılı olursa userResult referans tip olduğu için Login methodu içerisinde atansın 
                // ve bu methodda kullanabilelim, userResult içerisindeki Role referans özelliğini de new'liyoruz ki
                // methodda rolü de doldurabilelim
                Result result = _accountService.Login(model, userResultModel);
                if (result.IsSuccessful)
                {
                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name,userResultModel.Name),
                        new Claim(ClaimTypes.Role, userResultModel.Role.RoleName),
                        new Claim(ClaimTypes.Sid, userResultModel.Id.ToString()),
                    };

                    // oluşturduğumuz claim listesi üzerinden cookie authentication default'ları ile bir identity (kimlik) oluşturuyoruz
                    var identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);

                    // oluşturduğumuz kimlik üzerinden de MVC'de authentication (kimlik doğrulama) için kullanacağımız bir principal oluşturuyoruz 
                    var principal = new ClaimsPrincipal(identity);

                    // son olarak oluşturduğumuz principal üzerinden cookie authentication default'ları ile MVC'de kimlik giriş işlemini tamamlıyoruz,
                    // SignInAsync methodu bir asenkron method olduğu için başına await (asynchronous wait) yazmalıyız
                    // ve methodun dönüş tipinin başına async yazarak dönüş tipini de bir Task tipi içerisinde tip olarak (IActionResult) tanımlamalıyız
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    if (!string.IsNullOrWhiteSpace(model.ReturnUrl)) // giriş işlemi başarılı olduğu için kullanıcıyı ReturnUrl doluysa ReturnUrl üzerinden login'e geldiği controller ve action'a yönlendiriyoruz

                        return Redirect(model.ReturnUrl);
                    
                    return RedirectToAction("Index", "Home", new {area = ""});// eğer ReturnUrl boşsa kullanıcıyı hoşgeldin view'ını dönen Home controller -> Index action'ına area'sı olmadığı için route value'da area = "" atayarak yönlendiriyoruz
                }
                ModelState.AddModelError("", result.Message);
            }
           
            return View();
        }

        public IActionResult Logout() //çıkış
        {
            HttpContext.SignOutAsync();//login aksiyonu ile oluşan çerezi(cookie) kaldırır

            // projenin Home controller -> Index action'ı bir area'nın içerisinde olmadığı için area özelliğini içeren anonim tipteki objeyi route value parametresi olarak ve "" atayarak oluşturuyoruz
            return RedirectToAction("Index", "Home", new { area = "" });
        }
        public IActionResult AccessDenied()
            // kullanıcı giriş yaptı ancak yetkisi olmayan bir controller action'ını çağırdı
        {
            return View("_Error", "Access is denied to this page!");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register (AccountRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                Result result = _accountService.Register(model);
                if (result.IsSuccessful)           
                    return RedirectToAction(nameof(Login));
                ModelState.AddModelError("",result.Message);
            }

            return View(model);
        }
    }
}
