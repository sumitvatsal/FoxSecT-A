using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Core.Objects;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;
using System.Threading.Tasks;

namespace FoxSec.Infrastructure.EF.Repositories
{
    internal class HolidayBuildingRepository : RepositoryBase<HolidayBuilding>, IHolidayBuildingRepository
    {
        public HolidayBuildingRepository(IDatabaseFactory factory) : base(factory) { }

        protected override IQueryable<HolidayBuilding> All()
        {
            return (base.All() as ObjectSet<HolidayBuilding>).Where(x => !x.IsDeleted);
        }
    }
}
