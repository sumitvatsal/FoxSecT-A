using System;
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
    internal class UsersAccessUnitService : ServiceBase, IUsersAccessUnitService
    {
        private readonly IUsersAccessUnitRepository _usersAccessUnitRepository;
        private readonly IClassificatorValueRepository _classificatorValueRepository;
        private readonly IControllerUpdateService _controllerUpdateService;
        private readonly ILogService _logService;
        string flag = "";
        public UsersAccessUnitService(ICurrentUser currentUser,
                                        IDomainObjectFactory domainObjectFactory,
                                        IEventAggregator eventAggregator,
                                        IUsersAccessUnitRepository usersAccessUnitRepository,
                                        IClassificatorValueRepository classificatorValueRepository,
                                        IControllerUpdateService controllerUpdateService,
                                        ILogService logService) : base(currentUser, domainObjectFactory, eventAggregator)
        {
            _usersAccessUnitRepository = usersAccessUnitRepository;
            _logService = logService;
            _classificatorValueRepository = classificatorValueRepository;
            _controllerUpdateService = controllerUpdateService;
        }
        public bool CardIsBack(int id)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                try
                {

                    UsersAccessUnit usersAccessUnit = _usersAccessUnitRepository.FindById(id);
                    usersAccessUnit.Closed = DateTime.Now;
                    usersAccessUnit.ValidTo = DateTime.Now.AddDays(-1);
                    usersAccessUnit.Comment = "";
                    work.Commit();
                    var msg = string.Format("Card is Back ! User:{0}. Card:{1}", usersAccessUnit.User.LoginName.ToString(), usersAccessUnit.CardFullCode);
                    _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName,
                      CurrentUser.Get().CompanyId, msg);
                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, id, UpdateParameter.UserCardChange, ControllerStatus.Edited, msg);

                }
                catch (Exception)
                {
                    return false;
                }


                return true;
            }
        }
        public void CreateCard(int? userId, int? typeId, int? companyId, int buildingId, string serial, string dk, string code, bool isFree, DateTime? from, DateTime? to, bool? IsMainUnit)
        {
            //var result = -1;
            var is_new = true;
            var card_message = string.Empty;
            int? cardId = null;

            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                int? id = IsCardInSystem(serial, dk, code);

                var message = string.Empty;

                if (!id.HasValue)
                {
                    UsersAccessUnit usersAccessUnit = DomainObjectFactory.CreateUsersAccessUnit();
                    IFoxSecIdentity identity = CurrentUser.Get();

                    usersAccessUnit.UserId = userId;
                    usersAccessUnit.CompanyId = companyId;
                    usersAccessUnit.TypeId = typeId;
                    usersAccessUnit.Serial = serial;
                    usersAccessUnit.Code = code;
                    usersAccessUnit.Free = isFree;
                    usersAccessUnit.Active = !isFree;
                    usersAccessUnit.Dk = dk;
                    usersAccessUnit.IsDeleted = false;
                    usersAccessUnit.CreatedBy = identity.Id;
                    usersAccessUnit.ValidFrom = from;
                    usersAccessUnit.ValidTo = to;
                    usersAccessUnit.BuildingId = Convert.ToInt32(buildingId);
                    usersAccessUnit.IsMainUnit = IsMainUnit;
                   
                    _usersAccessUnitRepository.Add(usersAccessUnit);

                    work.Commit();
                    var logCardEntity =
                        new UserAccessUnitEventEntity(_usersAccessUnitRepository.FindById(usersAccessUnit.Id));
                    cardId = usersAccessUnit.Id;

                    card_message = string.Format(" Card '{0}'. Valid from '{1}' to '{2}'", string.IsNullOrEmpty(usersAccessUnit.Code)
                                                                                    ? string.Format("{0} {1}",
                                                                                                    usersAccessUnit.Serial,
                                                                                                    usersAccessUnit.Dk)
                                                                                                    : usersAccessUnit.Code,
                                                                                                    usersAccessUnit.ValidFrom.HasValue ? usersAccessUnit.ValidFrom.Value.ToString("dd.MM.yyyy") : "not setted",
                                                                                                    usersAccessUnit.ValidTo.HasValue ? usersAccessUnit.ValidTo.Value.ToString("dd.MM.yyyy") : "not setted");

                    message = logCardEntity.GetCreateMessage();

                    _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName,
                                          CurrentUser.Get().CompanyId, message);

                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, cardId.Value, UpdateParameter.UserCardChange, is_new ? ControllerStatus.Created : ControllerStatus.Edited, card_message);

                }
                else
                {
                    is_new = false;
                    cardId = id;

                    UsersAccessUnit usersAccessUnit = _usersAccessUnitRepository.FindById(id.Value);
                    var logCardEntity = new UserAccessUnitEventEntity(usersAccessUnit);

                    usersAccessUnit.UserId = userId;
                    usersAccessUnit.CompanyId = companyId;
                    usersAccessUnit.TypeId = typeId;
                    usersAccessUnit.Serial = serial;
                    usersAccessUnit.Code = code;
                    usersAccessUnit.Free = isFree;
                    if (!usersAccessUnit.Active)
                    {
                        usersAccessUnit.ClassificatorValueId = null;
                    }
                    usersAccessUnit.Active = true;
                    usersAccessUnit.Dk = dk;
                    usersAccessUnit.IsDeleted = false;
                    usersAccessUnit.ValidFrom = from;
                    usersAccessUnit.ValidTo = to;
                    usersAccessUnit.BuildingId = buildingId;


                    work.Commit();

                    card_message = string.Format(" Card '{0}'. Valid from '{1}' to '{2}'", string.IsNullOrEmpty(usersAccessUnit.Code)
                                                                                    ? string.Format("{0} {1}",
                                                                                                    usersAccessUnit.Serial,
                                                                                                    usersAccessUnit.Dk)
                                                                                                    : usersAccessUnit.Code,
                                                                                                    usersAccessUnit.ValidFrom.HasValue ? usersAccessUnit.ValidFrom.Value.ToString("dd.MM.yyyy") : "not setted",
                                                                                                    usersAccessUnit.ValidTo.HasValue ? usersAccessUnit.ValidTo.Value.ToString("dd.MM.yyyy") : "not setted");

                    logCardEntity.SetNewUserAccessUnit(_usersAccessUnitRepository.FindById(usersAccessUnit.Id));

                    message = logCardEntity.GetEditMessage();

                    _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName,
                                          CurrentUser.Get().CompanyId, message);

                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, cardId.Value, UpdateParameter.UserCardChange, is_new ? ControllerStatus.Created : ControllerStatus.Edited, card_message);
                }
                //result = 1;

            }
        }

        private int? IsCardInSystem(string serial, string dk, string code)
        {
            int? result = null;

            if (serial != null && serial != string.Empty && dk != null && dk != string.Empty)
            {
                var cards = _usersAccessUnitRepository.FindAll(x => !x.IsDeleted && x.Serial != null && x.Serial != string.Empty && x.Serial.Trim() == serial.Trim()
                                                    && x.Dk != null && x.Dk != string.Empty && x.Dk.Trim() == dk.Trim());

                if (cards.Count() > 0) result = cards.FirstOrDefault().Id;
            }
            else
            if (code != null && code != string.Empty)
            {
                var cards = _usersAccessUnitRepository.FindAll(x => !x.IsDeleted && x.Code != null && x.Code != string.Empty && x.Code.Trim() == code.Trim());
                if (cards.Count() > 0) result = cards.FirstOrDefault().Id;
            }

            return result;
        }

        public bool EditCard(int id, int? userId, int? typeId, int? companyId, int? buildingId, string serial, string dk, string code, bool isFree, DateTime? from, DateTime? to, string Comment, bool? IsMainUnit, bool? isActive = null)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                try
                {

                    UsersAccessUnit usersAccessUnit = _usersAccessUnitRepository.FindById(id);
                    int? idc = IsCardInSystem(serial, dk, code);

                    var message = string.Empty;

                    

                    if (!idc.HasValue || idc == id)
                    {

                        var logCardEntity =
                                new UserAccessUnitEventEntity(usersAccessUnit);
                        ////127 changes
                        usersAccessUnit.Opened = DateTime.Now;
                        if (to >= DateTime.Now.Date)
                        {
                            usersAccessUnit.Closed = null;
                        }
                        else
                        { usersAccessUnit.Closed = DateTime.Now; }
                        usersAccessUnit.UserId = userId;
                        usersAccessUnit.CompanyId = companyId;
                        usersAccessUnit.TypeId = typeId;
                        usersAccessUnit.Serial = serial;
                        usersAccessUnit.Code = code;
                        usersAccessUnit.Free = isFree;
                        usersAccessUnit.Dk = dk;
                        usersAccessUnit.IsDeleted = false;
                        usersAccessUnit.ValidFrom = from;
                        usersAccessUnit.ValidTo = to;
                        usersAccessUnit.Comment = Comment;
                        usersAccessUnit.IsMainUnit = IsMainUnit;
                        if (buildingId.HasValue)
                        {
                            if (buildingId != 0)
                            {
                                usersAccessUnit.BuildingId = buildingId.Value;
                            }
                        }
                        if (isActive.HasValue)
                        {
                            if (usersAccessUnit.Active == false && isActive.Value == true)
                            {
                                usersAccessUnit.ClassificatorValueId = null;
                            }
                            usersAccessUnit.Active = isActive.Value;
                        }

                        work.Commit();

                        logCardEntity.SetNewUserAccessUnit(_usersAccessUnitRepository.FindById(usersAccessUnit.Id));

                        _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName,
                                              CurrentUser.Get().CompanyId, logCardEntity.GetEditMessage());

                        var card_message = string.Format(" Card '{0}'. Valid from '{1}' to '{2}'", string.IsNullOrEmpty(usersAccessUnit.Code)
                                                                                            ? string.Format("{0} {1}",
                                                                                                            usersAccessUnit.Serial,
                                                                                                            usersAccessUnit.Dk)
                                                                                                            : usersAccessUnit.Code,
                                                                                                            usersAccessUnit.ValidFrom.HasValue ? usersAccessUnit.ValidFrom.Value.ToString("dd.MM.yyyy") : "not setted",
                                                                                                            usersAccessUnit.ValidTo.HasValue ? usersAccessUnit.ValidTo.Value.ToString("dd.MM.yyyy") : "not setted");

                        _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, usersAccessUnit.Id, UpdateParameter.UserCardChange, ControllerStatus.Edited, card_message);

                    }
                    else { return false; }
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        public bool EditCommentCard(int cardId,DateTime validTo, DateTime validFrom,string comment)
        {
            try
            {
                using (IUnitOfWork work = UnitOfWork.Begin())
                {
                    UsersAccessUnit usersAccessUnit = _usersAccessUnitRepository.FindById(cardId);
                    usersAccessUnit.ValidFrom = validFrom;
                    usersAccessUnit.ValidTo = validTo;
                    usersAccessUnit.Comment = comment;
                    work.Commit();
                    return true;
                }
            }
            catch
            {
                return false;
            }
           
        }

        private void SetActiveState(int cardId, int? classificatorValueId, bool state)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                UsersAccessUnit usersAccessUnit = _usersAccessUnitRepository.FindById(cardId);
                usersAccessUnit.ClassificatorValueId = classificatorValueId;
                usersAccessUnit.Classificator_dt = DateTime.Now;
                var reason_str = classificatorValueId.HasValue
                                    ? _classificatorValueRepository.FindById(classificatorValueId.Value).Value
                                    : "Empty";

                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCardChanged", new List<string> { string.IsNullOrWhiteSpace(usersAccessUnit.Code) ? string.Format("{0} {1}", usersAccessUnit.Serial, usersAccessUnit.Dk) : usersAccessUnit.Code }));
                message.Add(state
                                ? XMLLogMessageHelper.TemplateToXml("LogMessageCardActivated", new List<string> { reason_str })
                                : XMLLogMessageHelper.TemplateToXml("LogMessageCardDeactivated", new List<string> { reason_str }));
                usersAccessUnit.Active = state;

                work.Commit();

                var card_message = string.Format(" Card '{0}' active is '{1}'", string.IsNullOrEmpty(usersAccessUnit.Code)
                                                                                    ? string.Format("{0} {1}",
                                                                                                    usersAccessUnit.Serial,
                                                                                                    usersAccessUnit.Dk)
                                                                                    : usersAccessUnit.Code, state);

                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, cardId, UpdateParameter.CardStatusChange, ControllerStatus.Edited, card_message);

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId, message.ToString());
            }
        }

        public void Deactivate(int cardId, int? classificatorValueId)
        {
            SetActiveState(cardId, classificatorValueId, false);

        }

        public void Activate(int cardId, int? classificatorValueId)
        {
            SetActiveState(cardId, classificatorValueId, true);
        }

        public void Delete(int cardId)
        {
            var card_message = string.Empty;
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                UsersAccessUnit usersAccessUnit = _usersAccessUnitRepository.FindById(cardId);
                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCardDeleted", new List<string> { string.IsNullOrWhiteSpace(usersAccessUnit.Code) ? string.Format("{0} {1}", usersAccessUnit.Serial, usersAccessUnit.Dk) : usersAccessUnit.Code }));

                usersAccessUnit.IsDeleted = true;
                work.Commit();

                card_message = string.Format(" Card '{0}'. Valid from '{1}' to '{2}'", string.IsNullOrEmpty(usersAccessUnit.Code)
                                                                                    ? string.Format("{0} {1}",
                                                                                                    usersAccessUnit.Serial,
                                                                                                    usersAccessUnit.Dk)
                                                                                                    : usersAccessUnit.Code,
                                                                                                    usersAccessUnit.ValidFrom.HasValue ? usersAccessUnit.ValidFrom.Value.ToString("dd.MM.yyyy") : "not setted",
                                                                                                    usersAccessUnit.ValidTo.HasValue ? usersAccessUnit.ValidTo.Value.ToString("dd.MM.yyyy") : "not setted");

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName,
                                      CurrentUser.Get().CompanyId, message.ToString());
            }

            _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, cardId, UpdateParameter.UserCardChange, ControllerStatus.Deleted, card_message);
        }

        public void SetFreeState(int cardId, int? classificatorValueId)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                UsersAccessUnit usersAccessUnit = _usersAccessUnitRepository.FindById(cardId);
                usersAccessUnit.Free = true;
                usersAccessUnit.Active = false;
                //usersAccessUnit.UserId = null;
                usersAccessUnit.Closed = DateTime.Now;
                usersAccessUnit.ClassificatorValueId = classificatorValueId;
                var reason_str = classificatorValueId.HasValue
                                     ? _classificatorValueRepository.FindById(classificatorValueId.Value).Value
                                     : "Empty";


                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCardChanged", new List<string> { string.IsNullOrWhiteSpace(usersAccessUnit.Code) ? string.Format("{0} {1}", usersAccessUnit.Serial, usersAccessUnit.Dk) : usersAccessUnit.Code }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCardSetToFree", new List<string> { reason_str }));

                work.Commit();

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName,
                                      CurrentUser.Get().CompanyId, message.ToString());

                var card_message = string.Format(" Card '{0}' is free", string.IsNullOrEmpty(usersAccessUnit.Code)
                                                                                    ? string.Format("{0} {1}",
                                                                                                    usersAccessUnit.Serial,
                                                                                                    usersAccessUnit.Dk)
                                                                                    : usersAccessUnit.Code);

                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, cardId, UpdateParameter.CardStatusChange, ControllerStatus.Edited, card_message);
            }
        }

        public void SetValidFrom(int cardId, DateTime date)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                UsersAccessUnit usersAccessUnit = _usersAccessUnitRepository.FindById(cardId);
                var old_date = usersAccessUnit.ValidFrom;
                usersAccessUnit.ValidFrom = date;
                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCardChanged", new List<string> { string.IsNullOrWhiteSpace(usersAccessUnit.Code) ? string.Format("{0} {1}", usersAccessUnit.Serial, usersAccessUnit.Dk) : usersAccessUnit.Code }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageValidFromChanged", new List<string> { old_date == null ? "empty" : old_date.Value.ToString("dd.MM.yyyy"),
                    date.ToString("dd.MM.yyyy") }));


                work.Commit();

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName,
                                      CurrentUser.Get().CompanyId, message.ToString());

                var card_message = string.Format(" Card '{0}'. Valid from '{1}' to '{2}'", string.IsNullOrEmpty(usersAccessUnit.Code)
                                                                                    ? string.Format("{0} {1}",
                                                                                                    usersAccessUnit.Serial,
                                                                                                    usersAccessUnit.Dk)
                                                                                                    : usersAccessUnit.Code,
                                                                                                    usersAccessUnit.ValidFrom.HasValue ? usersAccessUnit.ValidFrom.Value.ToString("dd.MM.yyyy") : "not setted",
                                                                                                    usersAccessUnit.ValidTo.HasValue ? usersAccessUnit.ValidTo.Value.ToString("dd.MM.yyyy") : "not setted");

                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, cardId, UpdateParameter.UserCardChange, ControllerStatus.Edited, card_message);
            }
        }

        public void SetValidTo(int cardId, DateTime date)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                UsersAccessUnit usersAccessUnit = _usersAccessUnitRepository.FindById(cardId);
                usersAccessUnit.ValidTo = date;
                var old_date = usersAccessUnit.ValidTo;
                work.Commit();

                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCardChanged", new List<string> { string.IsNullOrWhiteSpace(usersAccessUnit.Code) ? string.Format("{0} {1}", usersAccessUnit.Serial, usersAccessUnit.Dk) : usersAccessUnit.Code }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageValidToChanged", new List<string> { old_date == null ? "empty" : old_date.Value.ToString("dd.MM.yyyy"),
                    date.ToString("dd.MM.yyyy") }));
                work.Commit();

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName,
                                      CurrentUser.Get().CompanyId, message.ToString());

                var card_message = string.Format(" Card '{0}'. Valid from '{1}' to '{2}'", string.IsNullOrEmpty(usersAccessUnit.Code)
                                                                                    ? string.Format("{0} {1}",
                                                                                                    usersAccessUnit.Serial,
                                                                                                    usersAccessUnit.Dk)
                                                                                                    : usersAccessUnit.Code,
                                                                                                    usersAccessUnit.ValidFrom.HasValue ? usersAccessUnit.ValidFrom.Value.ToString("dd.MM.yyyy") : "not setted",
                                                                                                    usersAccessUnit.ValidTo.HasValue ? usersAccessUnit.ValidTo.Value.ToString("dd.MM.yyyy") : "not setted");

                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, cardId, UpdateParameter.UserCardChange, ControllerStatus.Edited, card_message);
            }
        }

        public bool GiveCardBack(List<int> cardIds)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                try
                {
                    for (int i = 0; i < cardIds.Count(); i++)
                    {
                        UsersAccessUnit usersAccessUnit = _usersAccessUnitRepository.FindById(cardIds[i]);

                        var logCardEntity =
                               new UserAccessUnitEventEntity(usersAccessUnit);

                        usersAccessUnit.Active = true;
                        usersAccessUnit.ClassificatorValueId = null;
                        logCardEntity.SetNewUserAccessUnit(_usersAccessUnitRepository.FindById(usersAccessUnit.Id));

                        _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName,
                                              CurrentUser.Get().CompanyId, logCardEntity.GetGiveCardBack());

                        var card_message = string.Format("Give Crad Back: Card '{0}'. Valid from '{1}' to '{2}'. Active '{3}'", string.IsNullOrEmpty(usersAccessUnit.Code)
                                                                                            ? string.Format("{0} {1}",
                                                                                                            usersAccessUnit.Serial,
                                                                                                            usersAccessUnit.Dk)
                                                                                                            : usersAccessUnit.Code,
                                                                                                            usersAccessUnit.ValidFrom.HasValue ? usersAccessUnit.ValidFrom.Value.ToString("dd.MM.yyyy") : "not setted",
                                                                                                            usersAccessUnit.ValidTo.HasValue ? usersAccessUnit.ValidTo.Value.ToString("dd.MM.yyyy") : "not setted", usersAccessUnit.Active);

                        _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, usersAccessUnit.Id, UpdateParameter.UserCardChange, ControllerStatus.Edited, card_message);
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }
    }
}