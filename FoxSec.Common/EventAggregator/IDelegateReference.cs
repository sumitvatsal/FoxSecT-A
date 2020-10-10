using System;

namespace FoxSec.Common.EventAggregator
{
	public interface IDelegateReference
	{
		Delegate Target{ get; }
	}
}