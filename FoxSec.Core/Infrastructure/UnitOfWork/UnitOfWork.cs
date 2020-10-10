using System.Diagnostics;

namespace FoxSec.Core.Infrastructure.UnitOfWork
{
	public static class UnitOfWork
	{
		[DebuggerStepThrough]
		public static IUnitOfWork Begin()
		{
			return IoC.IoC.Resolve<IUnitOfWork>();
		}
	}
}
