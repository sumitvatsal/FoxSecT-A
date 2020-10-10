using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using FoxSec.Common.Helpers;
using FoxSec.Core.Infrastructure.IoC;

namespace FoxSec.Infrastructure.EntLib.Unity
{
	class UnityDependencyResolver : DisposableResource, IDependencyResolver
	{
		private readonly IUnityContainer _container;

		[DebuggerStepThrough]
		public UnityDependencyResolver(): this(new UnityContainer())
		{
			var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
			section.Configure(_container);
		}

		[DebuggerStepThrough]
		public UnityDependencyResolver(IUnityContainer container)
		{
			Contract.Requires(Check.Argument.IsNotNull(container));

			_container = container;
		}

		[DebuggerStepThrough]
		public void Register<T>(T instance)
		{
			_container.RegisterInstance(instance);
		}

		[DebuggerStepThrough]
		public void Inject<T>(T existing)
		{
			_container.BuildUp(existing);
		}

		[DebuggerStepThrough]
		public T Resolve<T>(Type type)
		{
			return (T)_container.Resolve(type);
		}

		[DebuggerStepThrough]
		public T Resolve<T>(Type type, string name)
		{
			return (T)_container.Resolve(type, name);
		}

		[DebuggerStepThrough]
		public T Resolve<T>()
		{
			return _container.Resolve<T>();
		}

		[DebuggerStepThrough]
		public T Resolve<T>(string name)
		{
			return _container.Resolve<T>(name);
		}

		[DebuggerStepThrough]
		public IEnumerable<T> ResolveAll<T>()
		{
			IEnumerable<T> named_instances = _container.ResolveAll<T>();
			T unnamed_instance = default(T);

			try
			{
				unnamed_instance = _container.Resolve<T>();
			}
			catch( ResolutionFailedException )
			{
				//When default instance is missing
			}

			if( Equals(unnamed_instance, default(T)) )
			{
				return named_instances;
			}

			return new ReadOnlyCollection<T>(new List<T>(named_instances) { unnamed_instance });
		}

		[DebuggerStepThrough]
		protected override void Dispose(bool disposing)
		{
			if( disposing )
			{
				_container.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}
