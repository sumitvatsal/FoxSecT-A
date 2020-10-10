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
	public class UserDeletedEventArgs : AuditEventArgsBase
	{
		public UserDeletedEventArgs(User user, string loginName, string firstName, string lastName, DateTime eventTime) : base(loginName, firstName, lastName, eventTime)
		{
			this.User = new AuditEventUser();

			Mapper.Map(user, User);
		}

		public AuditEventUser User { get; private set; }
	}

	public class UserDeletedEvent : BaseEvent<UserDeletedEventArgs>
	{
	}
}
