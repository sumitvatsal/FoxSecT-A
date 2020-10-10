using FoxSec.Authentication;
using FoxSec.Common.EventAggregator;
using FoxSec.Core.Infrastructure.UnitOfWork;
using FoxSec.Core.SystemEvents;
using FoxSec.Core.SystemEvents.DTOs;
using FoxSec.DomainModel;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.Infrastructure.EF.Repositories.Interfaces;
using FoxSec.ServiceLayer.Contracts;
using FoxSec.ServiceLayer.ServiceResults;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FoxSec.ServiceLayer.Services
{
    internal class VisitorService : ServiceBase, IVisitorService
    {
        private readonly IVisitorRepository _visitorRepository;
        private readonly IClassificatorValueRepository _classificatorValueRepository;
        private readonly IUserRepository _userRepository;
        private readonly IControllerUpdateService _controllerUpdateService;
        private readonly ILogService _logService;
        private readonly ICompanyRepository _companyRepository;
        private readonly ILogRepository _logRepository;
        private readonly IUsersAccessUnitRepository _usersAccessUnitRepository;
        private readonly IUserRepository _usersRepository;
        private readonly ILogService _logservice1;

        string flag = "";
        public VisitorService(ICurrentUser currentUser,
                                        IDomainObjectFactory domainObjectFactory,
                                        IEventAggregator eventAggregator,
                                        IUserRepository userRepository,
                                        IVisitorRepository visitorRepository,
                                        IClassificatorValueRepository classificatorValueRepository,
                                        IControllerUpdateService controllerUpdateService,
                                        IUsersAccessUnitRepository usersAccessUnitRepository,
                                        ILogService logService,
                                        IUserRepository usersRepository,
                                        ILogRepository logRepository,
                                        ICompanyRepository companyRepository) : base(currentUser, domainObjectFactory, eventAggregator)
        {
            _visitorRepository = visitorRepository;
            _usersAccessUnitRepository = usersAccessUnitRepository;
            _logService = logService;
            _logservice1 = logService;
            _userRepository = userRepository;
            _classificatorValueRepository = classificatorValueRepository;
            _controllerUpdateService = controllerUpdateService;
            _companyRepository = companyRepository;
            _logRepository = logRepository;
            _usersRepository = usersRepository;
        }

        /* public VisitorCreateResult CreateVisitor(string carNr, int? userId, string firstName, string carType, System.DateTime startDateTime, System.DateTime stopDateTime, int? companyId, string lastName, string company, int? parentVisitorsId, DateTime? returnDate, string email, string host, string phoneNumber)
          {

                  var result = new VisitorCreateResult { ErrorCode = UserServiceErrorCode.Ok, Id = 0 };


                    using(IUnitOfWork work = UnitOfWork.Begin()){

                          Visitor visitor = DomainObjectFactory.CreateVisitor();
                          IFoxSecIdentity identity = CurrentUser.Get();

                          visitor.CarNr = carNr;
                          visitor.UserId = userId;
                          visitor.FirstName = firstName;
                          visitor.CarType = carType;
                          visitor.StartDateTime = startDateTime;
                          visitor.StopDateTime = stopDateTime;
                          visitor.CompanyId = companyId;
                          visitor.LastName = lastName;
                          visitor.Company = company;
                          visitor.ParentVisitorsId = parentVisitorsId;
                          visitor.ReturnDate = returnDate;
                         // visitor.Email = string.IsNullOrEmpty(email) ? "" : email;           //error
                          visitor.Email = email;
                          visitor.PhoneNumber = phoneNumber;
                          visitor.FirstName = firstName;
                          visitor.LastName = lastName;

                          //New Visitor..change accordingly
                          visitor.IsDeleted = false;
                          visitor.IsUpdated = false;
                          visitor.UpdateDatetime = startDateTime;
                          //visitor.LastChange = (DateTime)startDateTime; //error
                          visitor.LastChange = new DateTime();
                          visitor.Accept = true;
                          visitor.AcceptUserId = 1;
                          visitor.Active = true;
                          visitor.AcceptDateTime = startDateTime;

                          //Modfied accordingly
                          visitor.CardNeedReturn = true;
                          visitor.IsPhoneNrAccessUnit = false;
                          visitor.IsCarNrAccessUnit = false;

                          _visitorRepository.Add(visitor);

                          work.Commit();

                          result.Id = visitor.Id;
                          var entity = new VisitorEventEntity(_visitorRepository.FindById(result.Id));

                          var message = entity.GetCreateMessage();

                          // _logService.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, message);
                          // _logService.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, message);

                          //_controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, user.Id, UpdateParameter.UserNameChanged, ControllerStatus.Created, string.Format("{0} {1}", user.FirstName, user.LastName));

                          // _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, visitor.Id, UpdateParameter.UserStatusChanged, ControllerStatus.Created, "Active");
                          /* i19082012
                                          if( !string.IsNullOrEmpty(PIN1) )
                                          {
                                              _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, user.Id, UpdateParameter.UserPin1Changed, ControllerStatus.Created, Encryption.Decrypt(user.PIN1));
                                          }

                                          if(!string.IsNullOrEmpty(PIN2))
                                          {
                                              _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, user.Id, UpdateParameter.UserPin2Changed, ControllerStatus.Created, Encryption.Decrypt(user.PIN2));
                                          }


                    }
                  return result;

          } */

        public VisitorCreateResult CreateVisitor(string carNr, int? userId, string firstName, string carType, DateTime? startDateTime, DateTime? stopDateTime, int? companyId, string lastName, string company, string email, string host, string phoneNumber, bool isphonenraccessunit, bool iscarnraccessunit, DateTime? returndate, bool iscardneedreturn,string PersonalCode, string Comment)
        {
            var result = new VisitorCreateResult { ErrorCode = UserServiceErrorCode.Ok, Id = 0 };

            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                int? tc = 0;
                int id1 = 0;
                int tc1 = 0;
                DateTime? validto = null;
                IEnumerable<ClassificatorValue> cv = _classificatorValueRepository.FindByValue("Visitors");
                foreach (var obj in cv)
                {
                    tc = obj.Legal;
                    id1 = obj.Id;
                    validto = obj.ValidTo;
                }
                if (validto == null && tc == null)
                {
                    result.Id = 0;
                }
                else
                {
                    //tc1 = _userRepository.FindAll(x => !x.IsDeleted && x.Active == true && (x.IsShortTermVisitor == true || x.IsVisitor == true)).ToList().Count();
                    tc1 = _visitorRepository.FindAll(x => !x.IsDeleted && x.StopDateTime > DateTime.Now).ToList().Count();
                    int remaining = Convert.ToInt32(tc) - tc1;
                    remaining = remaining < 0 ? 0 : remaining;
                    if (remaining > 0 && validto > DateTime.Now)
                    {
                        Visitor visitor = DomainObjectFactory.CreateVisitor();
                        IFoxSecIdentity identity = CurrentUser.Get();
                        visitor.CarNr = carNr;
                        if (userId == 0)
                        {
                            visitor.UserId = null;
                            visitor.ParentVisitorsId = null;
                        }
                        else
                        {
                            visitor.UserId = userId;
                            //int cardid = _usersAccessUnitRepository.FindAll(x => !x.IsDeleted && x.UserId == visitor.UserId && x.TypeId == 7 && x.Active == true && x.ValidTo < DateTime.Now).Select(x => x.Id).FirstOrDefault();

                            List<UsersAccessUnit> cardidlist = _usersAccessUnitRepository.FindAll(x => !x.IsDeleted && x.UserId == visitor.UserId && x.TypeId == 7 && x.Active == true).ToList();
                            var obj = cardidlist.FirstOrDefault();
                            //visitor.ParentVisitorsId = cardid;

                            //foreach (var obj in cardidlist)
                            //{
                                UsersAccessUnit carddet = _usersAccessUnitRepository.FindById(Convert.ToInt32(obj.Id));
                                var message2 = new XElement(XMLLogLiterals.LOG_MESSAGE);
                                message2.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCardCodeSerial", new List<string> { carddet.Serial, carddet.Serial }));
                                message2.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCardCodeDk", new List<string> { carddet.Dk, carddet.Dk }));
                                message2.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCardValidFromChanged", new List<string> { Convert.ToString(carddet.ValidFrom), Convert.ToString(startDateTime) }));
                                message2.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCardValidToChanged", new List<string> { Convert.ToString(carddet.ValidTo), Convert.ToString(stopDateTime) }));
                                carddet.ValidFrom = startDateTime;
                                carddet.ValidTo = stopDateTime;
                                carddet.Closed = null;
                                carddet.Id = obj.Id;
                                work.Commit();
                                _logservice1.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, message2.ToString());
                            //}

                            _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, Convert.ToInt32(visitor.UserId), UpdateParameter.UserStatusChanged, ControllerStatus.Created, "Active");
                            
                        }
                        if (companyId == null || companyId == 0)
                        {
                            if (visitor.UserId != null)
                            {
                                companyId = _userRepository.FindById(Convert.ToInt32(visitor.UserId)).CompanyId;
                            }
                        }

                        

                        visitor.FirstName = firstName;
                        visitor.CarType = carType;
                        visitor.StartDateTime = startDateTime;
                        visitor.StopDateTime = stopDateTime;
                        visitor.CompanyId = companyId;
                        visitor.LastName = lastName;
                        //visitor.Company = company;

                        visitor.ReturnDate = stopDateTime;
                        // visitor.Email = string.IsNullOrEmpty(email) ? "" : email;           //error
                        visitor.Email = email;
                        visitor.PhoneNumber = phoneNumber;
                        visitor.FirstName = firstName;
                        visitor.LastName = lastName;

                        //New Visitor..change accordingly
                        visitor.IsDeleted = false;
                        visitor.IsUpdated = false;
                        visitor.UpdateDatetime = startDateTime;
                        //visitor.LastChange = (DateTime)startDateTime; //error
                        visitor.LastChange = startDateTime;
                        visitor.Accept = true;
                        visitor.AcceptUserId = 1;
                        visitor.Active = true;
                        visitor.AcceptDateTime = startDateTime;

                        //Modfied accordingly
                        visitor.CardNeedReturn = true;
                        visitor.IsPhoneNrAccessUnit = isphonenraccessunit;
                        visitor.IsCarNrAccessUnit = iscarnraccessunit;
                        visitor.CardNeedReturn = iscardneedreturn;
                        visitor.ReturnDate = returndate;
                        visitor.PersonalCode = PersonalCode;
                        visitor.Comment = Comment;
                        _visitorRepository.Add(visitor);

                        work.Commit();
                        try
                        {
                            result.Id = visitor.Id;
                            var entity = new VisitorEventEntity(_visitorRepository.FindById(result.Id));

                            var message = entity.GetCreateMessage();

                            _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId, message.ToString());


                        }
                        catch (Exception e) { Console.WriteLine("error:" + e); }

                    }
                    else
                    {
                        result.Id = 0 - Convert.ToInt32(tc);
                    }
                }
                return result;
            }
        }

        public void EditUserPersonalData(int id,
                                 string firstName,
                                 string lastName,
                                 int? companyid,
                                 string phonenr,
                                 bool isphonenraccessunit,
                                 string email,
                                 string carnr,
                                 bool iscarnraccessunit,
                                 string cartype,
                                 DateTime? startdatetime,
                                 DateTime? stopdatetime,
                                 int? userid,
                                 bool cardneedreturn,
                                 DateTime? returndate, 
                                 string PersonalCode, 
                                 string Comment
                                        )
        {

            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                Visitor user = _visitorRepository.FindById(id);
                IFoxSecIdentity identity = CurrentUser.Get();

                var logEntity = new VisitorEventEntity(user);

                if (companyid != user.CompanyId)
                {
                    var c1 = companyid.HasValue ? _companyRepository.FindById(companyid.Value) : null;
                }

                //if (user.UserId == userid)
                //{
                //}
                //else
                {
                    int cardid = 0;
                    if (userid == null)
                    {
                        cardid = _usersAccessUnitRepository.FindAll(x => !x.IsDeleted && x.UserId == user.UserId && x.Id == user.ParentVisitorsId).Select(x => x.Id).FirstOrDefault();
                        user.ParentVisitorsId = null;
                    }
                    else
                    {
                        cardid = _usersAccessUnitRepository.FindAll(x => !x.IsDeleted && x.UserId == userid && x.TypeId == 7 && x.Active == true && x.ValidTo < DateTime.Now).Select(x => x.Id).FirstOrDefault();
                        user.ParentVisitorsId = cardid;
                    }

                    List<UsersAccessUnit> carddetails = _usersAccessUnitRepository.FindAll(x => !x.IsDeleted && x.UserId == userid && x.TypeId == 7 && x.Active == true).ToList();
                    foreach (var obj in carddetails)
                    {
                        UsersAccessUnit carddet = _usersAccessUnitRepository.FindById(Convert.ToInt32(obj.Id));
                        var message2 = new XElement(XMLLogLiterals.LOG_MESSAGE);
                        message2.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCardCodeSerial", new List<string> { carddet.Serial, carddet.Serial }));
                        message2.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCardCodeDk", new List<string> { carddet.Dk, carddet.Dk }));
                        message2.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCardValidFromChanged", new List<string> { Convert.ToString(carddet.ValidFrom), Convert.ToString(startdatetime) }));
                        message2.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCardValidToChanged", new List<string> { Convert.ToString(carddet.ValidTo), Convert.ToString(stopdatetime) }));

                        carddet.ValidFrom = startdatetime;
                        carddet.ValidTo = stopdatetime;
                        carddet.Closed = null;
                        carddet.Id = obj.Id;
                        work.Commit();

                        _logservice1.CreateLog(CurrentUser.Get().Id, "web", flag, "", CurrentUser.Get().CompanyId, message2.ToString());
                       
                    }
                }

                if (stopdatetime != null)
                {
                    if (stopdatetime < DateTime.Now)
                    {
                        userid = null;
                    }
                }

                if (companyid == null || companyid == 0)
                {
                    if (userid != null)
                    {
                        companyid = _userRepository.FindById(Convert.ToInt32(userid)).CompanyId;
                    }
                }

                user.CompanyId = companyid;

                user.FirstName = firstName;
                user.LastName = lastName;

                user.PhoneNumber = phonenr;
                user.IsPhoneNrAccessUnit = isphonenraccessunit;
                user.Email = email;
                user.CarNr = carnr;
                user.IsCarNrAccessUnit = iscarnraccessunit;
                user.CarType = cartype;
                user.StartDateTime = startdatetime;
                user.StopDateTime = stopdatetime;
                user.UserId = userid;
                user.CardNeedReturn = cardneedreturn;
                user.ReturnDate = returndate;
                user.IsUpdated = true;
                user.UpdateDatetime = DateTime.Now;
                user.LastChange = DateTime.Now;
                user.PersonalCode = PersonalCode;
                user.Comment = Comment;

                work.Commit();

                logEntity.SetNewUser(_visitorRepository.FindById(id));

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                                  logEntity.ChangeWorkDataMessage());

                if (userid != null)
                {
                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, Convert.ToInt32(userid), UpdateParameter.UserStatusChanged, ControllerStatus.Edited, string.Format("User save button"));
                }
                else { 
}

            }
        }

        public void DeleteVisitor(int id, string host)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                Visitor user = _visitorRepository.FindById(id);


                var logEntity = new VisitorEventEntity(user);

                IFoxSecIdentity identity = CurrentUser.Get();
                var e = new VisitorDeletedEventArgs(user, identity.LoginName, identity.FirstName, identity.LastName, DateTime.Now);

                user.IsDeleted = true;
                work.Commit();

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, logEntity.GetDeleteMessage());

                //	_controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, id, UpdateParameter.UserNameChanged, ControllerStatus.Deleted, string.Empty);

                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, id, UpdateParameter.UserStatusChanged, ControllerStatus.Deleted, string.Empty);
                /*
                                if( !string.IsNullOrEmpty(PIN1) )
                                {
                                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, id, UpdateParameter.UserPin1Changed, ControllerStatus.Deleted, PIN1);
                                }

                                if( !string.IsNullOrEmpty(PIN2) )
                                {
                                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, id, UpdateParameter.UserPin2Changed, ControllerStatus.Deleted, PIN2);
                                } */

                //EventAggregator.GetEvent<UserDeletedEvent>().Publish(e);
            }
        }

        public int CreateLog(int userId, string building, string flag, string node, int? companyId, string action, int? logTypeId = null)
        {
            if (companyId == 0)
            {
                companyId = null;
            }

            int result = 0;
            using (IUnitOfWork work = UnitOfWork.Begin())
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

        public bool GiveCardBack(int id, DateTime returndate, bool cardneedreturn,int userID)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                try
                {
                    DateTime retdate = returndate.AddDays(-1);
                    Visitor card = _visitorRepository.FindById(id);
                    var logEntity = new VisitorEventEntity(card);

                    //int cardid = Convert.ToInt32(card.ParentVisitorsId);
                    int cardid = Convert.ToInt32(card.UserId);
                    //UsersAccessUnit carddet = _usersAccessUnitRepository.FindById(Convert.ToInt32(cardid));
                    List<UsersAccessUnit> carddet = _usersAccessUnitRepository.FindAll(x => !x.IsDeleted && x.UserId == cardid && x.TypeId == 7 && x.Active == true).ToList();
                    var CardFullCode = "";
                    foreach (var obj in carddet)
                    {
                        UsersAccessUnit carddets = _usersAccessUnitRepository.FindById(Convert.ToInt32(obj.Id));
                        CardFullCode = carddets.CardFullCode;
                        var message2 = new XElement(XMLLogLiterals.LOG_MESSAGE);
                        message2.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserAccessUnitCardCodeSerial", new List<string> { carddets.Serial, carddets.Serial }));
                        message2.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserAccessUnitCardCodeDk", new List<string> { carddets.Dk, carddets.Dk }));
                        message2.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserAccessUnitCardValidToChanged", new List<string> { Convert.ToString(carddets.ValidTo), Convert.ToString(card.StopDateTime) }));
                        message2.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserAccessUnitCardClosedChanged", new List<string> { Convert.ToString(carddets.Closed), Convert.ToString(returndate) }));
                        carddets.ValidTo = retdate;
                        carddets.Closed = returndate;
                        carddets.Id = obj.Id;
                        work.Commit();
                        _logservice1.CreateLog(CurrentUser.Get().Id, "web", flag, "", CurrentUser.Get().CompanyId, message2.ToString());
                    }
                    card.ReturnDate = returndate;
                    card.CardNeedReturn = cardneedreturn;
                    card.StopDateTime = retdate;
                    card.LastChange = DateTime.Now;
                    card.ParentVisitorsId = null;
                    card.UserId = null;
                    //int usrid = 0;
                    //if (card.ParentVisitorsId > 0 && card.ParentVisitorsId != null)
                    //{
                    //    usrid = (int)card.ParentVisitorsId;
                    //}

                    var ownername = "";
                    if (cardid > 0)
                    {
                        ownername = _usersRepository.FindById(cardid).LoginName;
                    }
                    work.Commit();
                    var msg = logEntity.CardReturnDataMessage(CardFullCode, ownername, returndate);
                    _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName,
                      CurrentUser.Get().CompanyId, msg);
                  
                    if (Convert.ToString(userID) != null)
                    {
                        _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, userID, UpdateParameter.UserCardChange, ControllerStatus.Edited, msg);
                    }
                    else
                    {

                    }

                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
        }

        public string Encrypttxt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            // Get the key from config file
            //   string key = (string)settingsReader.GetValue(ENCRYPTION_KEY, typeof(String));
            //System.Windows.Forms.MessageBox.Show(key);
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes("A456E4DA104F960563A66DDC"));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes("A456E4DA104F960563A66DDC");

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
    }
}
