using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects ;
using System.Linq;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
	internal class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : Entity
	{
		private readonly IObjectSet<TEntity> _objectSet;

		protected IDatabase Database { get; private set; }

		public RepositoryBase(IDatabase database)
		{
			Database = database;

		    _objectSet = database.CreateObjectSet<TEntity>();

		}

		public RepositoryBase(IDatabaseFactory factory) : this(factory.Get())
		{
		}

		protected virtual IQueryable<TEntity> All()
		{
			return _objectSet;
		}


		public virtual IEnumerable<TEntity> FindAll()
		{
			return All().ToList();
		}

        public virtual IEnumerable<TEntity> GetCount(int cnt)
        {
            return All().Take(cnt).ToList();
        }

        public IEnumerable<TEntity> FindAll(Func<TEntity, bool> exp)
        {
            return All().Where(exp).ToList();
        }

		public virtual TEntity FindById(int id)
		{
            //return All().Where(entity => entity.Id == id).SingleOrDefault();
            return All().Where(entity => entity.Id == id).FirstOrDefault();
        }

		public virtual void Add(TEntity entity)
		{
			_objectSet.AddObject(entity);
          

        }

		public virtual void Delete(TEntity entity)
		{
			_objectSet.DeleteObject(entity);
           
		}
      
	}
}