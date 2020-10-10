using System.Security.Principal;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Authentication.Extensions
{
	public static class IIdentityExtension
	{
		public static bool HasPermission(this IIdentity identity, Permission permission)
		{
			return ((FoxSecIdentity)identity).Permissions[permission];
		}

        public static bool HasMenu(this IIdentity identity, Menu menu)
        {
            return ((FoxSecIdentity) identity).Menues[menu];
        }
	}
}