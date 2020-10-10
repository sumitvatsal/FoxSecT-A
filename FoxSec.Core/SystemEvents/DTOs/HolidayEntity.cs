using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxSec.Core.SystemEvents.DTOs
{
	public class HolidayEntity : LogEntity
	{
		public string Name { get; set; }
		public string Comments { get; set; }
        public string EventStart { get; set; }
        public string EventEnd { get; set; }
        public bool MovingHoliday { get; set; }
		
	}
}
