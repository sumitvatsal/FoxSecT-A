using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
    internal class CompanyBuildingObjectRepository : RepositoryBase<CompanyBuildingObject>, ICompanyBuildingObjectRepository
    {
        public CompanyBuildingObjectRepository(IDatabaseFactory factory) : base(factory) {}

        protected override IQueryable<CompanyBuildingObject> All()
        {
            return (base.All() as ObjectSet<CompanyBuildingObject>).Include("Company").Include("BuildingObject.Building").Include("BuildingObject.BuildingObjectType").Where(x => !x.IsDeleted);
        }
    }
}