using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
    internal class VideoAccessRepository : RepositoryBase<VideoAccess>, IVideoAccessRepository
    {
        public VideoAccessRepository(IDatabaseFactory factory) : base(factory) { }

    }
}



