using System;
using FoxSec.Authentication;
using FoxSec.Common.EventAggregator;
using FoxSec.Core.Infrastructure.UnitOfWork;
using FoxSec.DomainModel;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.ServiceLayer.Contracts;


namespace FoxSec.ServiceLayer.Services
{
	internal class LogService : ServiceBase, ILogService
	{
		private readonly ILogRepository _logRepository;
      
        public LogService(ICurrentUser currentUser,
										IDomainObjectFactory domainObjectFactory,
										IEventAggregator eventAggregator,
										ILogRepository logRepository) : base(currentUser, domainObjectFactory, eventAggregator)
		{
			_logRepository = logRepository;
		}

		public int CreateLog(int userId, string building,string flag, string node, int? companyId, string action, int? logTypeId = null)
		{
            if(companyId==0)
            {
                companyId = null;
            }

            int result = 0;
			using( IUnitOfWork work = UnitOfWork.Begin() )
			{
				Log log = DomainObjectFactory.CreateLog();

				log.UserId = userId;
				log.CompanyId = companyId;
				log.Action = action;
				log.Building = building;
				log.Node = node;

                if (flag == "Log")
                {
                    log.LogTypeId = 15;
                }
                else
                {
                    if (!logTypeId.HasValue)
                    {
                        log.LogTypeId = (int)LogTypeEnum.WebInfo;
                    }
                    else
                    {
                        log.LogTypeId = logTypeId.Value;
                    }
                }

                log.EventTime = DateTime.Now;
				_logRepository.Add(log);
              
                work.Commit();
				result = log.Id;
			}
			
			return result;
		}

        public int CreateLogWorkLeave(int userId, int boid, string flag, int? companyId, string action, DateTime eventtime, int logTypeId, string building, string node, string evntkey)
        {
            if (companyId == 0)
            {
                companyId = null;
            }
           
            int result = 0;
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                Log log = DomainObjectFactory.CreateLog();

                log.Action = action;
                log.UserId = userId;
                log.EventTime = eventtime;
                log.CompanyId = companyId;
                log.BuildingObjectId = boid;
                log.LogTypeId = logTypeId;
                log.Building = building;
                log.Node = node;
                log.EventKey = evntkey;
                _logRepository.Add(log);

                work.Commit();
                result = log.Id;
            }

            return result;
        }
    }
}