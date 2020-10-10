using System.Security.Principal;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Common.Enums;

namespace FoxSec.Authentication
{
	public class FoxSecIdentity : GenericIdentity, IFoxSecIdentity
	{
        public FoxSecIdentity(  int id,
                       
                                string loginName,
                                string firstName,
                                string lastName,
                                string email,                            
                                string authenticationType,                          
                                IPermissionSet permissions,
                                IMenuSet menues,
                                int roleId,
								int? roleTypeId,
                                string roleName,
                                int? companyId, 
								string hostName): base(loginName, authenticationType)
		{
            Id = id;
			FirstName = firstName;
			LastName = lastName;
			Email = email;
			Permissions = permissions;
            Menues = menues;
            RoleId = roleId;
        	RoleTypeId = roleTypeId.HasValue ? roleTypeId.Value : (int) FixedRoleType.DepartmentManager;
            RoleName = roleName;
            CompanyId = companyId;
        	HostName = hostName;
            IsBuildingAdmin = IsSuperAdmin = IsCompanyManager = IsCommonUser = IsDepartmentManager = false;
			IsBuildingAdmin = RoleTypeId == (int)FixedRoleType.Administrator;
			IsCompanyManager = RoleTypeId == (int)FixedRoleType.CompanyManager;
			IsCommonUser = RoleTypeId == (int)FixedRoleType.User;
			IsDepartmentManager = RoleTypeId == (int)FixedRoleType.DepartmentManager;
			IsSuperAdmin = RoleTypeId == (int)FixedRoleType.SuperAdmin;
        }

        public int Id { get; private set; }

		public string LoginName
		{
			get { return this.Name; }
		}
 
        public string FirstName { get; private set; }
		public string LastName { get; private set; }
		public string Email { get; private set; }
		public IPermissionSet Permissions { get; private set; }
        public IMenuSet Menues { get; private set; }
        public int? CompanyId { get; private set; }
        public int RoleId { get; private set; }
		public int RoleTypeId { get; private set; }
        public string RoleName { get; private set; }
        public bool IsBuildingAdmin { get; private set; }
        public bool IsCompanyManager { get; private set; }
        public bool IsCommonUser { get; private set; }
        public bool IsDepartmentManager { get; private set; }
		public bool IsSuperAdmin { get; private set; }
		public string HostName { get; private set; }
	}
}