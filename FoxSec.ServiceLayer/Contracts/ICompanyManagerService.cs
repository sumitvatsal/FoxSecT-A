using System;
using System.Collections.Generic;

namespace FoxSec.ServiceLayer.Contracts
{
	public interface ICompanyManagerService
	{
        void SaveCompanyManager(int companyId, int userId, string host);
	}
}