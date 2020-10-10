using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
    internal class CameraAccessRepository : RepositoryBase<CameraPermissions>, ICameraAccessRepository
    {
                
      public CameraAccessRepository(IDatabaseFactory factory) : base(factory) { }
    }
}



