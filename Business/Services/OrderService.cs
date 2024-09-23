using Business.Models;
using Core.Repositories.EntityFramework.Bases;
using Core.Results;
using Core.Results.Bases;
using Core.Services.Bases;
using DataAccess.Entities;

namespace Business.Services
{
    public interface IOrderService : IService<OrderModel>
    {
    }
    public class OrderService : IOrderService
    {
        private readonly RepoBase<Order> _orderRepo;
        private readonly RepoBase<OrderDetail> _orderDetailRepo;

        public OrderService(RepoBase<Order> orderRepo, RepoBase<OrderDetail> orderDetailRepo)
        {
            _orderRepo = orderRepo;
            _orderDetailRepo = orderDetailRepo;
        }

        public IQueryable<OrderModel> Query()
        {
            return _orderRepo.Query().OrderByDescending(o => o.Date).Select(o => new OrderModel()
            {
                Id = o.Id,
                Guid = o.Guid,
                Situation = o.Situation,
                Date = o.Date,
                Description = o.Description,
                UserId = o.UserId,

                UserNameDisplay = o.User.Name,
                DateDisplay = o.Date.HasValue ? o.Date.Value.ToString("dd/MM/yyyy") : "",

                ProductNameDisplay = string.Join("<br />", o.OrderDetails.Select(od => od.Product.Name)),
                //Many to many ilişkili kayıtları getirme
                //String i IEnumerable ye joinle atarız

                ProductIds = o.OrderDetails.Select(od => od.ProductId).ToList(),
                //orderla ilgili productıd leri orderdetails üzerinden atadık

            });
        }
        public Result Add(OrderModel model)
        {
            Order entity = new Order()
            {
            
                Date = DateTime.Now,
                Situation = model.Situation,
                UserId = model.UserId,
                Description = model.Description,

                OrderDetails = model.ProductIds?.Select(productId => new OrderDetail()
                {
                    ProductId = productId,
                    
                }).ToList(),
            };
            _orderRepo.Add(entity);

            return new SuccessResult("Order added successfully!");
        }
        public Result Update(OrderModel model)
        {
            
            _orderDetailRepo.Delete(od => od.OrderId == model.Id);

            Order entity = _orderRepo.Query().SingleOrDefault(o => o.Id == model.Id);

            entity.Situation = model.Situation;
            entity.UserId = model.UserId;
            entity.Description = model.Description;
            entity.OrderDetails = model.ProductIds?.Select(productId => new OrderDetail()
            {
                ProductId = productId
            }).ToList();
            _orderRepo.Update(entity);
           
            return new SuccessResult("Order updated successfully!");
        }

        public Result Delete(int id)
        {
            _orderRepo.Delete(p => p.Id == id);

            return new SuccessResult("Order deleted successfully.");
        }
        public void Dispose()
        {
            _orderRepo.Dispose();
        }
    }
}
