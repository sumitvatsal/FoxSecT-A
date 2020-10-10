using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using FoxSec.Audit.Data;
using FoxSec.Audit.Extensions;
using FoxSec.Common.EventAggregator;
using FoxSec.Common.Extensions;
using FoxSec.Core.Infrastructure.BackgroundTask;
using FoxSec.Core.Infrastructure.Configuration;
using FoxSec.Core.SystemEvents;

namespace FoxSec.Audit.Services
{
	internal class UserAuditService : AuditServiceBase
	{
		private SubscriptionToken _userCreatedToken;
		private SubscriptionToken _userDeletedToken;
		private SubscriptionToken _userEditedToken;
		public UserAuditService(IEventAggregator eventAggregator, IConfigurationSettings configurationSettings) : base(eventAggregator, configurationSettings)
		{
		}

		protected override void OnStart()
		{
			if( IsRunning )
			{
				return;
			}			
			if( ConfigurationSettings.AuditUsers )
			{
				_userCreatedToken = Subscribe<UserCreatedEvent, UserCreatedEventArgs>(ActionExtension.ExceptionSafe<UserCreatedEventArgs>(UserCreated));
				_userDeletedToken = Subscribe<UserDeletedEvent, UserDeletedEventArgs>(ActionExtension.ExceptionSafe<UserDeletedEventArgs>(UserDeleted));
				_userEditedToken = Subscribe<UserEditedEvent, UserEditedEventArgs>(ActionExtension.ExceptionSafe<UserEditedEventArgs>(UserEdited));
			}
		}

		protected override void OnStop()
		{
			if( !IsRunning )
			{
				return;
			}
			
			if( _userCreatedToken != null )
			{
				Unsubscribe<UserCreatedEvent>(_userCreatedToken);
			}

			if( _userDeletedToken != null )
			{
				Unsubscribe<UserDeletedEvent>(_userDeletedToken);
			}

			if( _userEditedToken != null )
			{
				Unsubscribe<UserDeletedEvent>(_userEditedToken);
			}
		}

		private static void UserCreated(UserCreatedEventArgs e)
		{
			var log_item = new AuditLog();
			Mapper.Map(e, log_item);
			log_item.EventTypeId = (int)AuditEventType.UserCreated;
			log_item.OldValue = string.Empty;
			log_item.NewValue = e.User.ToXmlString();
			WriteToLog(log_item);
		}

		private static void UserDeleted(UserDeletedEventArgs e)
		{
			var log_item = new AuditLog();
			Mapper.Map(e, log_item);
			log_item.EventTypeId = (int)AuditEventType.UserDeleted;
			log_item.OldValue = string.Empty;
			log_item.NewValue = e.User.ToXmlString();
			WriteToLog(log_item);
		}

		private static void UserEdited(UserEditedEventArgs e)
		{
			var log_item = new AuditLog();
			Mapper.Map(e, log_item);
			log_item.EventTypeId = (int)AuditEventType.UserEdited;
			log_item.OldValue = e.OldUser.ToXmlString();
			log_item.NewValue = e.NewUser.ToXmlString();
			WriteToLog(log_item);
		}
	}
}
