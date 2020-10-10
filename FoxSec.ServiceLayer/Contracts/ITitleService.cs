namespace FoxSec.ServiceLayer.Contracts
{
	public interface ITitleService
	{
        void CreateTitle(string name, string description, int companyId);
        void DeleteTitle(int id);
        void EditTitle(int id, string name, string description, int companyId);
	}
}