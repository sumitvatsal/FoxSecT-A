using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FoxSec.Common.Extensions;

namespace FoxSec.DomainModel.DomainObjects
{
	public enum Permission
	{
        [Display(Name = "Full arming own building")]
		FullArmingOwnBuilding = 1,
        [Display(Name = "Full arming all buildings")]
		FullArmingAllBuildings = 2,
        [Display(Name = "Full disarming own building")]
        FullDisarmingOwnBuilding = 3,
        [Display(Name = "Full disarming all buldings")]
        FullDisarmingAllBuldings = 4,
        [Display(Name = "Guard minmal own building")]
		GuardMinmalOwnBuilding = 5,
        [Display(Name = "Guard minmal all buildings")]
		GuardMinmalAllBuildings = 6,
        [Display(Name = "Accept own alarms")]
        AcceptOwnAlarms = 7,
        [Display(Name = "Accept all alarms own building")]
		AcceptAllAlarmsOwnBuilding = 8,
        [Display(Name = "Accept all alarms all buildings")]
		AcceptAllAlarmsAllBuildings = 9,
        [Display(Name = "Unlock keypads own building")]
		UnlockKeypadsOwnBuilding = 10,
        [Display(Name = "Unlock keypads all buildings")]
		UnlockKeypadsAllBuildings = 11,
        [Display(Name = "Installer own building")]
        InstallerOwnBuilding = 12,
        [Display(Name = "Installer all buildings")]
        InstallerAllBuildings = 13,
        [Display(Name = "Cleaner own buildings")]
        CleanerOwnBuilding = 14,
        [Display(Name = "Cleaner all buildings")]
        CleanerAllBuildings = 15,
        [Display(Name = "Global User")]
        GlobalUser = 16
    }

	public interface IPermissionSet : IEnumerable<KeyValuePair<Permission, bool>>
	{
		bool this[Permission p] { get; set; }
		byte[] GetByteArray();
		IPermissionSet AsReadOnly();
	}

	public class PermissionSet : IPermissionSet
	{
		private readonly IDictionary<Permission, bool> _permissionMap;
		private readonly byte[] _permissions;

		public PermissionSet(byte[] permissions)
		{
			_permissionMap = new Dictionary<Permission, bool>(permissions.Length);
			_permissions = permissions;
			InitializePermissionMap(permissions, _permissionMap);
		}

		private static void InitializePermissionMap(byte[] permissions, IDictionary<Permission, bool> permissionMap)
		{
			for( int i = 0; i < permissions.Length; i++ )
			{
				var p = i.AsEnum<Permission>();

				if( Enum.IsDefined(typeof(Permission), p) )
				{
					permissionMap.Add(p, permissions[i] != 0);
				}
			}
		}

		public bool this[Permission p]
		{
			get
			{
				bool granted;
				_permissionMap.TryGetValue(p, out granted);
				return granted;
			}

			set
			{
				_permissionMap[p] = value;
				_permissions[(int)p] = (byte)(value ? 1 : 0);
			}
		}

		public static IPermissionSet Merge(IPermissionSet left, IPermissionSet right)
		{
			var result = new PermissionSet(left.GetByteArray());

			foreach( var permission in left )
			{
				result[permission.Key] = result[permission.Key] || right[permission.Key];
			}

			return result;
		}

		public byte[] GetByteArray()
		{
			return _permissions;
		}

		public IPermissionSet AsReadOnly()
		{
			return new PermissionSetReadOnly(this);
		}

		public IEnumerator<KeyValuePair<Permission, bool>> GetEnumerator()
		{
			return _permissionMap.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}

	internal class PermissionSetReadOnly : IPermissionSet
	{
		private readonly PermissionSet _permissionSet;

		public PermissionSetReadOnly(PermissionSet permissionSet)
		{
			_permissionSet = permissionSet;
		}

		public bool this[Permission p]
		{
			get { return _permissionSet[p]; }
			set { throw ThrowOnMutation(); }
		}

		public byte[] GetByteArray()
		{
			throw ThrowOnMutation();
		}

		public IPermissionSet AsReadOnly()
		{
			return this;
		}

		public IEnumerator<KeyValuePair<Permission, bool>> GetEnumerator()
		{
			return _permissionSet.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private static Exception ThrowOnMutation()
		{
			return new InvalidOperationException("Operation is not allowed on read-only permission set.");
		}
	}
}