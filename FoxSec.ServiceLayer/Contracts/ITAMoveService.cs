using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSec.ServiceLayer.Contracts
{
    public interface ITAMoveService
    {
        void LockTAMovesByTAReport(int TAReportId);

        void DeleteTAMovesByTAReport(int TAReportId);

        void CreateTAMove(int userId, int departmentId, string name, DateTime ReportDate, Int16 day, float hours, int shift, byte status, Boolean completed, Boolean isDeleted);

        void DeleteTAMove(int id);

        void EditTAMove(int id, int userId, int departmentId, string name, DateTime ReportDate, Int16 day, float hours, int shift, byte status, Boolean completed, Boolean isDeleted);
    }
    //      void SaveJobMove(int id, bool isChecked);
    //      void SaveComplete(int id, bool isChecked);
}
