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
	public class TitleEventEntity : ILogEventEntity 
	{
		public TitleEventEntity(Title title)
		{
			OldValue = new TitleEntity();
			NewValue = new TitleEntity();
			Mapper.Map(title, OldValue);
		}

		public void SetNewTitle(Title title)
		{
			Mapper.Map(title, NewValue);
		}
		
		public TitleEntity OldValue { get; set; }

		public TitleEntity NewValue { get; set; }

		public string GetCreateMessage()
		{
			var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageTitleCreated", new List<string> { OldValue.Name }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageDescription", new List<string> { string.IsNullOrEmpty(OldValue.Description) ? "empty" : OldValue.Description }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCompanyName", new List<string> { OldValue.CompanyName }));

			return message.ToString();

		}

		public string GetDeleteMessage()
		{
			var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageTitleDeleted", new List<string> { OldValue.Name }));
			
			return message.ToString();
		}

		public string GetEditMessage()
		{
			var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageTitleChanged", new List<string> { OldValue.Name }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNameChanged", new List<string> { OldValue.Name, NewValue.Name }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageDescriptionChanged", new List<string> { string.IsNullOrEmpty(OldValue.Description) ? "empty" : OldValue.Description,
				string.IsNullOrEmpty(NewValue.Description) ? "empty" : NewValue.Description}));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCompanyNameChanged", new List<string> { OldValue.CompanyName, NewValue.CompanyName }));

			return message.ToString();
		}
	}
}
