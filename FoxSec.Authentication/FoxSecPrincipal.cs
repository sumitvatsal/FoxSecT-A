using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace FoxSec.Authentication
{
	public class FoxSecPrincipal : GenericPrincipal, IFoxSecPrincipal
	{
		public FoxSecPrincipal(IFoxSecIdentity identity) : base(identity, new string[]{})
		{
		}
	}
}
