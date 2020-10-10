using System;
using System.Diagnostics.Contracts;
using FoxSec.Common.Helpers;
using FoxSec.Core.Infrastructure.UnitOfWork;

namespace FoxSec.Infrastructure.EF.Database
{
	internal class UnitOfWork : DisposableResource, IUnitOfWork
	{
		private readonly IDatabase _database;
		private bool _isDisposed;

		public UnitOfWork(IDatabase database)
		{
			Contract.Requires(Check.Argument.IsNotNull(database));

			_database = database;
		}

		public UnitOfWork(IDatabaseFactory factory) : this(factory.Get())
		{
		}

		public virtual void Commit()
		{
			if( _isDisposed )
			{
				throw new ObjectDisposedException(GetType().Name);
			}

			_database.SubmitChanges();
		}

		protected override void Dispose(bool disposing)
		{
			if( !_isDisposed )
			{
				_isDisposed = true;
			}

			base.Dispose(disposing);
		}
	}
}
