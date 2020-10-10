using FoxSec.Authentication;
using FoxSec.Common.EventAggregator;
using FoxSec.Core.Infrastructure.UnitOfWork;
using FoxSec.Core.SystemEvents;
using FoxSec.DomainModel;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.ServiceLayer.Contracts;

namespace FoxSec.ServiceLayer.Services
{
	internal class TitleService : ServiceBase, ITitleService
	{
		private readonly ITitleRepository _titleRepository;
	    private readonly ILogService _logService;
        string flag = "";
		public TitleService(ICurrentUser currentUser, IDomainObjectFactory domainObjectFactory, IEventAggregator eventAggregator, ITitleRepository titleRepository, ILogService logService)
			: base(currentUser, domainObjectFactory, eventAggregator)
		{
		    _logService = logService;
            _titleRepository = titleRepository;
		}

        public void CreateTitle(string name, string description, int companyId)
		{
			using( IUnitOfWork work = UnitOfWork.Begin() )
			{
				Title title = DomainObjectFactory.CreateTitle();

                title.Name = name;
                title.Description = description;
                title.CompanyId = companyId;
			    title.IsDeleted = false;

				_titleRepository.Add(title);

				work.Commit();

			    title = _titleRepository.FindById(title.Id);

			    var logTitleEntity = new TitleEventEntity(title);

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                                      logTitleEntity.GetCreateMessage());
			}
		}

        public void DeleteTitle(int id)
		{
			using( IUnitOfWork work = UnitOfWork.Begin() )
			{
                Title title = _titleRepository.FindById(id);

                title.IsDeleted = true;
                var logTitleEntity = new TitleEventEntity(title);

                work.Commit();

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                                      logTitleEntity.GetDeleteMessage());
			}
		}

        public void EditTitle(int id, string name, string description, int companyId)
		{
			using( IUnitOfWork work = UnitOfWork.Begin() )
			{
                Title title = _titleRepository.FindById(id);
                var logTitleEntity = new TitleEventEntity(title);

                title.Name = name;
                title.Description = description;
			    title.CompanyId = companyId;
                
				work.Commit();

                logTitleEntity.SetNewTitle(_titleRepository.FindById(id));
                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                                      logTitleEntity.GetEditMessage());
			}
		}
	}
}
