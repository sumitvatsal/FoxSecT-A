using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
	class ControllerUpdateRepository : RepositoryBase<ControllerUpdate>, IControllerUpdateRepository
	{
		public ControllerUpdateRepository(IDatabaseFactory factory) : base(factory) { }

	}
}