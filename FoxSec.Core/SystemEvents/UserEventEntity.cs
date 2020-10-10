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
	public class UserEventEntity : ILogEventEntity 
	{
		public UserEventEntity(User user)
		{
			OldValue = new UserEntity();
			NewValue = new UserEntity();
			Mapper.Map(user, OldValue);
		}

		public void SetNewUser(User user)
		{
			Mapper.Map(user, NewValue);
		}
		
		public UserEntity OldValue { get; set; }

		public UserEntity NewValue { get; set; }

		public string GetCreateMessage()
		{
			var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserCreated", new List<string> { OldValue.LoginName }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFirstName", new List<string> { OldValue.FirstName }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageLastName", new List<string> { OldValue.LastName }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCompanyName", new List<string> { OldValue.CompanyName }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserId", new List<string> { OldValue.PersonalId }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageEmail", new List<string> { OldValue.Email }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessagePersonalCode", new List<string> { OldValue.PersonalCode }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageExternalPersonalCode", new List<string> { OldValue.ExternalPersonalCode }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageBirthday", new List<string> { OldValue.Birthday }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessagePIN1", new List<string> { OldValue.PIN1 }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessagePIN2", new List<string> { OldValue.PIN2 }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageRegisteredDate", new List<string> { OldValue.RegistredStartDate }));
			
			return message.ToString();

		}

		public string GetDeleteMessage()
		{
			var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserDeleted", new List<string> { OldValue.LoginName }));
			
			return message.ToString();
		}

		public string GetEditMessage()
		{
			var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserChanged", new List<string> { OldValue.LoginName }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageLoginChanged", new List<string> { OldValue.LoginName, NewValue.LoginName }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageFirstNameChanged", new List<string> { OldValue.FirstName, NewValue.FirstName }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageLastNameChanged", new List<string> { OldValue.LastName, NewValue.LastName }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageCompanyNameChanged", new List<string> { OldValue.CompanyName, NewValue.CompanyName }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserIdChanged", new List<string> { string.IsNullOrEmpty(OldValue.PersonalId) ? "" : OldValue.PersonalId,
				string.IsNullOrEmpty(NewValue.PersonalId) ? "" : NewValue.PersonalId }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageEmailChanged", new List<string> { string.IsNullOrEmpty(OldValue.Email) ? "" : OldValue.Email,
				string.IsNullOrEmpty(NewValue.Email) ? "" : NewValue.Email }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessagePersonalCodeChanged", new List<string> { OldValue.PersonalCode, NewValue.PersonalCode }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageExternalPersonalCodeChanged", new List<string> { OldValue.ExternalPersonalCode, NewValue.ExternalPersonalCode }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageBirthdayChanged", new List<string> { OldValue.Birthday, NewValue.Birthday }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessagePIN1Changed", new List<string> { OldValue.PIN1, NewValue.PIN1 }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessagePIN2Changed", new List<string> { OldValue.PIN2, NewValue.PIN2 }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageRegisteredDateChanged", new List<string> { OldValue.RegistredStartDate, NewValue.RegistredStartDate }));

			return message.ToString();
		}

		public string ChangeWorkDataMessage()
		{
			var message = new XElement(XMLLogLiterals.LOG_MESSAGE);
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageUserChanged", new List<string> { OldValue.LoginName }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageTitleNameChanged", new List<string> { OldValue.TitleName, NewValue.TitleName }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageContractNumberChanged", new List<string> { OldValue.ContractNum, NewValue.ContractNum }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageContractStartDateChanged", new List<string> { OldValue.ContractStartDate, NewValue.ContractStartDate }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageContractEndDateChanged", new List<string> { OldValue.ContractEndDate, NewValue.ContractEndDate }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessagePermitOfWorkChanged", new List<string> { OldValue.PermitOfWork, NewValue.PermitOfWork }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageWorkTimeChanged", new List<string> { OldValue.WorkTime.ToString(), NewValue.WorkTime.ToString() }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageTableNumberChanged", new List<string> { OldValue.TableNumber.ToString(), NewValue.TableNumber.ToString() }));
			message.Add(XMLLogMessageHelper.TemplateToXml("LogMessageEServiceAllowedChanged", new List<string> { OldValue.EServiceAllowed.ToString(), NewValue.EServiceAllowed.ToString() }));

			return message.ToString();
		}
	}
}
