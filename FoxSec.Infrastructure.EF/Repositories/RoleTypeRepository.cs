using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects ;
using System.Linq;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
    internal class RoleTypeRepository : LookupTitleRepositoryBase<RoleType>, IRoleTypeRepository
	{
        public RoleTypeRepository(IDatabaseFactory factory) : base(factory) {}
    }
}