using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MVC.Controllers
{
    public class DbController : Controller
    {
        private readonly Db _db;

        public DbController(Db db)
        {
            _db = db;
        }

        public IActionResult Seed()
        {
            #region Mevcut verilerin silinmesi

           

            var users = _db.Users.ToList();
            _db.Users.RemoveRange(users);

            var roles = _db.Roles.ToList();
            _db.Roles.RemoveRange(roles);

            if (roles.Count > 0) // eğer veritabanında rol kaydı varsa eklenecek rollerin rol id'lerini aşağıdaki SQL komutu üzerinden 1'den başlayacak hale getiriyoruz
                                 // eğer kayıt yoksa o zaman zaten rol tablosuna daha önce veri eklenmemiştir dolayısıyla rol id'leri 1'den başlayacaktır
            {
                _db.Database.ExecuteSqlRaw("dbcc CHECKIDENT ('Roles', RESEED, 0)"); // ExecuteSqlRaw methodu üzerinden istenilen SQL sorgusu elle yazılıp veritabanında çalıştırılabilir
            }

            var suppliers = _db.Suppliers.ToList();
            _db.Suppliers.RemoveRange(suppliers);

            var orderDetails = _db.OrderDetails.ToList();
            _db.OrderDetails.RemoveRange(orderDetails);

            var products = _db.Products.ToList();
            _db.Products.RemoveRange(products);

            var orders = _db.Orders.ToList();
            _db.Orders.RemoveRange(orders);

            var categories = _db.Categories.ToList();
            _db.Categories.RemoveRange(categories);

            #endregion

            #region İlk verilerin oluşturulması

            _db.Roles.Add(new Role()
            {
                RoleName = "Admin", // 1
            });

            _db.Roles.Add(new Role()
            {
                RoleName = "User", // 2
            });

            _db.SaveChanges();

            _db.Users.Add(new User()
            {
                Name = "GZD",
                Password = "GZD",
                RoleId = (int)Roles.Admin

            });
            _db.Users.Add(new User()
            {
                Name = "Gzd1",
                Password = "Gzd1",
                RoleId = (int)Roles.User


            });
            _db.SaveChanges();
            
            _db.Suppliers.Add(new Supplier()
            {
                Name = "X Firması",
                Description = "X Malzemeleri",
                Address = "Ankara",
                ContactNumber = 0000,
                
            });

            _db.Suppliers.Add(new Supplier()
            {
                Name = "Y Firması",
                Description = "Y Malzemeleri",
                Address = "İstanbul",
                ContactNumber = 1111,
                
            });

            _db.Suppliers.Add(new Supplier()
            {
                Name = "Z Firması",
                Description = "Z Malzemeleri",
                Address = "İzmir",
                ContactNumber = 2222,

            });

            _db.SaveChanges();

            _db.Categories.Add(new Category()
            {
                Name = "Komponent",
                Description = "elektronik bir cihazın çalışmasına yardımcı olan devre sistemi",
                Products = new List<Product>()
                {
                    new Product()
                    {
                        Name = "Kondansatör",
                        Description = "Kapasitesi: 1 uF - Gerilim: 50 V",
                        StockAmount = 1,
                        LotNo = 010101,
                        UnitPrice = 20,
                        SupplierId = _db.Suppliers.SingleOrDefault(s => s.Name == "X Firması").Id,
                        
                    },
                }
            }
            );
            _db.Categories.Add(new Category()
            {
                Name = "Sensör",
                Description = "elektronik bir cihazın çalışmasına yardımcı olan devre sistemi",
                Products = new List<Product>()
                {
                    new Product()
                    {
                        Name = "Sıcaklık/Nem",
                        Description = "Sıcaklık Sensörü - Sıcaklık Aralığı: -55 ile 150 derece",
                        StockAmount = 1,
                        LotNo = 010101,
                        UnitPrice = 20,
                        SupplierId = _db.Suppliers.SingleOrDefault(s => s.Name == "Y Firması").Id,

                                         
                    },
                }

            });
            _db.SaveChanges();

            _db.Orders.Add(new Order()
            {
                Situation = "Acil",
                Date = new DateTime(2024, 11, 11),
                Description = "X projesi için ",
                UserId = _db.Users.SingleOrDefault(u => u.Name == "GZD").Id,
                OrderDetails = new List<OrderDetail>()
                {
                   new OrderDetail()
                   {
                       ProductId = _db.Products.SingleOrDefault(p  => p.Name == "Sıcaklık/Nem").Id
                   }
                }

            });
            _db.Orders.Add(new Order()
            {
                Situation = "Standart",
                Date = new DateTime(2024, 10, 11),
                Description = "Stok için ",
                UserId = _db.Users.SingleOrDefault(u => u.Name == "Gzd1").Id,
                OrderDetails = new List<OrderDetail>()
                {
                   new OrderDetail()
                   {
                       ProductId = _db.Products.SingleOrDefault(p  => p.Name == "Kondansatör").Id
                   }
                }
            });

            _db.SaveChanges();
           

            #endregion

            return View();
        }
    }
}
