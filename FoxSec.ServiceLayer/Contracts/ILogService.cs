using System;

namespace FoxSec.ServiceLayer.Contracts
{
	public interface ILogService
	{
		int CreateLog(int userId, string building,string flag, string node, int? companyId, string action, int? logTypeId=null);

        int CreateLogWorkLeave(int userId, int boid, string flag, int? companyId, string action, DateTime eventtime, int logTypeId, string building, string node, string evntkey);

    }
}