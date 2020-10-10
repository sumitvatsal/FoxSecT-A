using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using FoxSec.Common.Extensions;
using FoxSec.Common.Helpers;
using FoxSec.Core.Infrastructure.BackgroundTask;

namespace FoxSec.Core.Infrastructure.Bootstrapper
{
	internal class StartBackgroundTasks : IBootstrapperTask
	{
		private readonly IBackgroundTask[] _tasks;

		public StartBackgroundTasks(IBackgroundTask[] tasks)
		{
			Contract.Requires(Check.Argument.IsNotEmpty(tasks));

			_tasks = tasks;
		}
		public void Execute()
		{
			_tasks.ForEach(task => task.Start());
		}
	}
}
