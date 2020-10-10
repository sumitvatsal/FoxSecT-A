using System;
using System.Linq;
using FoxSec.Authentication;
using FoxSec.Common.EventAggregator;
using FoxSec.Core.Infrastructure.UnitOfWork;
using FoxSec.Core.SystemEvents;
using FoxSec.DomainModel;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Repositories;
using FoxSec.ServiceLayer.Contracts;

namespace FoxSec.ServiceLayer.Services
{
	internal class DepartmentService : ServiceBase, IDepartmentService
	{
        private readonly IDepartmentRepository _departmentRepository;
		private readonly ILogService _logService;
        private string flag = "";
        public DepartmentService(ICurrentUser currentUser, IDomainObjectFactory domainObjectFactory, IEventAggregator eventAggregator, IDepartmentRepository departmentRepository, ILogService logService)
			: base(currentUser, domainObjectFactory, eventAggregator)
		{
            _departmentRepository = departmentRepository;
        	_logService = logService;
		}

        public int CreateDepartment(string number, string name, string createdBy, int companyId)
		{
			using( IUnitOfWork work = UnitOfWork.Begin() )
			{
                Department department = DomainObjectFactory.CreateDepartment();

                department.Name = name;
                department.Number = number;
			    department.ModifiedLast = DateTime.Now;
			    department.ModifiedBy = createdBy;
                department.CompanyId = companyId;
                department.IsDeleted = false;

                _departmentRepository.Add(department);

				work.Commit();

				department = _departmentRepository.FindById(department.Id);

				var departmentLogEntity = new DepartmentEventEntity(department);
				_logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
				                      departmentLogEntity.GetCreateMessage());

			    return department.Id;
			}
		}

        public void DeleteDepartment(int id)
		{
			using( IUnitOfWork work = UnitOfWork.Begin() )
			{
			    Department department = _departmentRepository.FindById(id);

                if (!department.UserDepartments.Any(x => !x.IsDeleted))
                {
                    department.IsDeleted = true;
                    work.Commit();

                    var departmentLogEntity = new DepartmentEventEntity(department);
                    _logService.CreateLog(CurrentUser.Get().Id, "web", flag, CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
                                            departmentLogEntity.GetDeleteMessage());
                }
		    }
		}

        public void EditDepartment(int id, string number, string name, string modifiedBy, int companyId)
		{
			using( IUnitOfWork work = UnitOfWork.Begin() )
			{
                Department department = _departmentRepository.FindById(id);
				var departmentLogEntity = new DepartmentEventEntity(department);
				
                department.Name = name;
                department.Number = number;
			    department.ModifiedLast = DateTime.Now;
                department.ModifiedBy = modifiedBy;
                department.CompanyId = companyId;

				work.Commit();

				departmentLogEntity.SetNewDepartment(_departmentRepository.FindById(department.Id));

				_logService.CreateLog(CurrentUser.Get().Id, "web", flag,CurrentUser.Get().HostName, CurrentUser.Get().CompanyId,
									  departmentLogEntity.GetEditMessage());
			}
		}
	}
}
