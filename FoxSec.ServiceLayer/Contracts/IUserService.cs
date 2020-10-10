using System;
using System.Collections.Generic;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.ServiceLayer.ServiceResults;
using FoxSec.ServiceLayer.Services;

namespace FoxSec.ServiceLayer.Contracts
{
	public interface IUserService
	{
        UserCreateResult CreateUser(string firstName,
                                    string lastName,
                                    string loginName,
                                    string password,
                                    string personalId,
                                    string email,
                                    String personalCode,
                                    String externalPersonalCode,
                                    DateTime? bithday,
                                    int? companyId,
                                    string PIN1,
                                    string PIN2,
                                    byte[] photo,
                                    string host,
                                    int? languageId, int classificatorid);

        UserCreateResult ImportUser(int EmployeeID,
                            string firstName,
                            string lastName,
                            int? companyId);

        void EditUserPersonalData(int id,
                                  string firstName,
                                  string lastName,
                                  string loginName,
                                  string password,
                                  string personalId,
                                  string email,
                                  String personalCode,
                                  String externalPersonalCode,
                                  DateTime? bithday,
                                  DateTime registered,
                                  int? companyId,
                                  string PIN1,
                                  string PIN2,
                                  byte[] photo,
                                    string host,
                                int? languageId);


        void Update(int id,
                                 string firstName,
                                 string lastName,
                                 string loginName,
                                    int? company);




        void UpdateCsv(
                                 string firstName,
                                 string lastName,
                                 string loginName,
                                  string email,
                                  string personalId,
                                  string externalPersonalCode
            );


        UserCreateResult ImportCsvUser(string firstName,
                                 string lastName,
                                 string loginName,
                                  string email,
                                  string personalId,
                                  string externalPersonalCode);


        void EditOtherData(int id, string comment);

		void EditUserRoles(int id, IEnumerable<UserRoleDto> roles, string host, int? newBuilding, bool activeRoleChanged);

		void UpdateUserRoles(int id, int? companyId);

		void DeleteUserRoles(int id);

		void EditUserContactData(int id,
								 string residence,
								 string phone, string host);

		void EditUserWorkData(int id,
							  int? titleId,
							  string contractNum,
							  DateTime? contractStartDate,
							  DateTime? contractEndDate,
							  DateTime? permitOfWork,
							  bool? workTime,
							  int? tableNumber,
							  bool? eServiceAllowed,
							  string host);

		void DeleteUser(int id, string host);

		void Activate(int id, int? classificatorValueId, string host,string cardactivate);

		void Deactivate(int id, int? classificatorValueId, string host);

        void Deactivate(int id, int? classificatorValueId, string host,bool isMoveToFree);

        void RemoveUserPhoto(int id);

        void SaveAtWorkLeaving(int boid, int userid, int type,string buildingname,string node,string lastlgtype);

    }
}