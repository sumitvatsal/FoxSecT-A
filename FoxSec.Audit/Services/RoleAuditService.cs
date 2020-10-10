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
	internal class RoleAuditService : AuditServiceBase
	{
		private SubscriptionToken _roleCreatedToken;
		private SubscriptionToken _roleDeletedToken;
		private SubscriptionToken _roleEditedToken;

		public RoleAuditService(IEventAggregator eventAggregator, IConfigurationSettings configurationSettings) : base(eventAggregator, configurationSettings)
		{
		}

		protected override void OnStart()
		{
			if( IsRunning )
			{
				return;
			}
			if( ConfigurationSettings.AuditRoles )
			{
				_roleCreatedToken = Subscribe<RoleCreatedEvent, RoleCreatedEventArgs>(ActionExtension.ExceptionSafe<RoleCreatedEventArgs>(RoleCreated));
				_roleDeletedToken = Subscribe<RoleDeletedEvent, RoleDeletedEventArgs>(ActionExtension.ExceptionSafe<RoleDeletedEventArgs>(RoleDeleted));
				_roleEditedToken = Subscribe<RoleEditedEvent, RoleEditedEventArgs>(ActionExtension.ExceptionSafe<RoleEditedEventArgs>(RoleEdited));
			}
		}

		protected override void OnStop()
		{
			if( !IsRunning )
			{
				return;
			}
			if( _roleCreatedToken != null )
			{
				Unsubscribe<UserCreatedEvent>(_roleCreatedToken);
			}
			if( _roleDeletedToken != null )
			{
				Unsubscribe<UserDeletedEvent>(_roleDeletedToken);
			}
			if( _roleEditedToken != null )
			{
				Unsubscribe<UserEditedEvent>(_roleEditedToken);
			}
		}

		private static void RoleCreated(RoleCreatedEventArgs e)
		{
			var log_item = new AuditLog();
			Mapper.Map(e, log_item);
			log_item.EventTypeId = (int)AuditEventType.RoleCreated;
			log_item.OldValue = string.Empty;
			log_item.NewValue = e.Role.ToXmlString();
			WriteToLog(log_item);
		}

		private static void RoleDeleted(RoleDeletedEventArgs e)
		{
			var log_item = new AuditLog();
			Mapper.Map(e, log_item);
			log_item.EventTypeId = (int)AuditEventType.RoleDeleted;
			log_item.OldValue = string.Empty;
			log_item.NewValue = e.Role.ToXmlString();
			WriteToLog(log_item);
		}

		private static void RoleEdited(RoleEditedEventArgs e)
		{
			var log_item = new AuditLog();
			Mapper.Map(e, log_item);
			log_item.EventTypeId = (int)AuditEventType.RoleEdited;
			log_item.OldValue = e.OldRole.ToXmlString();
			log_item.NewValue = e.NewRole.ToXmlString();
			WriteToLog(log_item);
		}
	}
}
