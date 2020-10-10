using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects ;
using System.Linq;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
    class UserAccessUnitTypeRepository : LookupTitleRepositoryBase<UserAccessUnitType>, IUserAccessUnitTypeRepository
    {
        public UserAccessUnitTypeRepository(IDatabaseFactory factory) : base(factory) { }

        protected override IQueryable<UserAccessUnitType> All()
        {
            return (base.All() as ObjectSet<UserAccessUnitType>).Where(x => !x.IsDeleted);
        }
    }
}