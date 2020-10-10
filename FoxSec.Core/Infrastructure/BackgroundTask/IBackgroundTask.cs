using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxSec.Core.Infrastructure.BackgroundTask
{
	public interface IBackgroundTask
	{
		bool IsRunning { get; }
		void Start();
		void Stop();
	}
}
