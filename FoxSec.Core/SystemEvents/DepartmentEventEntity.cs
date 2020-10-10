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
	public class DepartmentEventEntity : ILogEventEntity 
	{
		public DepartmentEventEntity(Department department)
		{
			OldValue = new DepartmentEntity();
			NewValue = new DepartmentEntity();
			Mapper.Map(department, OldValue);
		}

		public void SetNewDepartment(Department department)
		{
			Mapper.Map(department, NewValue);
		}
		
		public DepartmentEntity OldValue { get; set; }

		public DepartmentEntity NewValue { get; set; }

		public string GetCreateMessage()
		{
			var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageDepartmentCreated", new List<string> { OldValue.Name }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNumberField", new List<string> { OldValue.Number }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCompanyName", new List<string> { OldValue.CompanyName }));

			return message.ToString();

		}

		public string GetDeleteMessage()
		{
			var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageDepartmentDeleted", new List<string> { OldValue.Name }));
			
			return message.ToString();
		}

		public string GetEditMessage()
		{
			var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageDepartmentChanged", new List<string> { OldValue.Name }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNameChanged", new List<string> { OldValue.Name, NewValue.Name }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNumberChanged", new List<string> { OldValue.Number, NewValue.Number }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCompanyNameChanged", new List<string> { OldValue.CompanyName, NewValue.CompanyName }));
			
			return message.ToString();
		}
	}
}
