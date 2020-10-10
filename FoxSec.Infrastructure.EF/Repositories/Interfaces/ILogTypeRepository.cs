using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Infrastructure.EF.Repositories
{
    public interface ILogTypeRepository : IRepository<LogType>
    {
		LogType FindByErrorNumber(int number);
    }
}