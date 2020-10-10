using System.Collections.Generic;
using System.Linq;
using FoxSec.DomainModel.DomainObjects;
using System;

namespace FoxSec.Infrastructure.EF.Repositories
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        IEnumerable<TEntity> FindAll();

        TEntity FindById(int id);

        void Add(TEntity entity);

        void Delete(TEntity entity);

        IEnumerable<TEntity> FindAll(Func<TEntity, bool> exp);

        IEnumerable<TEntity> GetCount(int cnt);
    }
}
