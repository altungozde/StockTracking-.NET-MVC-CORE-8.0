using Business.Models;
using Core.Repositories.EntityFramework.Bases;
using Core.Results;
using Core.Results.Bases;
using Core.Services.Bases;
using DataAccess.Entities;

namespace Business.Services
{
    public interface IProductService : IService<ProductModel>
    {
    }
    public class ProductService : IProductService
    {
        private readonly RepoBase<Product> _productRepo;

        public ProductService(RepoBase<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public IQueryable<ProductModel> Query()
        // Read işlemi: repository üzerinden entity ile aldığımız verileri modele dönüştürerek veritabanı sorgumuzu oluşturuyoruz.
        // Bu method sadece sorgu oluşturur ve döner, çalıştırmaz. Çalıştırmak için ToList, SingleOrDefault vb. methodlar kullanılmalıdır.
        {
            // Repository üzerinden entity sorgusunu (Query) oluşturuyoruz,
            // daha sonra Select ile sorgu kolleksiyonundaki her bir entity için model dönüşümünü gerçekleştiriyoruz (projeksiyon işlemi).
            return _productRepo.Query().Select(product => new ProductModel()
            {
                CategoryId = product.CategoryId,
                Description = product.Description,
                LotNo = product.LotNo,
                Id = product.Id,
                Guid = product.Guid,
                Name = product.Name,
                StockAmount = product.StockAmount,
                UnitPrice = product.UnitPrice,
                SupplierId = product.SupplierId,

                LotNoDisplay = product.LotNo.ToString(),
                UnitPriceDisplay = product.UnitPrice.ToString("C2"),
                CategoryNameDisplay = product.Category.Name,
                SupplierNameDisplay = product.Supplier.Name,

                Image = product.Image,
                ImageExtension = product.ImageExtension,

                ImgSrcDisplay = product.Image != null ?
                (product.ImageExtension == ".jpg" || product.ImageExtension == ".jpeg" ? 
                "data:image/jpeg;base64," : 
                "data:image/png;base64,") + Convert.ToBase64String(product.Image) : null,
            });
        }     
        public Result Add(ProductModel model)
        {
            // Önce model üzerinden girilen ürün adına sahip entity veritabanındaki tabloda var mı diye kontrol ediyoruz, eğer varsa işleme izin vermeyip ErrorResult objesi dönüyoruz
            if (Query().Any(p => p.Name.ToLower() == model.Name.ToLower().Trim()))
            // Any LINQ methodu belirtilen koşul veya koşullara sahip herhangi bir kayıt var mı diye veritabanındaki tabloda kontrol eder, varsa true yoksa false döner.
            return new ErrorResult("Product can't be added because product with the same name exists!");
            // bu ürün adına sahip kayıt bulunmaktadır mesajını içeren ErrorResult objesini dönüyoruz ki ilgili controller action'ında kullanabilelim.
            
            Product entity = new Product()
            {
                //CategoryId = model.CategoryId ?? 0,//entity CategoryId özelliğine
                //eğer
                                                   //modelin CategoryId'si null
                                                   //gelirse 0, dolu gelirse gelen
                                                   //değeri ata,bu yöntemde dikkat
                                                   //edilmesi gereken veritabanındaki
                                                   //tabloya insert işlemi
                                                   //yapıldığında CategoryId'nin
                                                   //atanması durumunda kategori
                                                   //tablosunda 0 Id'li bir kategori
                                                   //bulunmadığından exception
                                                   //alınacağıdır.
                CategoryId = model.CategoryId.Value,//exception veritabanından
                                                    //gelmesin diye bunu tercih ettim
                Description = model.Description?.Trim(),
                Name = model.Name,
                LotNo= model.LotNo,
                StockAmount = model.StockAmount,
                UnitPrice = model.UnitPrice,
                SupplierId = model.SupplierId.Value,
                Image = model.Image,
                ImageExtension = model.ImageExtension,
            };
            _productRepo.Add(entity);

            return new SuccessResult("Product added successfully!");
        }
        public Result Update(ProductModel model)
        {
            if (Query().Any(p => p.Name.ToLower() == model.Name.ToLower().Trim() && p.Id != model.Id))

                return new ErrorResult("Product can't be updated because product with the same name exists!");

            // güncellenen ürün dışında (yukarıda Id üzerinden bu koşulu ekledik) bu ürün adına sahip kayıt bulunmaktadır mesajını içeren ErrorResult objesini dönüyoruz ki ilgili controller action'ında kullanabilelim.

            Product entity = _productRepo.Query().SingleOrDefault(p => p.Id == model.Id); 
            //veritabanından çekiyoruz bilgileri
            
            entity.CategoryId = model.CategoryId.Value;
            entity.Description = model.Description?.Trim();
            entity.Name = model.Name.Trim();
            entity.LotNo = model.LotNo;
            entity.StockAmount = model.StockAmount;
            entity.UnitPrice = model.UnitPrice;
            entity.SupplierId = model.SupplierId.Value;
            if (model.Image is not null)
            {
                entity.Image = model.Image;
                entity.ImageExtension = model.ImageExtension;
            }
            
            _productRepo.Update(entity);

            return new SuccessResult("Product updated successfully!");
        }
        public Result Delete(int id)
        {
            _productRepo.Delete(p => p.Id == id);

            return new SuccessResult("Product deleted successfully.");
        }
        public void Dispose()
        {
            _productRepo.Dispose();
        }

        public Result DeleteImage (int id)
        {
            Product entity = _productRepo.Query().SingleOrDefault(p => p.Id == id);
            if (entity.Image is null)
                return new ErrorResult("Product has no image to delete!");

            entity.ImageExtension = null;
            entity.Image = null;

            _productRepo.Update(entity);

            return new SuccessResult("Product image deleted succesfully!");
        }
    }
}
