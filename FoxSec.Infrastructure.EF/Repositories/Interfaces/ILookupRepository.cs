using System.Collections.Generic;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Infrastructure.EF.Repositories
{
	public interface ILookupRepository<TEntity> : IRepository<TEntity> where TEntity : LookupEntity
	{
		IEnumerable<TEntity> FindByDescription(string description);
	    IEnumerable<TEntity> FindByName(string name);
	}
}