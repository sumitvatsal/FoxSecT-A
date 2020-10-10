using System;
using System.Diagnostics.Contracts;
using FoxSec.Common.Helpers;
using FoxSec.Core.Infrastructure.Configuration;

namespace FoxSec.Core.Infrastructure.IoC
{
	class DependencyResolverFactory : IDependencyResolverFactory
	{
		private readonly Type _resolverType;

		public DependencyResolverFactory(string resolverTypeName)
		{
		  Contract.Requires(Check.Argument.IsNotEmpty(resolverTypeName));

			_resolverType = Type.GetType(resolverTypeName, true, true);
		}

		public DependencyResolverFactory() : this(new ConfigurationManagerWrapper().DependencyResolverTypeName) {}

		public IDependencyResolver CreateInstance()
		{
			return Activator.CreateInstance(_resolverType) as IDependencyResolver;
		}
	}
}
