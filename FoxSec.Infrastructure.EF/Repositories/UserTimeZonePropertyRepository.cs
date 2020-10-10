using System.Collections.Generic;
using System.Data.Entity.Core.Objects ;
using System.Linq;
using System.Text;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
    internal class  UserTimeZonePropertyRepository : RepositoryBase<UserTimeZoneProperty>, IUserTimeZonePropertyRepository
    {
        public UserTimeZonePropertyRepository(IDatabaseFactory factory) : base(factory) { }

        protected override IQueryable<UserTimeZoneProperty> All()
        {
            return (base.All() as ObjectSet<UserTimeZoneProperty>)./*Include("TimeZone").*/Include("UserTimeZone.User");
        }
    }
}