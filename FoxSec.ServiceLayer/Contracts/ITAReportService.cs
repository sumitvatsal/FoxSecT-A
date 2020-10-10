using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSec.ServiceLayer.Contracts
{
    public interface ITAReportService
    {
        void CreateTAReport(int userId, int? departmentId, string name, DateTime ReportDate, Int16 day, float hours, int shift, byte status, Boolean completed, Boolean isDeleted);

        void DeleteTAReport(int id);

        void EditTAReport(int id, int userId, int departmentId, string name, DateTime ReportDate, Int16 day, float hours, int shift, byte status, Boolean completed, Boolean isDeleted);
        void EditTAReport(int id, float hours);
    }
    //      void SaveJobMove(int id, bool isChecked);
    //      void SaveComplete(int id, bool isChecked);
}
