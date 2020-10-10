using System.Collections.Generic;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Infrastructure.EF.Repositories
{
	public interface ICompanyRepository : ILookupTitleRepository<Company>
	{
	    IEnumerable<Company> FindByParentId(int parentId);
	}
}