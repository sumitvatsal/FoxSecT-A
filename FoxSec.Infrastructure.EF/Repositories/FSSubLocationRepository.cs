using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects ;
using System.Linq;
using System.Text;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;
using FoxSec.Common.Enums;
//using FoxSec.Infrastructure.EF.Repositories.Interfaces;

namespace FoxSec.Infrastructure.EF.Repositories
{
    internal class FSSubLocationRepository : RepositoryBase<FSSubLocation>, FoxSec.Infrastructure.EF.Repositories.Interfaces.IFSSubLocationRepository
    {
        public FSSubLocationRepository(IDatabaseFactory factory) : base(factory) { }

        protected override IQueryable<FSSubLocation> All()
        {
            return (base.All() as ObjectSet<FSSubLocation>);
        }
    }
}
