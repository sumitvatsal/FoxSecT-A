using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxSec.Core.SystemEvents.DTOs
{
	public class CardTypeEntity : LogEntity
	{
		public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCardCode { get; set; }
        public bool IsSerDK { get; set; }		
	}
}
