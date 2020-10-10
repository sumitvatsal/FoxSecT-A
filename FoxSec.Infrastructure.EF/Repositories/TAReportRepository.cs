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
    class TAReportRepository : LookupTitleRepositoryBase<TAReport>, ITAReportRepository
    {
        public TAReportRepository(IDatabaseFactory factory) : base(factory) { }

        protected override IQueryable<TAReport> All()
        {
            return (base.All() as ObjectSet<TAReport>).Include("User").Where(x => !x.IsDeleted);
        }

        public IEnumerable<TAReport> SelectByDate()
        {
            return (base.All() as ObjectSet<TAReport>).Include("User").Where(x => !x.IsDeleted);
        }
    }
}
