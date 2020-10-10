using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects ;
using System.Linq;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
    internal class LocationRepository : LookupTitleRepositoryBase<Location>, ILocationRepository
    {
        public LocationRepository(IDatabaseFactory factory) : base(factory) { }

        protected override IQueryable<Location> All()
        {
            return (base.All() as ObjectSet<Location>).Include("Country").Where(x => !x.IsDeleted);
        }
    }
}