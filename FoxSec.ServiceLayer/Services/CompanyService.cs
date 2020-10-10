using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using FoxSec.Authentication;
using FoxSec.Common.Enums;
using FoxSec.Common.EventAggregator;
using FoxSec.Core.Infrastructure.UnitOfWork;
using FoxSec.Core.SystemEvents;
using FoxSec.Core.SystemEvents.DTOs;
using FoxSec.DomainModel;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.ServiceLayer.Contracts;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace FoxSec.ServiceLayer.Services
{
    internal class CompanyService : ServiceBase, ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IClassificatorValueRepository _classificatorValueRepository;
        private readonly IBuildingObjectRepository _buildingObjectRepository;
        private readonly ILogService _logservice;
        private readonly ILogService _logservice1;
        private string flag = "";
        public CompanyService(ICurrentUser currentUser, IDomainObjectFactory domainObjectFactory,
                              IEventAggregator eventAggregator, ICompanyRepository companyRepository, IClassificatorValueRepository classificatorValueRepository, IBuildingObjectRepository buildingObjectRepository,
                              IUserRepository userRepository, ILogService logService) : base(currentUser, domainObjectFactory, eventAggregator)
        {
            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _logservice = logService;
            _logservice1 = logService;
            _classificatorValueRepository = classificatorValueRepository;
            _buildingObjectRepository = buildingObjectRepository;
        }

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString);

        public int CreateCompany(int? parentId, string name, string comment, bool isCanUseOwnCards, string host, IEnumerable<CompanyBuildingDto> companyBuildings)
        {
            int result = 0;

            using (IUnitOfWork work = UnitOfWork.Begin())
            {

                Company company = DomainObjectFactory.CreateCompany();
                IFoxSecIdentity identity = CurrentUser.Get();

                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCompanyCreated", new List<string> { name, identity.LoginName }));
                if (parentId != null)
                {
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCompanyParent", new List<string> { _companyRepository.FindById(parentId.Value).Name }));
                }
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageComment", new List<string> { comment }));


                company.Name = name;
                company.ModifiedLast = DateTime.Now;
                company.ModifiedBy = identity.LoginName;
                company.Comment = comment;
                company.Active = true;
                company.IsDeleted = false;
                company.ParentId = parentId;
                company.IsCanUseOwnCards = isCanUseOwnCards;
                message.Add(isCanUseOwnCards
                                ? XMLLogMessageHelper.TemplateToXml("LogMessageCanUseOwnCardsTrue", null)
                                : XMLLogMessageHelper.TemplateToXml("LogMessageCanUseOwnCardsFalse", null));

                if (companyBuildings != null)
                {
                    foreach (var companyBuildingDto in companyBuildings)
                    {
                        if (companyBuildingDto.IsSelected)
                        {
                            var cbo = DomainObjectFactory.CreateCompanyBuildingObject();
                            var bo = _buildingObjectRepository.FindById(companyBuildingDto.BuildingObjectId);
                            message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageBuildingObjectAdded", new List<string> { bo.Description, bo.Building.Name }));
                            cbo.BuildingObjectId = companyBuildingDto.BuildingObjectId;
                            cbo.CompanyId = company.Id;
                            cbo.ValidFrom = DateTime.Now;
                            cbo.ValidTo = DateTime.Now.AddYears(50);
                            cbo.IsDeleted = false;
                            company.CompanyBuildingObjects.Add(cbo);
                        }

                    }
                }

                _companyRepository.Add(company);

                work.Commit();

                result = company.Id;

                _logservice.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, message.ToString());

            }

            return result;
        }

        public int CreateCompany(string name, string comment, bool isCanUseOwnCards, string host, IEnumerable<CompanyBuildingDto> companyBuildings)
        {
            return CreateCompany(null, name, comment, isCanUseOwnCards, host, companyBuildings);
        }

        public void UpdateCompany(int id, string name, string comment, string host)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                Company company = _companyRepository.FindById(id);
                IFoxSecIdentity identity = CurrentUser.Get();
                var old_name = company.Name;
                var old_comment = company.Comment;
                company.Name = name;
                company.ModifiedLast = DateTime.Now;
                company.ModifiedBy = identity.LoginName;
                company.Comment = comment;

                work.Commit();

                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCompanyChanged", new List<string> { old_name }));
                if (old_name != name)
                {
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNameChanged", new List<string> { old_name, name }));
                }
                if (old_comment != comment)
                {
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCommentChange", new List<string> { old_comment, comment }));
                }

                _logservice.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, message.ToString());
            }
        }

        public void UpdateCompany(int id, string name, string comment, bool isCanUseOwnCards, string host, IEnumerable<CompanyBuildingDto> companyBuildings)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                Company company = _companyRepository.FindById(id);
                IFoxSecIdentity identity = CurrentUser.Get();
                var old_name = company.Name;
                var old_comment = company.Comment;
                company.Name = name;
                company.ModifiedLast = DateTime.Now;
                company.ModifiedBy = identity.LoginName;
                company.Comment = comment;
                company.IsCanUseOwnCards = isCanUseOwnCards;
                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCompanyChanged", new List<string> { old_name }));
                if (old_name != name)
                {
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNameChanged", new List<string> { old_name, name }));
                }
                if (old_comment != comment)
                {
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCommentChange", new List<string> { old_comment, comment }));
                }
                message.Add(isCanUseOwnCards
                                ? XMLLogMessageHelper.TemplateToXml("LogMessageCanUseOwnCardsTrue", null)
                                : XMLLogMessageHelper.TemplateToXml("LogMessageCanUseOwnCardsFalse", null));
                foreach (var cb in company.CompanyBuildingObjects)
                {
                    if (!companyBuildings.Any(x => x.BuildingObjectId == cb.BuildingObjectId))
                    {
                        if (!cb.IsDeleted)
                        {
                            message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageBuildingObjectRemoved", new List<string> { cb.BuildingObject.Description, cb.BuildingObject.Building.Name }));
                        }
                        cb.IsDeleted = true;
                    }
                }
                foreach (var companyBuildingDto in companyBuildings)
                {
                    CompanyBuildingObject cbo =
                        company.CompanyBuildingObjects.Where(
                            x => x.BuildingObjectId == companyBuildingDto.BuildingObjectId).FirstOrDefault();

                    if (cbo != null)
                    {
                        if (companyBuildingDto.IsSelected)
                        {
                            cbo.ValidFrom = DateTime.Now;
                            cbo.ValidTo = DateTime.Now.AddYears(50);
                            if (cbo.IsDeleted)
                            {
                                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageBuildingObjectAdded",
                                                                              new List<string>
                                                                                  {
                                                                                      cbo.BuildingObject.Description,
                                                                                      cbo.BuildingObject.Building.Name
                                                                                  }));
                            }
                        }
                        else
                        {
                            if (!cbo.IsDeleted)
                            {
                                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageBuildingObjectRemoved",
                                                                              new List<string>
                                                                                  {
                                                                                      cbo.BuildingObject.Description,
                                                                                      cbo.BuildingObject.Building.Name
                                                                                  }));
                            }
                        }
                        cbo.IsDeleted = !companyBuildingDto.IsSelected;
                    }
                    else
                    {
                        cbo = DomainObjectFactory.CreateCompanyBuildingObject();
                        cbo.CompanyId = company.Id;
                        cbo.BuildingObjectId = companyBuildingDto.BuildingObjectId;
                        cbo.ValidFrom = DateTime.Now;
                        cbo.ValidTo = DateTime.Now.AddYears(50);
                        cbo.IsDeleted = !companyBuildingDto.IsSelected;
                        company.CompanyBuildingObjects.Add(cbo);
                        var bo = _buildingObjectRepository.FindById(companyBuildingDto.BuildingObjectId);
                        if (cbo.IsDeleted)
                        {
                            message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageBuildingObjectRemoved",
                                                                          new List<string> { bo.Description, bo.Building.Name }));
                        }
                        else
                        {
                            message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageBuildingObjectAdded",
                                                                          new List<string> { bo.Description, bo.Building.Name }));
                        }

                    }
                }

                work.Commit();

                _logservice.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, message.ToString());
            }
        }

        public void DeleteCompany(int id, string host)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                Company company = _companyRepository.FindById(id);
                company.IsDeleted = true;
                work.Commit();
                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCompanyDeleted", new List<string> { company.Name }));
                _logservice.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId,
                    message.ToString());
            }
        }

        public void Activate(int id, int? classificatorValueId, string host)
        {
            SetState(id, classificatorValueId, true, host);
        }

        public void Deactivate(int id, int? classificatorValueId, string host)
        {
            SetState(id, classificatorValueId, false, host);
        }

        public void SetState(int id, int? classificatorValueId, bool state, string host)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                Company company = _companyRepository.FindById(id);
                company.ClassificatorValueId = classificatorValueId;
                company.Active = state;
                work.Commit();

                var reason_str = classificatorValueId.HasValue
                                     ? _classificatorValueRepository.FindById(classificatorValueId.Value).Value
                                     : " ";
                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(state
                                ? XMLLogMessageHelper.TemplateToXml("LogMessageCompanyActivated",
                                                                    new List<string> { company.Name, reason_str })
                                : XMLLogMessageHelper.TemplateToXml("LogMessageCompanyDeActivated",
                                                                    new List<string> { company.Name, reason_str }));
                _logservice.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId,
                    message.ToString());
            }
        }

        public Dictionary<int, string> GetCompanyManagers(int companyId)
        {
            var departmentManagers = new Dictionary<int, string>();
            IEnumerable<User> allUsers = _userRepository.FindAll().ToList().Where(x => !x.IsDeleted).OrderBy(x => x.FirstName.ToLower()).ThenBy(x => x.LastName.ToLower());

            if (companyId != 0)
                allUsers = allUsers.Where(x => x.CompanyId == companyId || (x.CompanyId.HasValue && x.Company.ParentId == companyId));

            foreach (User user in allUsers)
            {
                foreach (UserRole role in user.UserRoles)
                {
                    if (role.Role.RoleTypeId == (int)FixedRoleType.DepartmentManager && role.IsDeleted == false && role.ValidFrom < DateTime.Now && role.ValidTo > DateTime.Now.AddDays(1))
                    {
                        departmentManagers.Add(user.Id, user.FirstName + " " + user.LastName);
                        break;
                    }
                }
            }
            return departmentManagers;
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

        public void SaveSubComapnyDetails(int compid, List<int> complist, string host)
        {
            try
            {
                con.Open();
                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                SqlCommand update = new SqlCommand("update CompanieSubCompanies set IsDeleted=1 where CompanyId='"+ compid + "'", con);
                update.ExecuteNonQuery();
                for (int i = 0; i < complist.Count; i++)
                {
                    SqlCommand cmd = new SqlCommand("insert into CompanieSubCompanies(CompanyId,ParentCompanieId,IsDeleted) values('" + compid + "','" + complist[i] + "','0')", con);
                    cmd.ExecuteNonQuery();

                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCompanieSubCompanies", new List<string> { _companyRepository.FindById(compid).Name, _companyRepository.FindById(complist[i]).Name }));
                }
                con.Close();
                _logservice.CreateLog(CurrentUser.Get().Id, "web", flag, host, CurrentUser.Get().CompanyId, message.ToString());
            }
            catch
            {
            }
        }
    }

    public class CompanyBuildingDto
    {
        public int? Id { get; set; }

        public int BuildingObjectId { get; set; }

        public bool IsSelected { get; set; }
    }

}
