using System;

namespace FoxSec.Core.Infrastructure.UnitOfWork
{
	public interface IUnitOfWork : IDisposable
	{
		void Commit();
	}
}
