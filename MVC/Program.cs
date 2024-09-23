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

#region IoC(Inversion of Control) Container: Ba��ml�l�klar�n Y�netilmesi
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// appsettings.json veya appsettings.Development.json dosyalar�ndaki isim �zerinden atanan veritaban� ba�lant� string'ini d�ner.
builder.Services.AddDbContext<Db>(options => options.UseSqlServer(connectionString));
// projede herhangi bir class'ta Db tipinde constructor injection yap�ld���nda Db objesini new'leyerek o class'a enjekte eder.       

builder.Services.AddScoped(typeof(RepoBase<>), typeof(Repo<>));
// projede herhangi bir class'ta entity tipindeki RepoBase tipinde constructor injection yap�ld���nda entity tipindeki Repo objesini new'leyerek o class'a enjekte eder.

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
// projeye Cookie authentication default'lar�n� kullanarak kimlik do�rulama ekliyoruz
.AddCookie(config =>
// olu�turulacak cookie'yi config action delegesi �zerinden konfig�re ediyoruz, action delegeleri func delegeleri gibi bir sonu� d�nmez,
// �zerlerinden burada oldu�u gibi genelde konfig�rasyon i�lemleri yap�lor
{
    config.LoginPath = "/Account/Users/Login";
    // e�er sisteme giri� yap�lmadan bir i�lem yap�lmaya �al���l�rsa kullan�c�y� Account area -> Users controller -> Login action'�na y�nlendir
    config.AccessDeniedPath = "/Account/Users/AccessDenied";
    // e�er sisteme giri� yap�ld�ktan sonra yetki diye bir i�lem yap�lmaya �al���l�rsa kullan�c�y� Account area -> Users controller -> AccessDenied
    // action'�na y�nlendir
    config.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    // sisteme giri� yap�ld�ktan sonra olu�an cookie 30 dakika boyunca kullan�labilsin
    config.SlidingExpiration = true;
    // SlidingExpiration true yap�larak kullan�c� sistemde her i�lem yapt���nda cookie kullan�m s�resi yukar�da belirtilen 30 dakika uzat�l�r,
    // e�er false atan�rsa kullanc��n�n cookie �mr� yukar�da belirtilen 30 dakika sonra sona erer ve yeniden giri� yapmak zorunda kal�r
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

app.UseAuthorization(); //sen i�lem i�in yetkili misin?

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
