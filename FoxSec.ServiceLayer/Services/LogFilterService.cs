using System;
using FoxSec.Authentication;
using FoxSec.Common.EventAggregator;
using FoxSec.Core.Infrastructure.UnitOfWork;
using FoxSec.Core.SystemEvents;
using FoxSec.DomainModel;
using System.Linq;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.ServiceLayer.Contracts;

namespace FoxSec.ServiceLayer.Services
{
	internal class LogFilterService : ServiceBase, ILogFilterService
	{
		private readonly ILogFilterRepository _logFilterRepository;
		private readonly ILogService _logService;
      private string flag = "";
		
		public LogFilterService(ICurrentUser currentUser,
										IDomainObjectFactory domainObjectFactory,
										IEventAggregator eventAggregator,
										ILogService logService,
										ILogFilterRepository logFilterRepository)
			: base(currentUser, domainObjectFactory, eventAggregator)
		{
			_logFilterRepository = logFilterRepository;
			_logService = logService;
		}

		public int CreateLogFilter(int userId, string userName, string building, string node, string name, int? companyId, DateTime? fromDate, DateTime? toDate, string activity, string host, bool isShowDefaultLog)
		{
			int result = 0;

			if( _logFilterRepository.FindAll().Any(lf=>!lf.IsDeleted && lf.Name.ToLower() == name.ToLower()))
			{
				return -1;
			}
			using( IUnitOfWork work = UnitOfWork.Begin() )
			{
				var logFilter = DomainObjectFactory.CreateLogFilter();
				logFilter.UserId = userId;
				logFilter.UserName = userName;
				logFilter.Building = building;
				logFilter.Node = node;
				logFilter.Name = name;
				logFilter.CompanyId = companyId;
				logFilter.FromDate = fromDate;
				logFilter.ToDate = toDate;
				logFilter.Activity = activity;
				logFilter.IsShowDefaultLog = isShowDefaultLog;

				_logFilterRepository.Add(logFilter);
				
				work.Commit();

				logFilter = _logFilterRepository.FindById(logFilter.Id);

				var logFilterEvent = new LogFilterEventEntity(logFilter);

				string message = logFilterEvent.GetCreateMessage();

				_logService.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, message);

				result = logFilter.Id;
			}

			return result;
		}

		public int EditLogFilter(int id, string userName, string building, string node, string name, int? companyId, DateTime? fromDate, DateTime? toDate, string activity, string host, bool isShowDefaultLog)
		{
			if (_logFilterRepository.FindAll().Any(lf => !lf.IsDeleted && lf.Name.ToLower() == name.ToLower() && lf.Id != id))
			{
				return -1;
			}

			using (IUnitOfWork work = UnitOfWork.Begin())
			{
				var logFilter = _logFilterRepository.FindById(id);

				var logFilterEntity = new LogFilterEventEntity(logFilter);
				logFilter.UserName = userName;
				logFilter.Building = building;
				logFilter.Node = node;
				logFilter.Name = name;
				logFilter.CompanyId = companyId;
				logFilter.FromDate = fromDate;
				logFilter.ToDate = toDate;
				logFilter.Activity = activity;
				logFilter.IsShowDefaultLog = isShowDefaultLog;

				work.Commit();

				logFilterEntity.SetNewFilter(_logFilterRepository.FindById(id));

				var message = logFilterEntity.GetEditMessage();

				_logService.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, message);

				return id;
			}
		}

		public void DeleteLogFilter(int id, string host)
		{
			using (IUnitOfWork work = UnitOfWork.Begin())
			{
				var log_filter = _logFilterRepository.FindById(id);

				var logFilterEvent = new LogFilterEventEntity(log_filter);

				string message = logFilterEvent.GetDeleteMessage();

				log_filter.IsDeleted = true;

				work.Commit();

				_logService.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, message);
			}
		}
	}
}