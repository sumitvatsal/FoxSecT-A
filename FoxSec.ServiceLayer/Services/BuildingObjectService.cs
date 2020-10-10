using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using FoxSec.Authentication;
using FoxSec.Common.EventAggregator;
using FoxSec.Core.Infrastructure.UnitOfWork;
using FoxSec.Common.Enums;
using FoxSec.Core.SystemEvents;
using FoxSec.Core.SystemEvents.DTOs;
using FoxSec.DomainModel;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.ServiceLayer.Contracts;

namespace FoxSec.ServiceLayer.Services
{
	internal class BuildingObjectService : ServiceBase, IBuildingObjectService
	{
        string flag = "";
		private readonly IBuildingObjectRepository _buildingObjectRepository;
		private readonly ILogService _logService;

		public BuildingObjectService(ICurrentUser currentUser,
										IDomainObjectFactory domainObjectFactory,
										IEventAggregator eventAggregator,
										IBuildingObjectRepository buildingObjectRepository,
										ILogService logService)
			: base(currentUser, domainObjectFactory, eventAggregator)
		{
			_buildingObjectRepository = buildingObjectRepository;
			_logService = logService;
		}

        public int CreateOrFindBuildingFloorId(int buildingId, int floorNr, string description, int? objectNr=null)
		{
			int result = 0;

			using (IUnitOfWork work = UnitOfWork.Begin())
			{
                var floor = _buildingObjectRepository.FindAll(x => !x.IsDeleted && x.BuildingId == buildingId && x.TypeId == (int)BuildingObjectTypes.Floor && x.FloorNr == floorNr);

                if (floor.Count() == 0)
                {
                    BuildingObject bObject = DomainObjectFactory.CreateBuildingObject();

                    bObject.TypeId = (int)BuildingObjectTypes.Floor;
                    bObject.BuildingId = buildingId;
                    bObject.FloorNr = floorNr;
                    bObject.Description = description;
                    bObject.IsDeleted = false;
                	bObject.ObjectNr = objectNr;

                    _buildingObjectRepository.Add(bObject);
                    work.Commit();

                    result = bObject.Id;
                }
                else
                {
                    result = floor.FirstOrDefault().Id;
                }
			}

			return result;
		}

        public int EditBuilding(int id, int? Global, string host) 
        {
            int result = 0;
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                var building_object = _buildingObjectRepository.FindById(id);
                var old_global = building_object.GlobalBuilding;
                building_object.GlobalBuilding = Global;
                var flag = "";
               // building_object.Comment = comment;

                work.Commit();
                
                if (old_global != building_object.GlobalBuilding)
                {
                    var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogBuildingObjectGlobalChange", new List<string> {"Building ", building_object.BuildingId.ToString()," from ", old_global.ToString(), " to ", Global.ToString() }));
                    // message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCommentChange", new List<string> { string.IsNullOrWhiteSpace(old_comment) ? " " : old_comment, string.IsNullOrWhiteSpace(comment) ? " " : comment }));
                    
                    _logService.CreateLog(CurrentUser.Get().Id, "web",flag, host, CurrentUser.Get().CompanyId, message.ToString());
                }
            }
            return result;
        }
		public void SetComment(int id, string comment, string host)
		{
			using (IUnitOfWork work = UnitOfWork.Begin())
			{
				var building_object = _buildingObjectRepository.FindById(id);

				var old_comment = building_object.Comment;
				building_object.Comment = comment;

				work.Commit();

				if( String.IsNullOrEmpty(old_comment) || string.IsNullOrEmpty(old_comment) || old_comment.ToLower().Trim() != comment.ToLower().Trim() )
				{
					var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
					message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageBuildingObjectChange", new List<string> { building_object.BuildingObjectType.Description, building_object.Description }));
					message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCommentChange", new List<string> { string.IsNullOrWhiteSpace(old_comment) ? " " : old_comment, string.IsNullOrWhiteSpace(comment) ? " " : comment }));

					_logService.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, message.ToString());
				}
			}
		}
	}
}