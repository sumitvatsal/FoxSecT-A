using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
    internal class TimeZoneRepository : LookupTitleRepositoryBase<TimeZone>, ITimeZoneRepository
	{
        public TimeZoneRepository(IDatabaseFactory factory) : base(factory) { }

        protected override IQueryable<TimeZone> All()
        {
            return (base.All() as ObjectSet<TimeZone>).Include("TimeZoneProperties").Include("UserTimeZoneProperties.UserTimeZone");
        }
	}
}