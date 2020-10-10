using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
	internal class HolidayRepository : LookupTitleRepositoryBase<Holiday>, IHolidayRepository
	{
        public HolidayRepository(IDatabaseFactory factory) : base(factory) { }
	}
}