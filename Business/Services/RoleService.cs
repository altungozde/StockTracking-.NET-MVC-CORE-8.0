using Business.Models;
using Core.Repositories.EntityFramework.Bases;
using Core.Results.Bases;
using Core.Services.Bases;
using DataAccess.Entities;

namespace Business.Services
{
    public interface IRoleService : IService<RoleModel>
    {

    }
    public class RoleService : IRoleService
    {
        private readonly RepoBase<Role> _roleRepo;

        public RoleService(RepoBase<Role> roleRepo)
        {
            _roleRepo = roleRepo;
        }

        public IQueryable<RoleModel> Query()
        {
            throw new NotImplementedException();
        }
        public Result Add(RoleModel model)
        {
            throw new NotImplementedException();
        }

        public Result Update(RoleModel model)
        {
            throw new NotImplementedException();
        }
        public Result Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _roleRepo.Dispose();
        }
    }
}
