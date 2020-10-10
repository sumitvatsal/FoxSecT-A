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
    public class VisitorDeletedEventArgs : AuditEventArgsBase
    {
        public VisitorDeletedEventArgs(Visitor user, string loginName, string firstName, string lastName, DateTime eventTime) : base(loginName, firstName, lastName, eventTime)
        {
        }

        public AuditEventUser User { get; private set; }
    }
}
