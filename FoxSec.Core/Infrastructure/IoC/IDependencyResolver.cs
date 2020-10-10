using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using FoxSec.Common.Helpers;

namespace FoxSec.Core.Infrastructure.IoC
{
	[ContractClass(typeof(IDependencyResolverContract))]
	public interface IDependencyResolver : IDisposable
	{
		void Register<T>(T instance);
		void Inject<T>(T existing);
		T Resolve<T>(Type type);
		T Resolve<T>(Type type, string name);
		T Resolve<T>();
		T Resolve<T>(string name);
		IEnumerable<T> ResolveAll<T>();
	}

	[ContractClassFor(typeof(IDependencyResolver))]
	internal abstract class IDependencyResolverContract : IDependencyResolver
	{
		void IDependencyResolver.Register<T>(T instance)
		{
			Contract.Requires(Check.Argument.IsNotNull(instance));
		}

		void IDependencyResolver.Inject<T>(T existing)
		{
			Contract.Requires(Check.Argument.IsNotNull(existing));
		}

		T IDependencyResolver.Resolve<T>(Type type)
		{
			Contract.Requires(Check.Argument.IsNotNull(type));

			return default(T);
		}

		T IDependencyResolver.Resolve<T>(Type type, string name)
		{
			Contract.Requires(Check.Argument.IsNotNull(type));
			Contract.Requires(Check.Argument.IsNotEmpty(name));

			return default(T);
		}

		T IDependencyResolver.Resolve<T>(string name)
		{
			Contract.Requires(Check.Argument.IsNotEmpty(name));

			return default(T);
		}

		public abstract T Resolve<T>();

		public abstract IEnumerable<T> ResolveAll<T>();

		public abstract void Dispose();
	}
}
