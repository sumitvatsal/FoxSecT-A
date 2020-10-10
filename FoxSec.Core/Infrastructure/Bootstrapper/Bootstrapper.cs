using System;
using FoxSec.Common.Extensions;
using FoxSec.Core.Infrastructure.IoC;

namespace FoxSec.Core.Infrastructure.Bootstrapper
{
	public static class Bootstrapper
	{
		static Bootstrapper()
		{
			try
			{
				IoC.IoC.InitializeWith(new DependencyResolverFactory());
			}
			catch( ArgumentException )
			{
				// Config file is Missing
			}
		}

		public static void Run()
		{
			IoC.IoC.ResolveAll<IBootstrapperTask>().ForEach(t => t.Execute());
		}
	}
}
