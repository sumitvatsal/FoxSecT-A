using System;
using System.Data.Entity.Core.Objects;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Infrastructure.EF.Database
{
    internal class Database : ObjectContext, IDatabase
    {
        public Database(string connectionString, string defaultContainerName) : base(connectionString, defaultContainerName)
        {
            ContextOptions.LazyLoadingEnabled = false;
        }

        public new IObjectSet<TEntity> CreateObjectSet<TEntity>() where TEntity : Entity
        {
            return base.CreateObjectSet<TEntity>();
        }

        public new TEntity CreateObject<TEntity>() where TEntity : Entity
        {
            return base.CreateObject<TEntity>();
        }


        public void SubmitChanges()
        {
            //Added Try Catch..
            try
            {

                SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine("error:" + e);
            }
        }
    }
}