using System.Linq;
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
    internal class UserAccessUnitTypeService : ServiceBase, IUserAccessUnitTypeService
    {
        private readonly IUserAccessUnitTypeRepository _cardTypeRepository;
        private readonly ILogService _logService;
        string flag = "";
        public UserAccessUnitTypeService( ICurrentUser currentUser,
                                IDomainObjectFactory domainObjectFactory,
                                IEventAggregator eventAggregator,
                                ILogService logService,
                                IUserAccessUnitTypeRepository cardTypeRepository) : base(currentUser, domainObjectFactory, eventAggregator)
		{
            _cardTypeRepository = cardTypeRepository;
            _logService = logService;
		}

        public int CreateCardType(string name, bool isCardCode, bool isSerDK, string description)
        {
            int result = 0;

            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                UserAccessUnitType cType = DomainObjectFactory.CreateUserAccessUnitType();

                cType.Name = name;
                cType.IsCardCode = isCardCode;
                cType.IsSerDK = isSerDK;
                cType.Description = description;
                cType.IsDeleted = false;

                _cardTypeRepository.Add(cType);

                var logCardTypeEntity = new CardTypeEventEntity(cType);

                work.Commit();

                result = cType.Id;

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName,
                                      CurrentUser.Get().CompanyId, logCardTypeEntity.GetCreateMessage());
            }

            return result;
        }

        public void DeleteCardType(int id)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                UserAccessUnitType cType = _cardTypeRepository.FindAll(x => x.Id == id && !x.IsDeleted).FirstOrDefault();

                cType.IsDeleted = true;

                var logCardTypeEntity = new CardTypeEventEntity(cType);

                work.Commit();

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName,
                                      CurrentUser.Get().CompanyId, logCardTypeEntity.GetDeleteMessage());
            }
        }

        public void EditCardType(int id, string name, bool isCardCode, bool isSerDK, string description)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                UserAccessUnitType cType = _cardTypeRepository.FindById(id);
                var logCardTypeEntity = new CardTypeEventEntity(cType);

                cType.Name = name;
                cType.IsCardCode = isCardCode;
                cType.IsSerDK = isSerDK;
                cType.Description = description;

                logCardTypeEntity.SetNewCardType(cType);

                work.Commit();

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag,CurrentUser.Get().HostName,
                                      CurrentUser.Get().CompanyId, logCardTypeEntity.GetEditMessage());
            }
        }
    }
}