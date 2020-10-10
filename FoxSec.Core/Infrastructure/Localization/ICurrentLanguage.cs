namespace FoxSec.Core.Infrastructure.Localization
{
	public interface ICurrentLanguage
	{
		string Get();
		void Set(string cultureIso2);
		void Reset();
	}
}