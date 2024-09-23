using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    public class DbFactory : IDesignTimeDbContextFactory<Db>
    //scaffolding işlemleri için bu class oluşturulmalıdır(Db objesini oluşturup kullanılmasını sağlan fabrika class ı)
    {
        public Db CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<Db>();
            optionsBuilder.UseSqlServer("server=.\\SQLEXPRESS;database=StockTracking;user id=sa;password=sa;multipleactiveresultsets=true;trustservercertificate=true;");
            // önce veritabanımızın (development veritabanı kullanılması daha uygundur) connection string'ini içeren bir obje oluşturuyoruz
            return new Db(optionsBuilder.Options);
            // daha sonra yukarıda oluşturduğumuz obje üzerinden Db tipinde bir obje dönüyoruz
        }
    }
}

