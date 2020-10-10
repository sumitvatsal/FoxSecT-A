using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Infrastructure.EF.Repositories
{
    public interface ICompanyManagerRepository : IRepository<CompanyManager>
    {
        CompanyManager FindByCompanyId(int companyId);
        int GetCompanyManagerId(int companyId);
        string GetCompanyManagerName(int companyId);
    }
}