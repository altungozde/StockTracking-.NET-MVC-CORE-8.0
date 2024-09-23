using Business.Models;
using Core.Repositories.EntityFramework.Bases;
using Core.Results;
using Core.Results.Bases;
using Core.Services.Bases;
using DataAccess.Entities;

namespace Business.Services
{
    public interface ICategoryService : IService<CategoryModel>
    {
    }
    public class CategoryService : ICategoryService
    {
        private readonly RepoBase<Category> _categoryRepo;

        public CategoryService(RepoBase<Category> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public IQueryable<CategoryModel> Query()
        {
            return _categoryRepo.Query().OrderBy(c => c.Name).Select(c => new CategoryModel()
            { 
                Guid = c.Guid,
                Id = c.Id,
                Description = c.Description,
                Name = c.Name,
                ProductsCount = c.Products.Count,
                ProductNameDisplay = string.Join("<br>", c.Products.Select(p => p.Name)), 
                

            });
        }
        public Result Add(CategoryModel model)
        {
            if (Query().Any(p => p.Name.ToLower() == model.Name.ToLower().Trim()))
                return new ErrorResult("Category can't be added because category with the same name exists!");
            var entity = new Category()
            {
                Description = model.Description?.Trim(), 
                Name = model.Name.Trim(),
                
            };
            _categoryRepo.Add(entity);
            return new SuccessResult("Category added successfully!");
        }
        public Result Update(CategoryModel model)
        {
            if (_categoryRepo.Query().Any(c => c.Name.ToLower() == model.Name.ToLower().Trim() && c.Id != model.Id)) // eğer düzenlediğimiz kategori dışında (Id koşulu üzerinden) bu ada sahip kategori varsa

                return new ErrorResult("Category can't be updated because category with the same name exists!");

            var entity = new Category()
            {
                Id = model.Id, // güncelleme işlemi için mutlaka Id set edilmeli
                Description = model.Description?.Trim(), 
                Name = model.Name.Trim(),
               //Products = model.ProductNameDisplay.
            };
            _categoryRepo.Update(entity);
            return new SuccessResult("Category updated successfully!"); //mesaj kullanmadan bir SuccessResult objesi oluşturduk ve döndük
        }
        public Result Delete(int id)
        {
            _categoryRepo.Delete(p => p.Id == id);

            return new SuccessResult("Category deleted successfully.");
        }
        public void Dispose()
        {
            _categoryRepo.Dispose();
        }
        
    }
}
