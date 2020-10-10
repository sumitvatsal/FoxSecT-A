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
	public class HolidayEventEntity : ILogEventEntity 
	{
		public HolidayEventEntity(Holiday holiday)
		{
			OldValue = new HolidayEntity();
			NewValue = new HolidayEntity();
			Mapper.Map(holiday, OldValue);
		}

		public void SetNewHoliday(Holiday holiday)
		{
			Mapper.Map(holiday, NewValue);
		}
		
		public HolidayEntity OldValue { get; set; }

		public HolidayEntity NewValue { get; set; }

		public string GetCreateMessage()
		{
			var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageHolidayCreated", new List<string> { OldValue.Name }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageStartDate", new List<string> { OldValue.EventStart }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageEndDate", new List<string> { OldValue.EventEnd }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageMoving", new List<string> { OldValue.MovingHoliday.ToString() }));

			return message.ToString();

		}

		public string GetDeleteMessage()
		{
			var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageHolidayDeleted", new List<string> { OldValue.Name }));

			return message.ToString();
		}

		public string GetEditMessage()
		{
			var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageHolidayChanged", new List<string> { OldValue.Name }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageNameChanged", new List<string> { OldValue.Name, NewValue.Name }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageEventStartChanged", new List<string> { OldValue.EventStart, NewValue.EventStart }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageEventEndChanged", new List<string> { OldValue.EventEnd, NewValue.EventEnd }));
            
			return message.ToString();
		}

        public string GetMovingMessage()
        {
			var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageHolidayChanged", new List<string> { OldValue.Name }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageMovingChanged", new List<string> { OldValue.MovingHoliday.ToString(), NewValue.MovingHoliday.ToString() }));

            return message.ToString();
        }
	}
}
