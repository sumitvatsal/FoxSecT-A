using System;
using System.Collections.Generic;
using System.Text;
using FoxSec.Authentication;
using FoxSec.Common.EventAggregator;
using FoxSec.Core.Infrastructure.UnitOfWork;
using FoxSec.DomainModel;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.ServiceLayer.Contracts;

namespace FoxSec.ServiceLayer.Services
{
    internal class CompanyBuildingObjectService : ServiceBase, ICompanyBuildingObjectService
    {
        ICompanyBuildingObjectRepository _companyBuildingObjectRepository;
    	private readonly ILogService _logService;
    	private readonly ICompanyRepository _companyRepository;
        private string flag = "";
        public CompanyBuildingObjectService(    ICurrentUser currentUser,
                                                IDomainObjectFactory domainObjectFactory,
                                                IEventAggregator eventAggregator,
												ILogService logservice,
												ICompanyRepository companyRepository,
                                                ICompanyBuildingObjectRepository companyBuildingObjectRepository    ) : base(currentUser, domainObjectFactory, eventAggregator)
    	{
            _companyBuildingObjectRepository = companyBuildingObjectRepository;
        	_logService = logservice;
        	_companyRepository = companyRepository;
		}

        public int CreateCompanyBuildingObject(int companyId, int buildingObjectId, string host)
        {
            int result = 0;

            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                CompanyBuildingObject cbo = DomainObjectFactory.CreateCompanyBuildingObject();

                cbo.CompanyId = companyId;
                cbo.BuildingObjectId = buildingObjectId;
                cbo.ValidFrom = DateTime.Now;
                cbo.ValidTo = DateTime.Now.AddYears(2);
                cbo.IsDeleted = false;

                _companyBuildingObjectRepository.Add(cbo);

                work.Commit();

                result = cbo.Id;

            	cbo = _companyBuildingObjectRepository.FindById(result);

            	var message = new StringBuilder();
            	message.Append(string.Format("Building objects for Company '{0}' changed. ", cbo.Company.Name));
            	message.Append(string.Format("Room '{0}' in '{1}' added. ", cbo.BuildingObject.Description,
            	                             cbo.BuildingObject.Building.Name));

            	_logService.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, message.ToString());
            }

            return result;
        }

        public void DeleteCompanyBuildingObjects(int companyId, string host)
        {
        	var cc = _companyRepository.FindById(companyId);
        	var message = new StringBuilder();
			message.Append(string.Format("Building objects for Company '{0}' changed. ", cc.Name));
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                IEnumerable<CompanyBuildingObject> objects = _companyBuildingObjectRepository.FindAll(x => x.CompanyId == companyId && !x.IsDeleted && x.BuildingObject.TypeId == 1);

                foreach(var item in objects)
                {
                    item.IsDeleted = true;
					message.Append(string.Format("Room '{0}' in '{1}' deleted. ", item.BuildingObject.Description,
											 item.BuildingObject.Building.Name));
                }

                work.Commit();

				_logService.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, message.ToString());
            }
        }
    }
}