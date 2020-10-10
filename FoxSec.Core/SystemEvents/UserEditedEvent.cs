using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using FoxSec.Common.EventAggregator;
using FoxSec.Core.SystemEvents.DTOs;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Core.SystemEvents
{
	public class UserEditedEventArgs : AuditEventArgsBase
	{
		public UserEditedEventArgs(User oldUser, string loginName, string firstName, string lastName, DateTime eventTime)
			: base(loginName, firstName, lastName, eventTime)
		{
			this.OldUser = new AuditEventUser();
			this.NewUser = new AuditEventUser();

			Mapper.Map(oldUser, OldUser);
		}

		public void SetNewUser(User newUser)
		{
			Mapper.Map(newUser, NewUser);
		}

		public AuditEventUser OldUser { get; private set; }
		public AuditEventUser NewUser { get; private set; }
	}

	public class UserEditedEvent : BaseEvent<UserEditedEventArgs>
	{
	}
}
