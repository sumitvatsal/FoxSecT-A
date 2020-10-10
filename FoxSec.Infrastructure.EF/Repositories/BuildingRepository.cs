using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
    class BuildingRepository : LookupTitleRepositoryBase<Building>, IBuildingRepository
    {
        public BuildingRepository(IDatabaseFactory factory) : base(factory) { }

        protected override IQueryable<Building> All()
        {
            return (base.All() as ObjectSet<Building>).Include("Location.Country").Where(x => !x.IsDeleted);
        }

        
      
    }
}