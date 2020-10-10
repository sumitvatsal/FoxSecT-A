using System.Collections.Generic;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Infrastructure.EF.Repositories
{
    public interface IUserTimeZoneRepository : IRepository<UserTimeZone>
    {
        IEnumerable<UserTimeZone> FindByUserId(int userId);
    }
}