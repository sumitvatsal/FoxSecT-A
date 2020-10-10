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
	public class LogFilterEventEntity : ILogEventEntity 
	{
		public LogFilterEventEntity(LogFilter filter)
		{
			OldValue = new LogFilterEntity();
			NewValue = new LogFilterEntity();
			Mapper.Map(filter, OldValue);
		}

		public void SetNewFilter(LogFilter filter)
		{
			Mapper.Map(filter, NewValue);
		}
		
		public LogFilterEntity OldValue { get; set; }

		public LogFilterEntity NewValue { get; set; }

		public string GetCreateMessage()
		{
			var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageLogFilterCreated", new List<string> { OldValue.Name }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFromDate", new List<string> { OldValue.FromDate }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageToDate", new List<string> { OldValue.ToDate }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageBuilding", new List<string> { OldValue.Building }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNode", new List<string> { OldValue.Node }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCompanyName", new List<string> { OldValue.Company }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUser", new List<string> { OldValue.UserName }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageActivity", new List<string> { OldValue.Activity }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageShowDefaultLog", new List<string> { OldValue.IsShowDefaultLog.HasValue ? OldValue.IsShowDefaultLog.Value.ToString() : "false" }));
			
			return message.ToString();

		}

		public string GetDeleteMessage()
		{
			var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageLogFilterDeleted", new List<string> { OldValue.Name }));
			
			return message.ToString();
		}

		public string GetEditMessage()
		{
			var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageLogFilterChanged", new List<string> { OldValue.Name }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNameChanged", new List<string> { OldValue.Name, NewValue.Name }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFromDateChanged", new List<string> { OldValue.FromDate, NewValue.FromDate }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageToDateChanged", new List<string> { OldValue.ToDate, NewValue.ToDate }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageBuildingChanged", new List<string> { OldValue.Building, NewValue.Building }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNodeChanged", new List<string> { OldValue.Node, NewValue.Node }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCompanyNameChanged", new List<string> { OldValue.Company, NewValue.Company }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserValueChanged", new List<string> { OldValue.UserName, NewValue.UserName }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageActivityChanged", new List<string> { OldValue.Activity, NewValue.Activity }));
		    message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageShowDefaultLogChanged", new List<string> { OldValue.IsShowDefaultLog.HasValue ? OldValue.IsShowDefaultLog.Value.ToString() : "false", 
				NewValue.IsShowDefaultLog.HasValue ? NewValue.IsShowDefaultLog.Value.ToString() : "false"}));

			return message.ToString();
		}
	}
}
