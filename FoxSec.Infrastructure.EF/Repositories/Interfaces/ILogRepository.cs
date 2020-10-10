using System.Collections.Generic;
using System.Resources;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Infrastructure.EF.Repositories
{
	public interface ILogRepository : IRepository<Log>
	{
        IEnumerable<Log> GetSerachedRecords(LogFilter logFilter, List<int> allowedUserIds, List<int> allowedCompanyIds, int? navPage, int? rows, int? sortDirection, int? sortField, out int searchedRowsCount);

        IEnumerable<Log> GetLocationRecords(LogFilter logFilter, List<int> allowedUserIds, List<int> allowedCompanyIds, int? navPage, int? rows, int? sortDirection, int? sortField, out int searchedRowsCount);

	    IEnumerable<Log> GetSearchedRecordsCommonSearch(LogFilter logFilter, List<int> allowedUserIds, List<int> restrUserIds,
	                                                           List<int> allowedCompanyIds, int? navPage, int? rowPerPage,
	                                                           int? sortDirection, int? sortField,
	                                                           out int searchedRowsCount, bool isUserCm, bool isUserSa, int? compid);
        IEnumerable<Log> GetSearchResultByIdList(List<int> logIds, List<int> logTypeIds);
        IEnumerable<Log> GetListOfLogsByLogIds(List<int> logIds);

    }
}