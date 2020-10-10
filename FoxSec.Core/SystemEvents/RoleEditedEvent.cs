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
	public class RoleEditedEventArgs : AuditEventArgsBase
	{
		public RoleEditedEventArgs(Role oldRole, string loginName, string firstName, string lastName, DateTime eventTime)
			: base(loginName, firstName, lastName, eventTime)
		{
			OldRole = new AuditEventRole();
			NewRole = new AuditEventRole();

			Mapper.Map(oldRole, OldRole);
		}

		public void SetNewRole(Role newRole)
		{
			Mapper.Map(newRole, NewRole);
		}

		public AuditEventRole OldRole { get; private set; }
		public AuditEventRole NewRole { get; private set; }
	}

	public class RoleEditedEvent : BaseEvent<RoleEditedEventArgs>
	{
	}
}
