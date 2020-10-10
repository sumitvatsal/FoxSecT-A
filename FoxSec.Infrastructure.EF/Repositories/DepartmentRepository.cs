using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects ;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
	internal class DepartmentRepository : LookupTitleRepositoryBase<Department>, IDepartmentRepository
	{
        public DepartmentRepository(IDatabaseFactory factory) : base(factory) { }

        protected override IQueryable<Department> All()
        {
            return (base.All() as ObjectSet<Department>).Include("Company").Include("UserDepartments.User").Where(x => !x.IsDeleted);
         
        }
	}
}