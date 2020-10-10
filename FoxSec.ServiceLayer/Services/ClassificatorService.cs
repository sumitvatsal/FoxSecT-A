using System.Collections.Generic;
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
using System;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Security.Cryptography;

namespace FoxSec.ServiceLayer.Services
{
    internal class ClassificatorService : ServiceBase, IClassificatorService
    {

        string flag = "";
        private readonly IClassificatorRepository _classificatorRepository;
        private readonly IVisitorService _visitorService;
        private readonly IClassificatorValueRepository _classificatorValueRepository;
        private readonly ILogService _logService;
        private readonly IUserRepository _userRepository;
        private readonly IUserTimeZoneRepository _userTimeZoneRepository;
        private readonly IBuildingObjectRepository _buildingObjectRepository;

        public ClassificatorService(ICurrentUser currentUser, IDomainObjectFactory domainObjectFactory,
                                    IEventAggregator eventAggregator, IClassificatorRepository classificatorRepository,
                                    ILogService logService,
                                    IVisitorService visitorService,
                                    IUserRepository userRepository,
                                    IUserTimeZoneRepository userTimeZoneRepository,
                                    IBuildingObjectRepository buildingObjectRepository,
                                    IClassificatorValueRepository classificatorValueRepository)
            : base(currentUser, domainObjectFactory, eventAggregator)
        {
            _buildingObjectRepository = buildingObjectRepository;
            _classificatorRepository = classificatorRepository;
            _classificatorValueRepository = classificatorValueRepository;
            _logService = logService;
            _userTimeZoneRepository = userTimeZoneRepository;
            _userRepository = userRepository;
            _visitorService = visitorService;
        }

        public void CreateClassificator(string name, string comment, string host)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                Classificator c = DomainObjectFactory.CreateClassificator();

                c.Description = name;
                c.Comments = comment;

                _classificatorRepository.Add(c);

                work.Commit();

                var classificatorLogEntity = new ClassificatorEventEntity(c);

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId,
                                      classificatorLogEntity.GetCreateMessage());
            }
        }

        public void CreateClassificatorValue(int classificatorId, string value, string host)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                ClassificatorValue cv = DomainObjectFactory.CreateClassificatorValue();

                cv.ClassificatorId = classificatorId;
                cv.Value = value;

                _classificatorValueRepository.Add(cv);

                work.Commit();

                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageClassificatorValueCreated", new List<string> { value }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageClassificatorValueClassificator", new List<string> { _classificatorRepository.FindById(classificatorId).Description }));

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, message.ToString());
            }
        }

        public void EditClassificator(int id, string name, string comment, string host)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                Classificator c = _classificatorRepository.FindById(id);
                var logClassificatorEntity = new ClassificatorEventEntity(c);

                c.Description = name;
                c.Comments = comment;

                work.Commit();
                logClassificatorEntity.SetNewClassificator(c);

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, logClassificatorEntity.GetEditMessage());
            }
        }

        public void EditClassificatorValue(int id, string value, string host)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                ClassificatorValue cv = _classificatorValueRepository.FindById(id);

                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageClassificatorValueChanged", new List<string> { cv.Value, value }));
                cv.Value = value;

                work.Commit();

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, message.ToString());
            }
        }

        public void DeleteClassificator(int id, string host)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                IEnumerable<ClassificatorValue> cv1 = _classificatorValueRepository.FindByClassificatorId(id);

                foreach (var obj in cv1)
                {
                    _classificatorValueRepository.Delete(obj);
                }


                work.Commit();

                Classificator c = _classificatorRepository.FindById(id);

                var logClassificatorEntity = new ClassificatorEventEntity(c);

                _classificatorRepository.Delete(c);

                work.Commit();

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, logClassificatorEntity.GetDeleteMessage());
            }
        }

        public void DeleteClassificatorValue(int id, string host)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                ClassificatorValue cv = _classificatorValueRepository.FindById(id);

                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageClassificatorValueDeleted", new List<string> { cv.Value }));

                _classificatorValueRepository.Delete(cv);

                work.Commit();

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, message.ToString());
            }
        }

        public void InsertLicencePathintbl(int id)
        {

            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                IEnumerable<ClassificatorValue> cv = _classificatorValueRepository.FindByValue("Licence Path");

                if (cv.ToList().Count == 0)
                {
                    ClassificatorValue cv1 = DomainObjectFactory.CreateClassificatorValue();

                    cv1.ClassificatorId = id;
                    cv1.Value = "Licence Path";
                    cv1.SortOrder = 8;
                    _classificatorValueRepository.Add(cv1);
                    work.Commit();
                }
            }
        }

        public int CheckLicenseLessValidation(string type, int value)
        {
            int tc = 0;
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                IEnumerable<ClassificatorValue> cv = _classificatorValueRepository.FindByValue(type);

                foreach (var obj in cv.ToList())
                {
                    if (obj.Legal != null)
                    {
                        if (obj.Legal > value)
                        {
                            return 1;
                        }
                    }
                }
            }
            return tc;
        }

        public void InsertNewLicense(string type, int value, string remainhashcode, int id, string host, string validto, int remaining, string encrypkey)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                IEnumerable<ClassificatorValue> cv = _classificatorValueRepository.FindByValue(type);
                if (type == "Licence Path")
                {
                    foreach (var obj in cv.ToList())
                    {
                        var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                        message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNewLicenseInsertedClassificatorValueChanged", new List<string> { obj.Value, type }));
                        message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNewLicenseInsertedClassificatorValueChanged", new List<string> { Convert.ToString(obj.Comments), Convert.ToString(remainhashcode) }));
                        if (obj.Legal < value || obj.Legal == null)
                        {
                            obj.Value = type;
                            obj.Comments = remainhashcode;
                            obj.ClassificatorId = id;
                            work.Commit();
                            _logService.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, message.ToString());
                        }
                    }
                }
                else
                {
                    foreach (var obj in cv.ToList())
                    {
                        var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                        message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNewLicenseInsertedClassificatorValueChanged", new List<string> { obj.Value, type }));
                        message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNewLicenseInsertedClassificatorValueChanged", new List<string> { Convert.ToString(obj.Remaining), Convert.ToString(value) }));
                        if (obj.Legal <= value || obj.Legal == null)
                        {
                            if (validto != "")
                            {
                                string strvalto = "";
                                char[] characters = (validto.Split('_')[0]).ToCharArray();

                                for (int i = 0; i < characters.Length; i++)
                                {
                                    strvalto = (i == 3 || i == 5) ? strvalto = strvalto + characters[i] + "/" : strvalto = strvalto + characters[i];
                                }
                                DateTime vldto = Convert.ToDateTime(strvalto);
                                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNewLicenseInsertedClassificatorValueChanged", new List<string> { Convert.ToString(obj.ValidTo), Convert.ToString(vldto) }));
                                obj.ValidTo = vldto;
                            }
                            else
                            {
                                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNewLicenseInsertedClassificatorValueChanged", new List<string> { Convert.ToString(obj.ValidTo), Convert.ToString(obj.ValidTo) }));
                            }

                            obj.Remaining = remaining;
                            obj.Value = type;
                            //obj.ValidToHash = validto.Split('_')[1];
                            obj.ValidToHash = Encrypttxt(type.ToLower() + validto.Split('_')[0], true, encrypkey);
                            obj.RemainingHash = Encrypttxt(Convert.ToString(obj.Remaining), true, encrypkey);
                            obj.Legal = value;
                            obj.LegalHash = remainhashcode;

                            work.Commit();
                            _logService.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, message.ToString());
                        }
                    }
                }
            }
        }

        public void UpdateLicenseValidTo(int id, string host, string validto)
        {

            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                string strvalto = "";
                char[] characters = (validto.Split('_')[0]).ToCharArray();

                for (int i = 0; i < characters.Length; i++)
                {
                    strvalto = (i == 3 || i == 5) ? strvalto = strvalto + characters[i] + "/" : strvalto = strvalto + characters[i];
                }
                DateTime vldto = Convert.ToDateTime(strvalto);

                IEnumerable<ClassificatorValue> cv = _classificatorValueRepository.FindByClassificatorId(id);
                foreach (var obj in cv.ToList())
                {
                    var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNewLicenseInsertedClassificatorValueChanged", new List<string> { obj.Value, obj.Value }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNewLicenseInsertedClassificatorValidToChanged", new List<string> { Convert.ToString(obj.ValidTo), Convert.ToString(vldto) }));

                    obj.ValidTo = vldto;
                    obj.ValidToHash = validto.Split('_')[1];

                    work.Commit();
                    _logService.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, message.ToString());
                }
            }
        }

        public string Encrypttxt(string toEncrypt, bool useHashing, string encrypkey)
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
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(encrypkey));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(encrypkey);

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