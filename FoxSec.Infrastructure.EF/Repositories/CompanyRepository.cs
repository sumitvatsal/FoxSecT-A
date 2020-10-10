using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects ;
using System.Linq;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
    internal class CompanyRepository : LookupTitleRepositoryBase<Company>, ICompanyRepository
	{
        public CompanyRepository(IDatabaseFactory factory) : base(factory) {}

        protected override IQueryable<Company> All()
        {
            return (base.All() as ObjectSet<Company>).Include("ClassificatorValue").Include("Users").Include("CompanyBuildingObjects.BuildingObject.Building").Where(x => !x.IsDeleted);
        }

        public IEnumerable<Company> FindByParentId(int parentId)
        {
            return All().Where(c => !c.IsDeleted && c.ParentId == parentId);
        }
	}
}