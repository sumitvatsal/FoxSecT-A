using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoxSec.Authentication;
using FoxSec.Common.EventAggregator;
using FoxSec.Core.Infrastructure.UnitOfWork;
using FoxSec.Core.SystemEvents;
using FoxSec.DomainModel;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.ServiceLayer.Contracts;
using System.Collections;

namespace FoxSec.ServiceLayer.Services
{
    internal class TAMoveService : ServiceBase, ITAMoveService
    {
        private readonly ITAReportRepository _taReportRepository;
        private readonly ITAMoveRepository _TAMoveRepository;
        private readonly ILogService _logService;
        private readonly IControllerUpdateService _controllerUpdateService;
        private string falg = "";
        public TAMoveService(ICurrentUser currentUser,
                               IDomainObjectFactory domainObjectFactory,
                               IEventAggregator eventAggregator,
                               ITAReportRepository taReportRepository,
                               ITAMoveRepository TAMoveRepository,
                               ILogService logService,
                               IControllerUpdateService controllerUpdateService)
            : base(currentUser, domainObjectFactory, eventAggregator)
        {
            _taReportRepository = taReportRepository;
            _TAMoveRepository = TAMoveRepository;
            _controllerUpdateService = controllerUpdateService;
            _logService = logService;
        }

        public void LockTAMovesByTAReport(int TAReportId)
        {
            TAReport rp = _taReportRepository.FindById(TAReportId);
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                List<TAMove> mvs = _TAMoveRepository.FindAll(x => x.UserId == rp.UserId && x.Started.Date == rp.ReportDate.Date && !x.IsDeleted).ToList();

                foreach (TAMove mv in mvs)
                {
                    mv.Status = 2;
                }
                work.Commit();
            }
        }
        public void DeleteTAMovesByTAReport(int TAReportId)
        {
            TAReport rp = _taReportRepository.FindById(TAReportId);
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                List<TAMove> mvs = _TAMoveRepository.FindAll(x => x.UserId == rp.UserId && x.Started.Date == rp.ReportDate.Date && !x.IsDeleted).ToList();

                foreach (TAMove mv in mvs)
                {
                    mv.IsDeleted = true;
                    mv.Status = 2;
                }
                work.Commit();
            }
        }
        public void CreateTAMove(int userId, int departmentId, string name,
            DateTime ReportDate, Int16 day, float hours, int shift, byte status, Boolean completed, Boolean isDeleted)
        {
            int _hours, _minutes;
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                TAReport taReport = DomainObjectFactory.CreateTAReport();
                taReport.UserId = userId;
                taReport.DepartmentId = departmentId;
                taReport.Name = name;
                //taReport.YearMonth = yearMonth;
                taReport.Day = day;
                taReport.Hours = hours;
                _hours = (int)Math.Floor(hours);                // täisarvulised tunnid
                _minutes = (int)((hours - _hours) * 60) % 60;     // murdosa minutiteks
                taReport.Hours_Min = Convert.ToString(_hours) + ':';
                if (_minutes < 10) taReport.Hours_Min = taReport.Hours_Min + Convert.ToString(_minutes);
                else taReport.Hours_Min = taReport.Hours_Min + '0' + Convert.ToString(_minutes);
                taReport.Shift = shift;
                taReport.Status = status;
                taReport.Completed = completed;
                taReport.IsDeleted = isDeleted;
                taReport.Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks);
                //              taReport.ScheduleNo = 1;
                taReport.ModifiedLast = DateTime.Now;
                taReport.ModifiedId = CurrentUser.Get().Id;
                _taReportRepository.Add(taReport);
                work.Commit();

                var taReportLogEntity = new TAReportEventEntity(taReport);

                _logService.CreateLog(CurrentUser.Get().Id, "web", falg,CurrentUser.Get().HostName, CurrentUser.Get().CompanyId, taReportLogEntity.GetCreateMessage());

                string taReport_value = string.Format("{0} {1} {2}", name, hours, taReport.ModifiedLast.ToString("dd.MM.yyyy"));

                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, taReport.Id, UpdateParameter.TAReportChange, ControllerStatus.Created, taReport_value);

            }
        }

        public void DeleteTAMove(int id)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                TAMove Move = _TAMoveRepository.FindById(id);

                if (Move.Hours > 0)
                {
                    TAReport rp = _taReportRepository.FindAll(x=>!x.IsDeleted && x.ReportDate.Date.DayOfYear == Move.Started.Date.DayOfYear && x.UserId == Move.UserId && x.DepartmentId == Move.DepartmentId).First();
                    rp.Hours = rp.Hours - Move.Hours;
                    TimeSpan t = TimeSpan.FromSeconds(rp.Hours);
                    rp.Hours_Min = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);// <3
                    rp.ModifiedId = CurrentUser.Get().Id;
                    Move.IsDeleted = true;
                }
                else {
                    Move.IsDeleted = true;
                }
                work.Commit();

                string msg = Move.UserId.ToString() + " TAMove_Change_Deleted";
                _logService.CreateLog(Move.UserId, "web", falg, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId, msg);
               // _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, Move.Id, UpdateParameter.TAReportChange, ControllerStatus.Deleted, "TAMove_Deleted");
            }
        }

        public void EditTAMove(int id, int userId, int departmentId, string name,
           DateTime ReportDate, Int16 day, float hours, int shift, byte status, Boolean completed, Boolean isDeleted)
        {
            int _hours, _minutes;
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                TAReport taReport = _taReportRepository.FindById(id);

                var taReportLogEntity = new TAReportEventEntity(taReport);

                taReport.UserId = userId;
                taReport.DepartmentId = departmentId;
                taReport.Name = name;
                // taReport.YearMonth = yearMonth;
                taReport.Day = day;
                taReport.Hours = hours;
                _hours = (int)Math.Floor(hours);
                _minutes = (int)(hours * 60) % 60;
                taReport.Hours_Min = Convert.ToString(_hours) + ':' + Convert.ToString(_minutes);
                //              taReport.Hours_Min = int(hours).ToString("C2");
                taReport.Shift = shift;
                taReport.Status = status;
                taReport.Completed = completed;
                taReport.IsDeleted = isDeleted;
                taReport.Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks);
                //              taReport.ScheduleNo = 1;     //   ScheduleNo;
                taReport.ModifiedLast = DateTime.Now;
                taReport.ModifiedId = CurrentUser.Get().Id;
                _taReportRepository.Add(taReport);
                work.Commit();
                //              taReportLogEntity.SetNewTAReport(taReport);

                _logService.CreateLog(CurrentUser.Get().Id, "web", falg, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId, taReportLogEntity.GetEditMessage());

                string taReport_value = string.Format("{0} {1} {2}", taReport.Name, taReport.ModifiedLast.ToString("dd.MM.yyyy"), hours);

                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, taReport.Id, UpdateParameter.TAReportChange, ControllerStatus.Edited, taReport_value);
            }
        }
    }
}

