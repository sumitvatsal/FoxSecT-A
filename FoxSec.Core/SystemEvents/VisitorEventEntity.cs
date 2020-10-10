using AutoMapper;
using FoxSec.Core.SystemEvents.DTOs;
using FoxSec.DomainModel.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FoxSec.Core.SystemEvents
{
    public class VisitorEventEntity : ILogEventEntity
    {
        public VisitorEntity OldValue { get; set; }

        public VisitorEntity NewValue { get; set; }
        public VisitorEventEntity(Visitor visitor)
        {
            OldValue = new VisitorEntity();
            NewValue = new VisitorEntity();
            Mapper.Map(visitor, OldValue);
        }

        public void SetNewUser(Visitor visitor)
        {
            Mapper.Map(visitor, NewValue);
        }

        public string GetCreateMessage()
        {
            try
            {
                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFirstName", new List<string> { OldValue.FirstName.ToString() }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageLastName", new List<string> { OldValue.LastName.ToString() }));
                //  message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCompanyName", new List<string> { OldValue.Company.ToString() }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageEmail", new List<string> { OldValue.Email.ToString() }));
                return message.ToString();
            }
            catch (Exception ex1)
            {
                return ex1.Message;
            }
        }
        public string GetEditMessage()
        {

            var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
            // message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserCreated", new List<string> { OldValue.FirstName }));
            message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFirstName", new List<string> { OldValue.FirstName }));
            message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageLastName", new List<string> { OldValue.LastName }));
            message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCompanyName", new List<string> { OldValue.Company }));
            // message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserId", new List<int> { OldValue.UserId }));
            message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageEmail", new List<string> { OldValue.Email }));
            /*  message.Add(XMLLogMessageHelper.TemplateToXml("LogMessagePersonalCode", new List<string> { OldValue.PersonalCode }));
              message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageExternalPersonalCode", new List<string> { OldValue.ExternalPersonalCode }));
              message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageBirthday", new List<string> { OldValue.Birthday }));
              message.Add(XMLLogMessageHelper.TemplateToXml("LogMessagePIN1", new List<string> { OldValue.PIN1 }));
              message.Add(XMLLogMessageHelper.TemplateToXml("LogMessagePIN2", new List<string> { OldValue.PIN2 }));
              message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageRegisteredDate", new List<DateTime > { OldValue.StartDateTime })); */

            return message.ToString();

        }
        public string GetDeleteMessage()
        {
            var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
            message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserDeleted", new List<string> { OldValue.FirstName }));

            return message.ToString();
        }

        public string GetCreateSendMailMessage()
        {
            try
            {
                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogActionMessage", new List<string> { "Sending QR Code" }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFirstName", new List<string> { OldValue.FirstName.ToString() }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageLastName", new List<string> { OldValue.LastName.ToString() }));
                //  message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCompanyName", new List<string> { OldValue.Company.ToString() }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageEmail", new List<string> { OldValue.Email.ToString() }));
                return message.ToString();
            }
            catch (Exception ex1)
            {
                return ex1.Message;
            }
        }

        public string ChangeWorkDataMessage()
        {
            try
            {
                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageVisitorChanged", new List<string> { OldValue.FirstName }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFirstNameChanged", new List<string> { OldValue.FirstName, NewValue.FirstName }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageLastNameChanged", new List<string> { OldValue.LastName, NewValue.LastName }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCompanyChanged", new List<string> { Convert.ToString(OldValue.CompanyId), Convert.ToString(NewValue.CompanyId) }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessagePhoneChanged", new List<string> { OldValue.PhoneNumber, NewValue.PhoneNumber }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageEmailChanged", new List<string> { OldValue.Email, NewValue.Email }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCarNrChanged", new List<string> { OldValue.CarNr, NewValue.CarNr }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFromdateChanged", new List<string> { OldValue.StartDateTime.ToString(), NewValue.StartDateTime.ToString() }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageTodateChanged", new List<string> { OldValue.StopDateTime.ToString(), NewValue.StopDateTime.ToString() }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserIdChanged", new List<string> { Convert.ToString(OldValue.UserId), Convert.ToString(NewValue.UserId) }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageReturndateChanged", new List<string> { OldValue.ReturnDate.ToString(), NewValue.ReturnDate.ToString() }));
                return message.ToString();
            }
            catch (Exception ex1)
            {
                return ex1.Message;
            }
        }

        public string UpdateImageDataMessage()
        {
            try
            {
                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageVisitorChanged", new List<string> { OldValue.FirstName + " " + OldValue.LastName }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageImageChange", new List<string> { OldValue.Image, NewValue.Image }));
                return message.ToString();
            }
            catch
            {
                return null;
            }
        }

        public string CardReturnDataMessage(string cardcode, string owner, DateTime returndate)
        {
            try
            {
                var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageVisitorCardReturnBackBy", new List<string> { OldValue.FirstName + " " + OldValue.LastName }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageOwner", new List<string> { owner }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCardCode", new List<string> { cardcode }));
                message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageReturnDate", new List<string> { returndate.ToString() }));
                return message.ToString();
            }
            catch
            {
                return null;
            }
        }

    }
}
