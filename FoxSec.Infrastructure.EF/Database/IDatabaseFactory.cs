using System;

namespace FoxSec.Infrastructure.EF.Database
{
	public interface IDatabaseFactory : IDisposable
	{
		IDatabase Get();
	}
}
