using FoxSec.Core.SystemEvents.DTOs;

namespace FoxSec.Core.SystemEvents
{
	public interface ILogEventEntity
	{
		string GetCreateMessage();

		string GetDeleteMessage();

		string GetEditMessage();

	}
}