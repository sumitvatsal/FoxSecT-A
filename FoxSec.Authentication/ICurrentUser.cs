namespace FoxSec.Authentication
{
	public interface ICurrentUser
	{
		IFoxSecIdentity Get();
	}
}