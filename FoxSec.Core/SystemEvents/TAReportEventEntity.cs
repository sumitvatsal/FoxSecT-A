using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using AutoMapper;
using FoxSec.Core.SystemEvents.DTOs;
using FoxSec.DomainModel.DomainObjects;
using System.Threading.Tasks;

namespace FoxSec.Core.SystemEvents
{
    public class TAReportEventEntity : ILogEventEntity
    {
        public TAReportEventEntity(TAReport TAReport)
        {
            OldValue = new TAReportEntity();
            NewValue = new TAReportEntity();
            Mapper.Map(TAReport, OldValue);
        }

        /*      public void SetNewHoliday(Holiday holiday) { Mapper.Map(holiday, NewValue); } */

        public TAReportEntity OldValue { get; set; }

        public TAReportEntity NewValue { get; set; }

        public string GetCreateMessage()
        {
            var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
            message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageReportCreated", new List<string> { OldValue.Name }));
            message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageReportModified", new List<string> { OldValue.ModifiedLast.ToString() }));
            message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCompleted", new List<string> { OldValue.Completed.ToString() }));
            return message.ToString();
        }

        public string GetDeleteMessage()
        {
            var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
            message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageReportDeleted", new List<string> { OldValue.Name }));
            return message.ToString();
        }

        public string GetEditMessage()
        {
            var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
            message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageReportChanged", new List<string> { OldValue.Name }));
            message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageReportChanged", new List<string> { OldValue.Name, NewValue.Name }));
            message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageReportModifiedLast", new List<string> { OldValue.ModifiedLast.ToString(), NewValue.ModifiedLast.ToString() }));
            //          message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageEventEndChanged", new List<string> { OldValue.Ended.ToString(), NewValue.Ended.ToString() }));
            return message.ToString();
        }

        public string GetMovingMessage()
        {
            var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
            message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageReportChanged", new List<string> { OldValue.Name }));
            message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageReportChanged", new List<string> { OldValue.Completed.ToString(), NewValue.Completed.ToString() }));
            return message.ToString();
        }
    }
}
