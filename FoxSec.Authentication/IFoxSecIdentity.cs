using System.Security.Principal;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Authentication
{
	public interface IFoxSecIdentity : IIdentity
	{
        int Id { get;}
        string LoginName { get; }
		string FirstName { get; }
		string LastName { get; }
		string Email { get; }
		IPermissionSet Permissions { get; }
	    IMenuSet Menues { get; }
        int? CompanyId { get; }
	    int RoleId { get; }
		int RoleTypeId { get; }
	    string RoleName { get; }
        bool IsBuildingAdmin { get; }
        bool IsCompanyManager { get; }
        bool IsCommonUser { get; }
        bool IsDepartmentManager { get; }
		bool IsSuperAdmin { get; }
		string HostName { get; }
	}
}