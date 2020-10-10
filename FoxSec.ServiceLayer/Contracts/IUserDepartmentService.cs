using System;
using System.Collections.Generic;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.ServiceLayer.Contracts
{
    public interface IUserDepartmentService
    {
        //void UpdateUserId(int Id, int UserId);

        void DeleteUserDepartment(int Id);
        void DeleteUserDepartment(int Id, int departmentId);

        void AddUserDepartment(bool currentDep,
                               int departmentId,
                               bool isDeleted,
                               bool isDepartmentManager,
                               int userId,
                               DateTime validFrom,
                               DateTime validTo);

        void AddDepartmentManager(int userId,
                                  int departmentId,
                                  DateTime validFrom,
                                  DateTime validTo);

        void SetCurrentDepartament(int userId, bool value);
        void UpdateUserDepartment(int userId, int departmentId, DateTime validFrom, DateTime validTo);
        void UpdateUserDepartment(int userId, int departmentId, DateTime validFrom, DateTime validTo, bool isDelete);
        IEnumerable<UserDepartment> GetDepartmentManagers(int departmentId);
        void MoveToDepartment(int userId, int oldDepartmentId, int departmentId);
        void DeleteUserFromDepartments(int userId);
        void DeleteDepartmentUserWithRole(int departmentId, int roleId);
    }
}