using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using FoxSec.Common.Helpers;

namespace FoxSec.Core.Infrastructure.IoC
{
	public static class IoC
	{
		private static IDependencyResolver _resolver;

		[DebuggerStepThrough]
		public static void InitializeWith(IDependencyResolverFactory factory)
		{
			Contract.Requires(Check.Argument.IsNotNull(factory));
			_resolver = factory.CreateInstance();
		}

		[DebuggerStepThrough]
		public static void Register<T>(T instance)
		{
			Contract.Requires(Check.Argument.IsNotNull(instance));
			_resolver.Register(instance);
		}

		[DebuggerStepThrough]
		public static void Inject<T>(T existing)
		{
			Contract.Requires(Check.Argument.IsNotNull(existing));
			_resolver.Inject(existing);
		}

		[DebuggerStepThrough]
		public static T Resolve<T>(Type type)
		{
			Contract.Requires(Check.Argument.IsNotNull(type));
			return _resolver.Resolve<T>(type);
		}

		[DebuggerStepThrough]
		public static T Resolve<T>(Type type, string name)
		{
			Contract.Requires(Check.Argument.IsNotNull(type));
			Contract.Requires(Check.Argument.IsNotNull(name));
			return _resolver.Resolve<T>(type, name);
		}

		[DebuggerStepThrough]
		public static T Resolve<T>()
		{
			return _resolver.Resolve<T>();
		}

		[DebuggerStepThrough]
		public static T Resolve<T>(string name)
		{
			Contract.Requires(Check.Argument.IsNotNull(name));

			return _resolver.Resolve<T>(name);
		}

		[DebuggerStepThrough]
		public static IEnumerable<T> ResolveAll<T>()
		{
			return _resolver.ResolveAll<T>();
		}

		[DebuggerStepThrough]
		public static void Reset()
		{
			if( _resolver != null )
			{
				_resolver.Dispose();
			}
		}        
	}
}
