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
	public class UserDepartmentEventEntity : ILogEventEntity 
	{
		public UserDepartmentEventEntity(UserDepartment userDepartment)
		{
			OldValue = new UserDepartmentEntity();
			NewValue = new UserDepartmentEntity();
			Mapper.Map(userDepartment, OldValue);
		}

		public void SetNewUserDepartment(UserDepartment userDepartment)
		{
			Mapper.Map(userDepartment, NewValue);
		}
		
		public UserDepartmentEntity OldValue { get; set; }

		public UserDepartmentEntity NewValue { get; set; }

		public string GetCreateMessage()
		{
			var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserDepartmentAssigned", new List<string> { OldValue.DepartmentName, OldValue.UserName }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageValidFrom", new List<string> { OldValue.ValidFrom }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageValidTo", new List<string> { OldValue.ValidTo }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUDCurrentDepartment", new List<string> { OldValue.CurrentDep.ToString() }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUDDepartmentManager", new List<string> { OldValue.IsDepartmentManager.ToString() }));
			
			return message.ToString();

		}

		public string GetDeleteMessage()
		{
			return string.Empty;
		}

		public string GetEditMessage()
		{
			var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageDepartmentForUserChanged", new List<string> { OldValue.UserName }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserNameChanged", new List<string> { OldValue.UserName, NewValue.UserName }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageDepartmentNameChanged", new List<string> { OldValue.DepartmentName, NewValue.DepartmentName }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageValidFromChanged", new List<string> { OldValue.ValidFrom, NewValue.ValidFrom }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageValidToChanged", new List<string> { OldValue.ValidTo, NewValue.ValidTo }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUDCurrentDepartmentChanged", new List<string> { OldValue.CurrentDep.ToString(), NewValue.CurrentDep.ToString() }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUDDepartmentManagerChanged", new List<string> { OldValue.IsDepartmentManager.ToString(), NewValue.IsDepartmentManager.ToString() }));
			
			return message.ToString();
		}
	}
}
