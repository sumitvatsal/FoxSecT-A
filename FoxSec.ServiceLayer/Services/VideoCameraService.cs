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
using System.Data.SqlClient;

namespace FoxSec.ServiceLayer.Services
{

    internal class VideoCameraService : ServiceBase, IVideoCameraService
    {
        private readonly ILogService _logservice;

        public VideoCameraService(ICurrentUser currentUser, IDomainObjectFactory domainObjectFactory,
                                  IEventAggregator eventAggregator, IClassificatorRepository classificatorRepository,
                                  ILogService logService,
                                  IVisitorService visitorService,
                                  IUserRepository userRepository,
                                  IUserTimeZoneRepository userTimeZoneRepository,
                                  IBuildingObjectRepository buildingObjectRepository,
                                  IClassificatorValueRepository classificatorValueRepository)
          : base(currentUser, domainObjectFactory, eventAggregator)
        {
            _logservice = logService;          
        }

        SqlConnection myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["FoxSecDBContext"].ConnectionString);

        public string SaveUpdateCameraDetails(string Name, string ServerNr, string CameraNr, string Port, string ResX, string ResY, string Skip, string Delay, string QuickPreviewSeconds, string EnableLiveControls, int? Id, int? type)
        {
            try
            {

                IFoxSecIdentity identity = CurrentUser.Get();
                if (type == 2)
                {
                    int ocameraid = 0;
                    string oservernr = "";
                    string oCameraNr = "";
                    string oName = "";
                    string oPort = "";
                    string oResX = "";
                    string oResY = "";
                    string oSkip = "";
                    string oDelay = "";
                    string oEnableLiveControls = "";
                    string oQuickPreviewSeconds = "";

                    SqlCommand myCommand = new SqlCommand("select id,ServerNr,CameraNr,Name,Port,ResX,ResY,Skip,Delay,EnableLiveControls,QuickPreviewSeconds from FSCameras where id='" + Id + "'", myConnection);

                    SqlDataReader dr = myCommand.ExecuteReader();
                    while (dr.Read())
                    {
                        ocameraid = Convert.ToInt32(Id);
                        oservernr = dr["ServerNr"].ToString();
                        oCameraNr = dr["CameraNr"].ToString();
                        oName = dr["Name"].ToString();
                        oPort = dr["Port"].ToString();
                        oResX = dr["ResX"].ToString();
                        oResY = dr["ResY"].ToString();
                        oSkip = dr["Skip"].ToString();
                        oDelay = dr["Delay"].ToString();
                        oEnableLiveControls = dr["EnableLiveControls"].ToString();
                        oQuickPreviewSeconds = dr["QuickPreviewSeconds"].ToString();
                    }

                    myConnection.Open();
                    SqlCommand cmd = new SqlCommand("update FSCameras set ServerNr='" + ServerNr + "',CameraNr='" + CameraNr + "',Name='" + Name + "',Port='" + Port + "',ResX='" + ResX + "',ResY='" + ResY + "',Skip='" + Skip + "',Delay='" + Delay + "',EnableLiveControls='" + EnableLiveControls + "',QuickPreviewSeconds='" + QuickPreviewSeconds + "' where id='" + Id + "'", myConnection);
                    cmd.ExecuteNonQuery();
                    myConnection.Close();

                    var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFSCamerasUpdated", new List<string> { Name, identity.LoginName }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFSCamerasUpdatedNameChanged", new List<string> { oName, Name }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFSCamerasUpdatedServerNrChanged", new List<string> { oservernr, ServerNr }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFSCamerasUpdatedCameraNrChanged", new List<string> { oCameraNr, CameraNr }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFSCamerasUpdatedPortChanged", new List<string> { oPort, Port }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFSCamerasUpdatedResXChanged", new List<string> { oResX, ResX }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFSCamerasUpdatedResYChanged", new List<string> { oResY, ResY }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFSCamerasUpdatedSkipChanged", new List<string> { oSkip, Skip }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFSCamerasUpdatedDelayChanged", new List<string> { oDelay, Delay }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFSCamerasUpdatedEnableLiveControlsChanged", new List<string> { oEnableLiveControls, EnableLiveControls }));
                    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFSCamerasUpdatedQuickPreviewSecondsChanged", new List<string> { oQuickPreviewSeconds, QuickPreviewSeconds }));

                    _logservice.CreateLog(CurrentUser.Get().Id, "web", "", "", CurrentUser.Get().CompanyId, message.ToString());
                }
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
