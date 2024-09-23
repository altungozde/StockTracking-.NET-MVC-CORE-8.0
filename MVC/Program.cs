using Business.Services;
using Core.Repositories.EntityFramework.Bases;
using DataAccess.Context;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using MVC.Settings;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);


#region Localization
List<CultureInfo> cultures = new List<CultureInfo>()
{
    new CultureInfo("tr-TR")
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture(cultures.FirstOrDefault().Name);
    options.SupportedCultures = cultures;
    options.SupportedUICultures = cultures;
});
#endregion

// Add services to the container.

#region IoC(Inversion of Control) Container: Baðýmlýlýklarýn Yönetilmesi
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// appsettings.json veya appsettings.Development.json dosyalarýndaki isim üzerinden atanan veritabaný baðlantý string'ini döner.
builder.Services.AddDbContext<Db>(options => options.UseSqlServer(connectionString));
// projede herhangi bir class'ta Db tipinde constructor injection yapýldýðýnda Db objesini new'leyerek o class'a enjekte eder.       

builder.Services.AddScoped(typeof(RepoBase<>), typeof(Repo<>));
// projede herhangi bir class'ta entity tipindeki RepoBase tipinde constructor injection yapýldýðýnda entity tipindeki Repo objesini new'leyerek o class'a enjekte eder.

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IReportService, ReportService>();
#endregion

builder.Services.AddControllersWithViews();

#region Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
// projeye Cookie authentication default'larýný kullanarak kimlik doðrulama ekliyoruz
.AddCookie(config =>
// oluþturulacak cookie'yi config action delegesi üzerinden konfigüre ediyoruz, action delegeleri func delegeleri gibi bir sonuç dönmez,
// üzerlerinden burada olduðu gibi genelde konfigürasyon iþlemleri yapýlor
{
    config.LoginPath = "/Account/Users/Login";
    // eðer sisteme giriþ yapýlmadan bir iþlem yapýlmaya çalýþýlýrsa kullanýcýyý Account area -> Users controller -> Login action'ýna yönlendir
    config.AccessDeniedPath = "/Account/Users/AccessDenied";
    // eðer sisteme giriþ yapýldýktan sonra yetki diye bir iþlem yapýlmaya çalýþýlýrsa kullanýcýyý Account area -> Users controller -> AccessDenied
    // action'ýna yönlendir
    config.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    // sisteme giriþ yapýldýktan sonra oluþan cookie 30 dakika boyunca kullanýlabilsin
    config.SlidingExpiration = true;
    // SlidingExpiration true yapýlarak kullanýcý sistemde her iþlem yaptýðýnda cookie kullaným süresi yukarýda belirtilen 30 dakika uzatýlýr,
    // eðer false atanýrsa kullancýýnýn cookie ömrü yukarýda belirtilen 30 dakika sonra sona erer ve yeniden giriþ yapmak zorunda kalýr
});
#endregion

#region Session
builder.Services.AddSession(config =>
{
    config.IdleTimeout = TimeSpan.FromDays(30);

    config.IOTimeout = Timeout.InfiniteTimeSpan;
});
#endregion

#region AppSettings
var section = builder.Configuration.GetSection(nameof(AppSettings));
section.Bind(new AppSettings());
#endregion

var app = builder.Build();

#region Localization
app.UseRequestLocalization(new RequestLocalizationOptions()
{
    DefaultRequestCulture = new RequestCulture(cultures.FirstOrDefault().Name),
    SupportedCultures = cultures,
    SupportedUICultures = cultures,
});
#endregion

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

#region UseAuthentication
app.UseAuthentication(); //sen kimsin?
#endregion

app.UseAuthorization(); //sen iþlem için yetkili misin?

#region Session
app.UseSession();
#endregion

#region Area
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});
#endregion

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
