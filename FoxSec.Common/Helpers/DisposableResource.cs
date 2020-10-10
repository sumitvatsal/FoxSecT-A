using System;
using System.Diagnostics;

namespace FoxSec.Common.Helpers
{
	public abstract class DisposableResource : IDisposable
	{
		~DisposableResource()
		{
			Dispose(false);
		}

		[DebuggerStepThrough]
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
		}
	}
}
