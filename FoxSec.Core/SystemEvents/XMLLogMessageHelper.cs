using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FoxSec.Core.SystemEvents.DTOs
{
	public class XMLLogMessageHelper
	{
		public static XElement TemplateToXml(string messageTemplate, IEnumerable<string> messageParams )
		{
			var xElement = new XElement(XMLLogLiterals.LOG_TRANSLATABLE_SENTENSE);
			xElement.Add(new XAttribute(XMLLogLiterals.LOG_SENTENCE_FORMAT, messageTemplate));
			if (messageParams != null)
			{
				foreach (var messageParam in messageParams)
				{
					xElement.Add(new XElement(XMLLogLiterals.LOG_SENTENSE_PARAM, messageParam));
				}
			}

			return xElement;
		}
	}
}
