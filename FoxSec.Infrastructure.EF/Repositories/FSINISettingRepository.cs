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
    class FSINISettingRepository : RepositoryBase<FSINISetting>, IFSINISettingRepository
    {
        public FSINISettingRepository(IDatabaseFactory factory) : base(factory) { }

        protected override IQueryable<FSINISetting> All()
        {
            return (base.All() as ObjectSet<FSINISetting>);
        }

    }
}
