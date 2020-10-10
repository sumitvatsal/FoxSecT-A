using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Core.Objects ;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
	internal class UserTimeZoneRepository: RepositoryBase<UserTimeZone>, IUserTimeZoneRepository
	{
        public UserTimeZoneRepository(IDatabaseFactory factory) : base(factory) { }

        protected override IQueryable<UserTimeZone> All()
        {
            return (base.All() as ObjectSet<UserTimeZone>).Include("UserTimeZoneProperties").Include("User");//.Include("TimeZone");
        }

        public IEnumerable<UserTimeZone> FindByUserId(int userId)
	    {
            return All().Where(utz => utz.UserId == userId && !utz.IsDeleted);
	    }
	}
}