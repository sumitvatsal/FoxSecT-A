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
	public class RoleDeletedEventArgs : AuditEventArgsBase
	{
		public RoleDeletedEventArgs(Role role, string loginName, string firstName, string lastName, DateTime eventTime)
			: base(loginName, firstName, lastName, eventTime)
		{
			Role = new AuditEventRole();

			Mapper.Map(role, Role);
		}

		public AuditEventRole Role { get; private set; }
	}

	public class RoleDeletedEvent : BaseEvent<RoleDeletedEventArgs>
	{
	}
}
