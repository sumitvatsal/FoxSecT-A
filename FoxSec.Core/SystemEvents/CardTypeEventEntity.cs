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
	public class CardTypeEventEntity : ILogEventEntity 
	{
		public CardTypeEventEntity(UserAccessUnitType cardType)
		{
			OldValue = new CardTypeEntity();
			NewValue = new CardTypeEntity();
			Mapper.Map(cardType, OldValue);
		}

		public void SetNewCardType(UserAccessUnitType cardType)
		{
			Mapper.Map(cardType, NewValue);
		}
		
		public CardTypeEntity OldValue { get; set; }

		public CardTypeEntity NewValue { get; set; }

		public string GetCreateMessage()
		{
			var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCardTypeCreated", new List<string> { OldValue.Name }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageDescription", new List<string> { string.IsNullOrEmpty(OldValue.Description) ? "empty" : OldValue.Description }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCardCode", new List<string> { OldValue.IsCardCode.ToString() }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageSerial", new List<string> { OldValue.IsSerDK.ToString() }));

			return message.ToString();

		}

		public string GetDeleteMessage()
		{
			var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCardTypeDeleted", new List<string> { OldValue.Name }));

			return message.ToString();
		}

		public string GetEditMessage()
		{
			var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCardTypeChanged", new List<string> { OldValue.Name }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNameChanged", new List<string> { OldValue.Name, NewValue.Name }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageDescriptionChanged", new List<string> { string.IsNullOrEmpty(OldValue.Description) ? "empty" : OldValue.Description,
				string.IsNullOrEmpty(NewValue.Description) ? "empty" : NewValue.Description }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCardCodeChanged", new List<string> { OldValue.IsCardCode.ToString(), NewValue.IsCardCode.ToString()}));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageSerialChanged", new List<string> { OldValue.IsSerDK.ToString(), NewValue.IsSerDK.ToString() }));

			return message.ToString();
		}
	}
}
