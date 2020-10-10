using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using AutoMapper;
using FoxSec.Core.SystemEvents.DTOs;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Core.SystemEvents
{
	public class UserAccessUnitEventEntity : ILogEventEntity 
	{
		public UserAccessUnitEventEntity(UsersAccessUnit userAccessUnit)
		{
			OldValue = new UserAccessUnitEntity();
			NewValue = new UserAccessUnitEntity();
			Mapper.Map(userAccessUnit, OldValue);
		}

		public void SetNewUserAccessUnit(UsersAccessUnit userAccessUnit)
		{
			Mapper.Map(userAccessUnit, NewValue);
		}
		
		public UserAccessUnitEntity OldValue { get; set; }

		public UserAccessUnitEntity NewValue { get; set; }

		public string GetCreateMessage()
		{
			var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCardCreated", new List<string> { string.IsNullOrWhiteSpace(OldValue.Code) ? string.Format("{0} {1}", OldValue.Serial, OldValue.Dk) : OldValue.Code }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUser", new List<string> { string.IsNullOrEmpty(OldValue.UserName) ? "" : OldValue.UserName }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCompanyName", new List<string> { string.IsNullOrEmpty(OldValue.UserName) ? "" : OldValue.CompanyName }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCardTypeName", new List<string> { string.IsNullOrEmpty(OldValue.TypeName) ? "" : OldValue.TypeName }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageSerial", new List<string> { string.IsNullOrEmpty(OldValue.Serial) ? "" : OldValue.Serial }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCardCode", new List<string> { string.IsNullOrEmpty(OldValue.Code) ? "" : OldValue.Code }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageDk", new List<string> { string.IsNullOrEmpty(OldValue.Dk) ? "" : OldValue.Dk }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFree", new List<string> { OldValue.Free.ToString() }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageActive", new List<string> { OldValue.Active.ToString() }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageValidFrom", new List<string> { OldValue.ValidFrom }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageValidTo", new List<string> { OldValue.ValidTo }));
			
			return message.ToString();

		}

		public string GetDeleteMessage()
		{
			var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCardDeleted", new List<string> { string.IsNullOrWhiteSpace(OldValue.Code) ? string.Format("{0} {1}", OldValue.Serial, OldValue.Dk) : OldValue.Code }));
			
			return message.ToString();
		}

		public string GetEditMessage()
		{
			var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCardChanged", new List<string> { string.IsNullOrWhiteSpace(OldValue.Code) ? string.Format("{0} {1}", OldValue.Serial, OldValue.Dk) : OldValue.Code }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserNameChanged", new List<string> { string.IsNullOrEmpty(OldValue.UserName) ? "" : OldValue.UserName,
				string.IsNullOrEmpty(NewValue.UserName) ? "" : NewValue.UserName}));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCompanyNameChanged", new List<string> { string.IsNullOrEmpty(OldValue.CompanyName) ? "" : OldValue.CompanyName,
				string.IsNullOrEmpty(NewValue.CompanyName) ? "" : NewValue.CompanyName }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCardTypeNameChanged", new List<string> { string.IsNullOrEmpty(OldValue.TypeName) ? "" : OldValue.TypeName,
				string.IsNullOrEmpty(NewValue.TypeName) ? "" : NewValue.TypeName }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageSerialChanged", new List<string> { string.IsNullOrEmpty(OldValue.Serial) ? "" : OldValue.Serial,
				string.IsNullOrEmpty(NewValue.Serial) ? "" : NewValue.Serial }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCardCodeChanged", new List<string> { string.IsNullOrEmpty(OldValue.Code) ? "" : OldValue.Code,
				string.IsNullOrEmpty(NewValue.Code) ? "" : NewValue.Code }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageDkChanged", new List<string> { string.IsNullOrEmpty(OldValue.Dk) ? "" : OldValue.Dk,
				string.IsNullOrEmpty(NewValue.Dk) ? "" : NewValue.Dk }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFreeChanged", new List<string> { OldValue.Free.ToString(), NewValue.Free.ToString() }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageActiveChanged", new List<string> { OldValue.Active.ToString(), NewValue.Active.ToString() }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageValidFromChanged", new List<string> { OldValue.ValidFrom, NewValue.ValidFrom }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageValidToChanged", new List<string> { OldValue.ValidTo, NewValue.ValidTo }));

			return message.ToString();
		}

        public string GetGiveCardBack()
        {
            var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
            message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCardChanged", new List<string> { string.IsNullOrWhiteSpace(OldValue.Code) ? string.Format("{0} {1}", OldValue.Serial, OldValue.Dk) : OldValue.Code }));
            message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageActiveChanged", new List<string> { "0", "1" }));         
            return message.ToString();
        }
    }
}
