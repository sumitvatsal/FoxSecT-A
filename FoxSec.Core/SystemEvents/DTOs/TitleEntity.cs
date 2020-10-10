using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxSec.Core.SystemEvents.DTOs
{
	public class TitleEntity : LogEntity
	{
		public string Name { get; set; }

        public string Description { get; set; }

        public string CompanyName { get; set; }
		
	}
}
