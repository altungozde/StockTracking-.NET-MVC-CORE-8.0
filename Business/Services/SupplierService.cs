using Business.Models;
using Core.Repositories.EntityFramework.Bases;
using Core.Results;
using Core.Results.Bases;
using Core.Services.Bases;
using DataAccess.Entities;

namespace Business.Services
{
    public interface ISupplierService : IService<SupplierModel>
    {
    }
    public class SupplierService : ISupplierService
    {
        private readonly RepoBase<Supplier> _supplierRepo;

        public SupplierService(RepoBase<Supplier> supplierRepo)
        {
            _supplierRepo = supplierRepo;
        }

        public IQueryable<SupplierModel> Query()
        {
            return _supplierRepo.Query().OrderBy(s  => s.Name).Select(s => new SupplierModel()
            {
                Guid = s.Guid,
                Id = s.Id,
                Name = s.Name,
                Address = s.Address,
                ContactNumber = s.ContactNumber,
                Description = s.Description,

            });
        }
        public Result Add(SupplierModel model)
        {
            if (Query().Any(s => s.Name.ToLower() == model.Name.ToLower().Trim()))
                return new ErrorResult("Supplier can't be added because supplier with the same name exists!");
            var entity = new Supplier()
            {
                Name = model.Name,
                Address = model.Address,
                ContactNumber = model.ContactNumber,
                Description = model.Description,
            };
            _supplierRepo.Add(entity);
                return new SuccessResult("Supplier added successfully!");
            
        }
        public Result Update(SupplierModel model)
        {
            if (_supplierRepo.Query().Any(s => s.Name.ToLower() == model.Name.ToLower().Trim() && s.Id != model.Id))

                    return new ErrorResult("Supplier can't be updated because supplier with the same name exists!");
            var entity = new Supplier()
            {
                Id = model.Id,
                Name = model.Name,
                Address = model.Address,
                ContactNumber = model.ContactNumber,
                Description = model.Description,

            };
            _supplierRepo.Update(entity);
            return new SuccessResult("Supplier updated successfully!");
        }

        public Result Delete(int id)
        {
            _supplierRepo.Delete(s => s.Id == id);

            return new SuccessResult("Supplier deleted successfully");
        }

        public void Dispose()
        {
            _supplierRepo.Dispose();
        } 
    }
}
