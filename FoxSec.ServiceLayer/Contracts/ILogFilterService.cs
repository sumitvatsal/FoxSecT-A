using System;

namespace FoxSec.ServiceLayer.Contracts
{
	public interface ILogFilterService
	{
		int CreateLogFilter(int userId, string userName, string building, string node, string name, int? companyId, DateTime? fromDate, DateTime? toDate, string activity, string host, bool isShowDefaultLog);

		int EditLogFilter(int id, string userName, string building, string node, string name, int? companyId, DateTime? fromDate, DateTime? toDate, string activity, string host, bool isShowDefaultLog);

		void DeleteLogFilter(int id, string host);
	}
}