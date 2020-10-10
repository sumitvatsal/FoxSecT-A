using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
    internal class TimeZonePropertyRepository : RepositoryBase<TimeZoneProperty>, ITimeZonePropertyRepository
	{
        public TimeZonePropertyRepository(IDatabaseFactory factory) : base(factory) { }

        protected override IQueryable<TimeZoneProperty> All()
        {
            return (base.All() as ObjectSet<TimeZoneProperty>).Include("TimeZone");
        }
	}
}