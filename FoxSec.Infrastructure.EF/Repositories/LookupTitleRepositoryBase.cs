using System.Collections.Generic;
using System.Linq;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
    internal class LookupTitleRepositoryBase<TEntity> : RepositoryBase<TEntity>, ILookupTitleRepository<TEntity> where TEntity : EntityName
    {
        public LookupTitleRepositoryBase(IDatabase database) : base(database) { }

        public LookupTitleRepositoryBase(IDatabaseFactory factory) : base(factory) { }

        public virtual IEnumerable<TEntity> FindByName(string name)
        {
            return this.All().Where(entity => entity.Name == name).ToList();
        }
    }
}