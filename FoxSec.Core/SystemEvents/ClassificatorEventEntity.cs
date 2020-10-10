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
	public class ClassificatorEventEntity : ILogEventEntity 
	{
		public ClassificatorEventEntity(Classificator classificator)
		{
			OldValue = new ClassificatorEntity();
			NewValue = new ClassificatorEntity();
			Mapper.Map(classificator, OldValue);
		}

		public void SetNewClassificator(Classificator classificator)
		{
			Mapper.Map(classificator, NewValue);
		}
		
		public ClassificatorEntity OldValue { get; set; }

		public ClassificatorEntity NewValue { get; set; }

		public string GetCreateMessage()
		{
			var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageClassificatorCreated", new List<string> { OldValue.Description }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageComment", new List<string> { string.IsNullOrEmpty(OldValue.Comments) ? " " : OldValue.Comments }));

			return message.ToString();

		}

		public string GetDeleteMessage()
		{
			var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageClassificatorDeleted", new List<string> { OldValue.Description }));
			
			return message.ToString();
		}

		public string GetEditMessage()
		{
			var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageClassificatorChanged", new List<string> { OldValue.Description }));

			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageDescriptionChanged", new List<string> { string.IsNullOrWhiteSpace(OldValue.Description) ? " " : OldValue.Description, string.IsNullOrWhiteSpace(NewValue.Description) ? " " : NewValue.Description }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCommentChange", new List<string> { string.IsNullOrWhiteSpace(OldValue.Comments) ? " " : OldValue.Comments, string.IsNullOrWhiteSpace(NewValue.Comments) ? " " : NewValue.Comments }));

			return message.ToString();
		}
	}
}
