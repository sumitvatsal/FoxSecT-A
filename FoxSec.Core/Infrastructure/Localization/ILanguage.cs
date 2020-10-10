using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxSec.Core.Infrastructure.Localization
{
	public interface ILanguage
	{
		string Culture { get; }
		string DisplayName { get; }
	}
}
