using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FoxSec.Audit.Data;
using FoxSec.Common.EventAggregator;
using FoxSec.Core.Infrastructure.BackgroundTask;
using FoxSec.Core.Infrastructure.Configuration;

namespace FoxSec.Audit.Services
{
	internal abstract class AuditServiceBase : BackgroundTaskBase
	{
		protected IConfigurationSettings ConfigurationSettings { get; private set; }

		protected AuditServiceBase(IEventAggregator eventAggregator, IConfigurationSettings configurationSettings) : base(eventAggregator)
		{
			ConfigurationSettings = configurationSettings;
		}

		protected static void WriteToLog(AuditLog logItem)
		{
			using( var context = new AuditLogContext() )
			{
				context.AuditLogs.AddObject(logItem);
				context.SaveChanges();
			}
		}
	}
}
