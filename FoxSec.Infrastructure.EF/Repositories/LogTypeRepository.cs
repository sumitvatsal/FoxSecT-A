using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects ;
using System.Linq;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
	class LogTypeRepository : RepositoryBase<LogType>, ILogTypeRepository
	{
		public LogTypeRepository(IDatabaseFactory factory) : base(factory) { }

		public LogType FindByErrorNumber(int number)
		{
			return All().Where(entity => entity.NumberOfError == number).SingleOrDefault();
		}
	}
}