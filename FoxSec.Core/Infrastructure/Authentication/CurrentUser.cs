using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FoxSec.Authentication;

namespace FoxSec.Core.Infrastructure.Authentication
{
	internal class CurrentUser : ICurrentUser
	{
		public IFoxSecIdentity Get()
		{
			return Thread.CurrentPrincipal.Identity as IFoxSecIdentity;
		}
	}
}
