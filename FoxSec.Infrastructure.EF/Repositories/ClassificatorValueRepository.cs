using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
    internal class ClassificatorValueRepository : RepositoryBase<ClassificatorValue>, IClassificatorValueRepository
	{
        public ClassificatorValueRepository(IDatabaseFactory factory) : base(factory) { }
        public IEnumerable<ClassificatorValue> FindByClassificatorId(int id)
        {
            return All().Where(entity => entity.ClassificatorId == id).OrderBy(entity=>entity.Value.ToLower());
        }

        public IEnumerable<ClassificatorValue> FindByValue(string name)
        {
            return All().Where(entity => entity.Value == name).OrderBy(entity => entity.Value.ToLower());
        }
    }
}