using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
    class BuildingObjectRepository : RepositoryBase<BuildingObject>, IBuildingObjectRepository
    {
        public BuildingObjectRepository(IDatabaseFactory factory) : base(factory) { }

        protected override IQueryable<BuildingObject> All()
        {
            return (base.All() as ObjectSet<BuildingObject>).Include("BuildingObjectType").Include("Building").Include("Building.Location.Country").Where(x => !x.IsDeleted);
        }
    }
}