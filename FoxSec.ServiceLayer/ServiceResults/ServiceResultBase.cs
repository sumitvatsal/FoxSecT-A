using System;

namespace FoxSec.ServiceLayer.ServiceResults
{
	public abstract class ServiceResultBase
	{
		public Enum ErrorCode { get; set; }
	}
}