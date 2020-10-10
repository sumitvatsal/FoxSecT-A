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
    internal class TAReportService : ServiceBase, ITAReportService
    {
        private readonly ITAReportRepository _taReportRepository;
        private readonly ILogService _logService;
        private readonly IControllerUpdateService _controllerUpdateService;
        private readonly ITAMoveService _TAMoveService;
        private readonly ITAMoveRepository _TAMoveRepository;
        string flag = "";
        public TAReportService(ICurrentUser currentUser,
                                ITAMoveRepository TAMoveRepository,
                               IDomainObjectFactory domainObjectFactory,
                               IEventAggregator eventAggregator,
                               ITAMoveService TAMoveService,
                               ITAReportRepository taReportRepository,
                               ILogService logService,
                               IControllerUpdateService controllerUpdateService)
            : base(currentUser, domainObjectFactory, eventAggregator)
        {
            _TAMoveRepository = TAMoveRepository;
            _taReportRepository = taReportRepository;
            _TAMoveService = TAMoveService;
            _controllerUpdateService = controllerUpdateService;
            _logService = logService;
        }

        public void CreateTAReport(int userId, int? departmentId, string name,
            DateTime ReportDate, Int16 day, float hours, int shift, byte status, Boolean completed, Boolean isDeleted)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                TAReport taReport = new TAReport();

                    taReport = DomainObjectFactory.CreateTAReport();
                    taReport.UserId = userId;
                    taReport.DepartmentId = departmentId;
                    taReport.Name = name;
                    //taReport.YearMonth = yearMonth;
                    taReport.Day = day;
                    taReport.Hours = hours;
                    taReport.ReportDate = ReportDate;
                    TimeSpan t = TimeSpan.FromSeconds(hours);
                    taReport.Hours_Min = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
                    taReport.Shift = shift;
                    taReport.Status = status;
                    taReport.Completed = completed;
                    taReport.IsDeleted = isDeleted;
                    taReport.Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks);
                    taReport.ModifiedLast = DateTime.Now;
                    taReport.ModifiedId = CurrentUser.Get().Id;
                    _taReportRepository.Add(taReport);
                    work.Commit();
                
                var taReportLogEntity = new TAReportEventEntity(taReport);
                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId, taReportLogEntity.GetCreateMessage());

                string taReport_value = string.Format("{0} {1} {2}", name, hours, taReport.ModifiedLast.ToString("dd.MM.yyyy"));

                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, taReport.Id, UpdateParameter.TAReportChange, ControllerStatus.Created, taReport_value);
            }
        }

        public void DeleteTAReport(int id)
        {
            //    _TAMoveRepository.FindAll(x => x.UserId == usrId && x.Started.Date == date)

            using (IUnitOfWork work = UnitOfWork.Begin())
            {

                TAReport taReport = _taReportRepository.FindById(id);
                var taReport_id = taReport.Id;
                var taReportLogEntity = new TAReportEventEntity(taReport);
                if (taReport.Status == 2)
                {
                    var Tams = _TAMoveRepository.FindAll(x => x.UserId == taReport.UserId && x.Started.Date == taReport.ReportDate.Date);
                    if (Tams.Count()==0)
                    {
                        taReport.IsDeleted = true;
                    }
                    else
                    {
                        float hours = 0;
                        foreach (var tam in Tams)
                        {
                            hours = hours + tam.Hours;
                        }
                        taReport.Hours = hours;
                        TimeSpan t = TimeSpan.FromSeconds(hours);
                        taReport.Hours_Min = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
                        taReport.Status = 1;

                    }
                    work.Commit();

                    _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId, taReportLogEntity.GetDeleteMessage());

                    _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, taReport_id, UpdateParameter.TAReportChange, ControllerStatus.Deleted, string.Empty);
                }
            }
            
        }
        public void EditTAReport(int id, float hours)
        {
            using (IUnitOfWork work = UnitOfWork.Begin())
            {
                TAReport taReport = _taReportRepository.FindById(id);
              
                { 
                    var taReportLogEntity = new TAReportEventEntity(taReport);
                    TimeSpan t = TimeSpan.FromSeconds(hours);
                    string StrHours = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
                    if(taReport.Hours_Min != StrHours)
                    {
                        taReport.Hours = hours;
                        taReport.Hours_Min = StrHours;
                        taReport.Status = 2;
                        taReport.Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks);
                        taReport.ModifiedLast = DateTime.Now;
                        taReport.ModifiedId = CurrentUser.Get().Id;

                        work.Commit();

                        _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId, taReportLogEntity.GetEditMessage());

                        string taReport_value = string.Format("{0} {1} {2}", taReport.Name, taReport.ModifiedLast.ToString("dd.MM.yyyy"), hours);

                        _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, taReport.Id, UpdateParameter.TAReportChange, ControllerStatus.Edited, taReport_value);

                    }
                }
            }
            _TAMoveService.LockTAMovesByTAReport(id);

        }
        public void EditTAReport(int id, int userId, int departmentId, string name,
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

                _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId, taReportLogEntity.GetEditMessage());

                string taReport_value = string.Format("{0} {1} {2}", taReport.Name, taReport.ModifiedLast.ToString("dd.MM.yyyy"), hours);

                _controllerUpdateService.CreateControllerUpdate(CurrentUser.Get().Id, taReport.Id, UpdateParameter.TAReportChange, ControllerStatus.Edited, taReport_value);
            }
        }
    }
}
