using System.Collections.Generic;
using FoxSec.ServiceLayer.ServiceResults;
using FoxSec.ServiceLayer.Services;

namespace FoxSec.ServiceLayer.Contracts
{
    public interface IRoleService
	{
		RoleCreateResult CreateRole(string name, string description, string createdBy, bool active, int roleTypeId, IEnumerable<RoleBuildingDto> buildings, IEnumerable<int> permissionIds = null, IEnumerable<int> menuIds = null);
        RoleCreateResult EditRole(int id, string name, string description, string modifiedBy, bool active, int roleTypeId, IEnumerable<RoleBuildingDto> buildings, IEnumerable<int> permissionIds = null, IEnumerable<int> menuIds = null);
        void DeleteRole(int id);
	}
}