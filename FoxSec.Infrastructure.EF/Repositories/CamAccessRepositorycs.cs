using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
    
    internal class CamAccessRepository : RepositoryBase<CameraPermissions>, ICamAccessRepository
    {    
        public CamAccessRepository(IDatabaseFactory factory) : base(factory) { }   
    }
}
