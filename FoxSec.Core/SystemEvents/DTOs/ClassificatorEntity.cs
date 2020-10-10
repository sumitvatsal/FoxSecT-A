using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxSec.Core.SystemEvents.DTOs
{
	public class ClassificatorEntity : LogEntity
	{
		public virtual string Description { get; set; }
		public virtual string Comments { get; set; }		
	}
}
