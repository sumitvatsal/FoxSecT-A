using System;
using System.Collections.Generic;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Infrastructure.EF.Repositories
{
    public interface IClassificatorValueRepository : IRepository<ClassificatorValue>
	{
        IEnumerable<ClassificatorValue> FindByClassificatorId(int id);

        IEnumerable<ClassificatorValue> FindByValue(string name);
    }
}