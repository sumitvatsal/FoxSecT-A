using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Infrastructure.EF.Database
{
	public interface IDatabase : IDisposable
	{
		IObjectSet<TEntity> CreateObjectSet<TEntity>() where TEntity : Entity;

		TEntity CreateObject<TEntity>() where TEntity : Entity;

		void SubmitChanges();
	}
}
