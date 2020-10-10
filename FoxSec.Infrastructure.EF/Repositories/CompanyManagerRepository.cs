using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects ;
using System.Linq;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
    internal class CompanyManagerRepository : RepositoryBase<CompanyManager>, ICompanyManagerRepository
    {
        public CompanyManagerRepository(IDatabaseFactory factory) : base(factory) { }

        protected override IQueryable<CompanyManager> All()
        {
            return (base.All() as ObjectSet<CompanyManager>).Include("Company").Include("User").Where(x => !x.IsDeleted);
        }

        public CompanyManager FindByCompanyId(int companyId)
        {
            return All().Where(cm => cm.CompanyId == companyId).FirstOrDefault();
        }

        public int GetCompanyManagerId(int companyId)
        {
            CompanyManager companyManager = FindByCompanyId(companyId);
            return companyManager == null ? -1 : companyManager.UserId; 
        }

        public string GetCompanyManagerName(int companyId)
        {
            CompanyManager companyManager = FindByCompanyId(companyId);
            return companyManager == null
                       ? string.Empty
                       : string.Format("{0} {1}", companyManager.User.FirstName, companyManager.User.LastName)
                ;
        }
    }
}