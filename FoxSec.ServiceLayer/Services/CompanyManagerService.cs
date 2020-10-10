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
    internal class CompanyManagerService : ServiceBase, ICompanyManagerService
    {
        private readonly ICompanyManagerRepository _companyManagerRepository;
    	private readonly ILogService _logService;
        private string flag = "";
        public CompanyManagerService(ICurrentUser currentUser, IDomainObjectFactory domainObjectFactory,
                                     IEventAggregator eventAggregator,
									 ILogService logService,
                                     ICompanyManagerRepository companyManagerRepository
            )
            : base(currentUser, domainObjectFactory, eventAggregator)
        {
            _companyManagerRepository = companyManagerRepository;
        	_logService = logService;
        }

        public void SaveCompanyManager(int companyId, int userId, string host)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {

                CompanyManager companyManager =
                    _companyManagerRepository.FindAll(cm => cm.CompanyId == companyId).FirstOrDefault();
                
                if(companyManager == null)
                {
                    companyManager = DomainObjectFactory.CreateCompanyManager();
                    _companyManagerRepository.Add(companyManager);
                }

                companyManager.CompanyId = companyId;
                companyManager.UserId = userId;
                companyManager.IsDeleted = false;

                work.Commit();

            	companyManager = _companyManagerRepository.FindById(companyManager.Id);

            	var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
				message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageSetCompanyManager", new List<string> { companyManager.User.LoginName, companyManager.User.LastName }));

            	_logService.CreateLog(CurrentUser.Get().Id, "web",flag, host, CurrentUser.Get().CompanyId, message.ToString());
            }
        }
    }
}
