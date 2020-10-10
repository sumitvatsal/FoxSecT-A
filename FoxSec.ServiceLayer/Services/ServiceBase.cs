using FoxSec.Authentication;
using FoxSec.Common.EventAggregator;
using FoxSec.DomainModel;

namespace FoxSec.ServiceLayer.Services
{
	internal abstract class ServiceBase
	{
		protected ICurrentUser CurrentUser { get; private set; }
		protected IDomainObjectFactory DomainObjectFactory { get; private set; }
		protected IEventAggregator EventAggregator { get; private set; }
		
		protected ServiceBase(ICurrentUser currentUser, IDomainObjectFactory domainObjectFactory, IEventAggregator eventAggregator)
		{
			CurrentUser = currentUser;
			DomainObjectFactory = domainObjectFactory;
			EventAggregator = eventAggregator;
		}
	}
}
