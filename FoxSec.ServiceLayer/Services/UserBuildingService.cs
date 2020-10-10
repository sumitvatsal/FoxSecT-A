using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FoxSec.Authentication;
using FoxSec.Common.EventAggregator;
using FoxSec.Core.Infrastructure.UnitOfWork;
using FoxSec.Core.SystemEvents;
using FoxSec.Core.SystemEvents.DTOs;
using FoxSec.DomainModel;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.ServiceLayer.Contracts;

namespace FoxSec.ServiceLayer.Services
{
	internal class UserBuildingService : ServiceBase, IUserBuildingService
	{
		private readonly IUserBuildingRepository _userBuildingRepository;
		private readonly IUserRepository _userRepository;
		private readonly ILogService _logService;
        private string flag = "";
		public UserBuildingService(ICurrentUser currentUser,
												IDomainObjectFactory domainObjectFactory,
												IEventAggregator eventAggregator,
												IUserRepository userRepository,
												ILogService logService,
												IUserBuildingRepository userBuildingRepository)
			: base(currentUser, domainObjectFactory, eventAggregator)
		{
			_userBuildingRepository = userBuildingRepository;
			_userRepository = userRepository;
			_logService = logService;
		}

		public int CreateUserBuilding(int userId, int buildingId, int? buildingObjectId, string host)
		{
			int result = 0;

			var user = _userRepository.FindById(userId);

			using( IUnitOfWork work = UnitOfWork.Begin() )
			{
				UserBuilding ub = null;
				ub =
					_userBuildingRepository.FindAll().Where(
						x => x.UserId == userId && x.BuildingId == buildingId && x.BuildingObjectId == buildingObjectId).FirstOrDefault();
				var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserBuildingsChanged", new List<string> { user.LoginName }));
				if (ub == null)
				{
					ub = DomainObjectFactory.CreateUserBuilding();

					ub.UserId = userId;
					ub.BuildingObjectId = buildingObjectId;
					ub.IsDeleted = false;
					ub.BuildingId = buildingId;

					_userBuildingRepository.Add(ub);
				}
				else
				{
					ub.IsDeleted = false;
				}

				work.Commit();
				ub = _userBuildingRepository.FindById(ub.Id);
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserBuildingAdded", new List<string> { ub.Building.Name,
												 ub.BuildingObject==null ? " " : ub.BuildingObject.Description }));
				
				result = ub.Id;

				_logService.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, message.ToString());
			}

			return result;
		}

		public void DeleteUserBuildings(int userId, string host)
		{
			var user = _userRepository.FindById(userId);
			using( IUnitOfWork work = UnitOfWork.Begin() )
			{
				IEnumerable<UserBuilding> objects = _userBuildingRepository.FindAll(x => x.UserId == userId && !x.IsDeleted);
				var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserBuildingsChanged", new List<string> { user.LoginName }));
				foreach( var item in objects )
				{
					item.IsDeleted = true;

					message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserBuildingRemoved", new List<string> { item.Building.Name,
												 item.BuildingObject == null ? " " : item.BuildingObject.Description }));
				}

				work.Commit();

				_logService.CreateLog(CurrentUser.Get().Id, "web", flag,host, CurrentUser.Get().CompanyId, message.ToString());
			}
		}
	}
}