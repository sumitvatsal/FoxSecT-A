using System.Collections.Generic;

namespace FoxSec.DomainModel.DomainObjects
{
	public class LogType : Entity
	{
		public virtual int NumberOfError { get; set; }

		public virtual string Name { get; set; }

        public virtual string Color { get; set; }

        public virtual bool IsDefault { get; set; }

		// relations:

        public virtual ICollection<Log> Logs { get; set; }

	}

	public enum LogTypeEnum
	{
		Errors = 1,
		Protocol = 2,
		AlarmEvent = 4,
		WcfCommand = 5,
		NormalLog = 6,
		Arming = 7,
		ArmingInfo = 8,
		DoorAlarm = 9,
		DoorInfo = 10,
		CardEvent = 11,
		NewCard = 12,
		CreditInfo = 13,
		WebDatabaseChanges = 14,
		WebAuditLog = 15,
		Weberror = 16,
		WebWarning = 17,
		WebInfo = 18,
		Warning = 19,
		TecnicalFoult = 20,
		CreditInfoAlarm = 21,
		CardAlarm = 22,
		DownloadInfo = 23,
		ControllerAlarm = 24,
		ControllerOk = 27,
		ControllerWarning = 28,
		ObjectStatus = 29
	}
}