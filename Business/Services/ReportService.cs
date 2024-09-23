using Business.Models;
using Core.Repositories.EntityFramework.Bases;
using DataAccess.Entities;
using DataAccess.Repositories;

namespace Business.Services
{

    public interface IReportService
    {
        List<ReportItemModel> GetList(bool useInnerJoin = true, FilterModel filter = null);
    }
    public class ReportService : IReportService
    {
        private readonly RepoBase<Product> _productRepo;
        private readonly RepoBase<Order> _orderRepo;
        private readonly RepoBase<OrderDetail> _orderDetailRepo;
        private readonly RepoBase<Category> _categoryRepo;
        private readonly RepoBase<Supplier> _supplierRepo;

        public ReportService(RepoBase<Product> productRepo, RepoBase<OrderDetail> orderDetailRepo, RepoBase<Category> categoryRepo, RepoBase<Supplier> supplierRepo, RepoBase<Order> orderRepo)
        {
            _productRepo = productRepo;
            _orderDetailRepo = orderDetailRepo;
            _categoryRepo = categoryRepo;
            _supplierRepo = supplierRepo;
            _orderRepo = orderRepo;
        }

        public List<ReportItemModel> GetList(bool useInnerJoin = true, FilterModel filter = null)
        {
            #region Sorgu Oluşturma
            var productQuery = _productRepo.Query(); //product sorgusu
            var categoryQuery = _categoryRepo.Query(); //category sorgusu
            var supplierQuery = _supplierRepo.Query(); // supplier sorgusu
            var orderQuery = _orderRepo.Query(); //order sorgusu
            var orderDetailQuery = _orderDetailRepo.Query(); //order detail sorgusu

            IQueryable<ReportItemModel> query;

            if (useInnerJoin) // SQL Inner Join iki tablo arasında sadece primary key ve foreign key id'leri üzerinden eşleşenleri getirir. 
            {
                query = from product in productQuery
                        join orderdetail in orderDetailQuery
                        on product.Id equals orderdetail.ProductId
                        join order in orderQuery
                        on orderdetail.OrderId equals order.Id
                        join category in categoryQuery
                        on product.CategoryId equals category.Id
                        join supplier in supplierQuery
                        on product.SupplierId equals supplier.Id

                        select new ReportItemModel()
                        {
                            ProductName = product.Name,
                            ProductDescription = product.Description,
                            CategoryName = category.Name,
                            SupplierName = supplier.Name,
                            StockAmount = product.StockAmount + " units",
                            LotNo = product.LotNo.ToString(),
                            UnitPrice = product.UnitPrice.ToString("C2"),
                            OrderDate = order.Date.HasValue ? order.Date.Value.ToString("dd/MM/yyyy") : "",
                            DateValue = order.Date,
                            CategoryId = category.Id,
                            SupplierId = supplier.Id,

                        };
            }

            else // SQL Left Outer Join iki tablo arasında soldaki tablodan tüm kayıtları, sağdaki tablodan ise primary key ve foreign key id'leri
                 // üzerinden eşleşenler varsa eşleşenleri yoksa null getirir.
            {
                query = from p in productQuery

                        join c in categoryQuery
                        on p.CategoryId equals c.Id into categoryJoin
                        from category in categoryJoin.DefaultIfEmpty()
                        join s in supplierQuery
                        on p.SupplierId equals s.Id into supplierJoin
                        from supplier in supplierJoin.DefaultIfEmpty()
                        join od in orderDetailQuery
                        on p.Id equals od.ProductId into orderDetailjoin
                        from orderDetail in orderDetailjoin.DefaultIfEmpty()
                        join o in orderQuery
                        on orderDetail.OrderId equals o.Id into orderJoin
                        from order in orderJoin.DefaultIfEmpty()
                        select new ReportItemModel()
                        {
                            ProductName = p.Name,
                            ProductDescription = p.Description,
                            CategoryName = category.Name,
                            SupplierName = supplier.Name,
                            StockAmount = p.StockAmount + " units",
                            LotNo = p.LotNo.ToString(),
                            UnitPrice = p.UnitPrice.ToString("C2"),
                            OrderDate = order.Date.HasValue ? order.Date.Value.ToString("dd/MM/yyyy") : "",
                            DateValue = order.Date,
                            CategoryId = category.Id,
                            SupplierId = supplier.Id,
                        };
            }
                #endregion

            #region Sıralama
                query = query.OrderBy(q => q.CategoryName).ThenBy(q => q.ProductName);
            #endregion

            #region Filtreleme
            if (filter is not null)
            {
                if (!string.IsNullOrWhiteSpace(filter.ProductName))
                {
                    query = query.Where(q => q.ProductName.ToUpper().Contains(filter.ProductName.ToUpper().Trim()));
                }
                if (filter.DateBegin.HasValue)
                {
                    query = query.Where(q => q.DateValue >= filter.DateBegin.Value);
                }
                if (filter.DateBegin.HasValue)
                {
                    query = query.Where(q => q.DateValue <= filter.DateEnd.Value);
                }
                if (filter.CategoryId.HasValue)
                {
                    query = query.Where(q => q.CategoryId == filter.CategoryId.Value);
                }
                if (filter.SupplierId.HasValue)
                {
                    query = query.Where(q => q.SupplierId == filter.SupplierId.Value);
                }
            }
            #endregion

            return query.ToList();


        }
    }
}
