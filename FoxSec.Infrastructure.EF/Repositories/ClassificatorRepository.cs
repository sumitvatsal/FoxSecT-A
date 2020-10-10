using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
    internal class ClassificatorRepository : RepositoryBase<Classificator>, IClassificatorRepository
	{
        public ClassificatorRepository(IDatabaseFactory factory) : base(factory) { }
	}
}