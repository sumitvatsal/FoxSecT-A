using System;
using System.Collections.Generic;
using System.Text;
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
	internal class UserBuildingObjectService : ServiceBase, IUserBuildingObjectService
	{
		private readonly IUserBuildingObjectRepository _userBuildingObjectRepository;
		private readonly IUserRepository _userRepository;
		private readonly ILogService _logService;

		public UserBuildingObjectService(ICurrentUser currentUser,
												IDomainObjectFactory domainObjectFactory,
												IEventAggregator eventAggregator,
												IUserRepository userRepository,
												ILogService logService,
												IUserBuildingObjectRepository UserBuildingObjectRepository)
			: base(currentUser, domainObjectFactory, eventAggregator)
		{
			_userBuildingObjectRepository = UserBuildingObjectRepository;
			_userRepository = userRepository;
			_logService = logService;
		}

		public int CreateUserBuildingObject(int userId, int buildingObjectId, string host)
		{
			int result = 0;

			var user = _userRepository.FindById(userId);

			using( IUnitOfWork work = UnitOfWork.Begin() )
			{
				UserBuildingObject ubo = DomainObjectFactory.CreateUserBuildingObject();

				var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserBuildingsChanged", new List<string> { user.LoginName }));
				
				ubo.UserId = userId;
				ubo.BuildingObjectId = buildingObjectId;
				ubo.IsDeleted = false;

				_userBuildingObjectRepository.Add(ubo);

				work.Commit();
				ubo = _userBuildingObjectRepository.FindById(ubo.Id);
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserBuildingAdded", new List<string> { ubo.BuildingObject.Building.Name,
												 ubo.BuildingObject.Description }));
				
				result = ubo.Id;

				_logService.CreateLog(CurrentUser.Get().Id, "web", host, CurrentUser.Get().CompanyId, message.ToString());
			}

			return result;
		}

		public void DeleteUserBuildingObjects(int userId, string host)
		{
			var user = _userRepository.FindById(userId);
			using( IUnitOfWork work = UnitOfWork.Begin() )
			{
				IEnumerable<UserBuildingObject> objects = _userBuildingObjectRepository.FindAll(x => x.UserId == userId && !x.IsDeleted);
				var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserBuildingsChanged", new List<string> { user.LoginName }));
				foreach( var item in objects )
				{
					item.IsDeleted = true;

					message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserBuildingRemoved", new List<string> { item.BuildingObject.Building.Name,
												 item.BuildingObject.Description }));
				}

				work.Commit();

				_logService.CreateLog(CurrentUser.Get().Id, "web", host, CurrentUser.Get().CompanyId, message.ToString());
			}
		}
	}
}