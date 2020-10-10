using System.Collections.Generic;
using System.Linq;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
	internal class LookupRepositoryBase<TEntity> : RepositoryBase<TEntity>, ILookupRepository<TEntity> where TEntity : LookupEntity
	{
		public LookupRepositoryBase(IDatabase database) : base(database) {}

		public LookupRepositoryBase(IDatabaseFactory factory) : base(factory) {}

		public virtual IEnumerable<TEntity> FindByDescription(string description)
		{
			return this.All().Where(entity => entity.Description == description).ToList();
		}

        public virtual IEnumerable<TEntity> FindByName(string name)
        {
            return this.All().Where(entity => entity.Name == name).ToList();
        }
	}
}