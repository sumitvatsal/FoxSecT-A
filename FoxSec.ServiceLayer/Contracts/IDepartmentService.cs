using System;

namespace FoxSec.ServiceLayer.Contracts
{
	public interface IDepartmentService
	{
        int CreateDepartment(string number, string name, string createdBy, int companyId);
        void DeleteDepartment(int id);
        void EditDepartment(int id, string number, string name, string modifiedBy, int companyId);
	}
}