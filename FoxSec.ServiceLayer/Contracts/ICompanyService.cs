using System;
using System.Collections.Generic;
using FoxSec.ServiceLayer.Services;

namespace FoxSec.ServiceLayer.Contracts
{
	public interface ICompanyService
	{
        int CreateCompany(int? parentId, string name, string comment, bool isCanUseOwnCards, string host, IEnumerable<CompanyBuildingDto> companyBuildings=null);
        int CreateCompany(string name, string comment, bool isCanUseOwnCards, string host, IEnumerable<CompanyBuildingDto> companyBuildings);
        void UpdateCompany(int id, string name, string comment, string host);
        void UpdateCompany(int id, string name, string comment, bool isCanUseOwnCards, string host, IEnumerable<CompanyBuildingDto> companyBuildings);
        void DeleteCompany(int id, string host);
        void Activate(int id, int? classificatorValueId, string host);
		void Deactivate(int id, int? classificatorValueId, string host);
	    void SetState(int id, int? classificatorValueId, bool state, string host);
	    Dictionary<int, string> GetCompanyManagers(int companyId);
        void SaveSubComapnyDetails(int compid, List<int> complist,string host);

    }
}