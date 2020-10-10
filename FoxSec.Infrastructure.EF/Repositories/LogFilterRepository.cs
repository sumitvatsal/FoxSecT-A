using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects ;
using System.Linq;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
	class LogFilterRepository : RepositoryBase<LogFilter>, ILogFilterRepository
	{
		public LogFilterRepository(IDatabaseFactory factory) : base(factory) { }

		protected override IQueryable<LogFilter> All()
		{
			return (base.All() as ObjectSet<LogFilter>).Include("User").Include("Company");
		}

		public override LogFilter FindById(int id)
		{
			return All().Where(entity => entity.Id == id && !entity.IsDeleted).SingleOrDefault();
		}
	}
}