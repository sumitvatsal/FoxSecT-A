using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Core.Objects;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;
using System.Threading.Tasks;
using System.Data.Entity;
using FoxSec.Infrastructure.EF;

namespace FoxSec.Infrastructure.EF.Repositories
{
    class TAMoveRepository : LookupTitleRepositoryBase<TAMove>, ITAMoveRepository
    {
        public TAMoveRepository(IDatabaseFactory factory) : base(factory) { }

        protected override IQueryable<TAMove> All()
        {
            return (base.All() as ObjectSet<TAMove>).Include("User").Where(x => !x.IsDeleted); //Include("User");
        }

        public IEnumerable<TAMove> SelectByDate()
        {
            return (base.All() as ObjectSet<TAMove>).Where(x => !x.IsDeleted).ToList();
        }
    }
}
