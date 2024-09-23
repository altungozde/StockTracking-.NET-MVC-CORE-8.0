using Business.Models;
using Core.Repositories.EntityFramework.Bases;
using Core.Results;
using Core.Results.Bases;
using Core.Services.Bases;
using DataAccess.Entities;

namespace Business.Services
{
    public interface IUserService : IService<UserModel>
    {
    }
    public class UserService : IUserService
    {
        private readonly RepoBase<User> _userRepo;

        public UserService(RepoBase<User> userRepo)
        {
            _userRepo = userRepo;
        }
        public IQueryable<UserModel> Query()
        {
            return _userRepo.Query().OrderByDescending(u => u.Name).Select(u => new UserModel()
            {
                Guid = u.Guid,
                Id = u.Id,
                Name = u.Name,
                Password = u.Password,
                RoleId = u.RoleId,

                RoleNameDisplay = u.Role.RoleName,

                //Role = u.Role, Role atamam lazım ama bu şekilde yapama birinin tipi role model diğerinin ki entity bunu atayabilmem için yeni bir role model oluşturup içine role ile ilgili ihtiyacım olan name i user ın role nun name i üzerinde atarız
                Role = new RoleModel()
                {
                    RoleName = u.Role.RoleName,
                }
            });
        }
        public Result Add(UserModel model)
        {
            List<User> users = _userRepo.Query().ToList(); //user (entity) olucak çünkü repository kullanıyoruz

            if (users.Any(u => u.Name.Equals(model.Name, StringComparison.OrdinalIgnoreCase)))
                return new ErrorResult("User with  the same name exists!");

            User entity = new User()
            {
                Password = model.Password,
                RoleId = model.RoleId,
                Name = model.Name,
            };

            _userRepo.Add(entity);

            return new SuccessResult();

        }
        public Result Update(UserModel model)
        {
            throw new NotImplementedException();
        }

        public Result Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _userRepo.Dispose();
        }
    }
}

