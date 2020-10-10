using System.Collections.Generic;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Infrastructure.EF.Repositories
{
    public interface ILookupTitleRepository<TEntity> : IRepository<TEntity> where TEntity : EntityName
	{
	    IEnumerable<TEntity> FindByName(string name);
	}
}