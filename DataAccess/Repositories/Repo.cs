using Core.Records.Bases;
using Core.Repositories.EntityFramework.Bases;
using DataAccess.Context;

namespace DataAccess.Repositories
{
    public class Repo<TEntity> : RepoBase<TEntity> where TEntity : Record, new()
    {
        public Repo(Db dbContext) : base(dbContext)
        {
        }
    }
}

