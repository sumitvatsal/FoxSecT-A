using System.ComponentModel.DataAnnotations;

namespace FoxSec.Common.Enums
{
    public enum FixedUserRole
    {
        [Display(Name = "User")]
        User = 4,
        [Display(Name = "Company Manager")]
        CompanyManager = 3,
        [Display(Name = "Building Administrator")]
        Administrator = 31,
        [Display(Name = "Super-Admin")]
        SuperAdmin = 5,
        [Display(Name = "Department Manager")]
        DepartmentManager = 39
    }

	public enum FixedRoleType
	{
		[Display(Name = "Super-Admin")]
		SuperAdmin = 1,
		[Display(Name = "Building Administrator")]
		Administrator = 2,
		[Display(Name = "Company Manager")]
		CompanyManager = 3,
		[Display(Name = "Department Manager")]
		DepartmentManager = 4,
		[Display(Name = "User")]
		User = 5
	}
}
