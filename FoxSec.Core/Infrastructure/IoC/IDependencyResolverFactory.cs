namespace FoxSec.Core.Infrastructure.IoC
{
	public interface IDependencyResolverFactory
	{
		IDependencyResolver CreateInstance();
	}
}
