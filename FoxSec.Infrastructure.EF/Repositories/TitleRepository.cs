using System.Collections.Generic;
using System.Data.Entity.Core.Objects ;
using System.Linq;
using System.Text;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
	internal class TitleRepository : LookupTitleRepositoryBase<Title>, ITitleRepository
	{
		public TitleRepository(IDatabaseFactory factory) : base(factory) {}

        protected override IQueryable<Title> All()
        {
            return (base.All() as ObjectSet<Title>).Include("Company");
        }
	}
}