using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using FoxSec.Core.SystemEvents;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Database;

namespace FoxSec.Infrastructure.EF.Repositories
{
    class LogRepository : RepositoryBase<Log>, ILogRepository
    {

        private readonly IUserRepository _userRepository;
        public LogRepository(IDatabaseFactory factory, IUserRepository userRepository) : base(factory)
        {
            _userRepository = userRepository;
        }

        protected override IQueryable<Log> All()
        {
            return (base.All() as ObjectSet<Log>).Include("User.UserBuildings.Building").Include("User.UserBuildings.BuildingObject").Include("Company").Include("LogType");
        }

        public IEnumerable<Log> GetLocationRecords(LogFilter logFilter, List<int> allowedUserIds, List<int> allowedCompanyIds, int? navPage, int? rowPerPage, int? sortDirection, int? sortField, out int searchedRowsCount)
        {
            searchedRowsCount = (base.All() as ObjectSet<Log>).Count(x =>
            (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
            && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
            && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
            && (string.IsNullOrEmpty(logFilter.Building) || x.Building.Contains(logFilter.Building))
            && (string.IsNullOrEmpty(logFilter.Node) || x.Node.Contains(logFilter.Node))
            && (string.IsNullOrEmpty(logFilter.Activity) || x.Action.Contains(logFilter.Activity))
            && (!logFilter.CompanyId.HasValue || (x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)))
            && (string.IsNullOrEmpty(logFilter.UserName) || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value))));

            int skip_count = 0;
            if (rowPerPage.HasValue && navPage.HasValue)
            {
                if (searchedRowsCount > rowPerPage * navPage)
                {
                    skip_count = rowPerPage.Value * navPage.Value;
                }
            }
            else
            {
                rowPerPage = searchedRowsCount;
            }

            return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                            .Where(
                                x => (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
           && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
           && (x.LogTypeId == 30 || x.LogTypeId == 31)
           && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
           && (string.IsNullOrEmpty(logFilter.Building) || x.Building.Contains(logFilter.Building))
           // && (string.IsNullOrEmpty(logFilter.Node) || x.Node.Contains(logFilter.Node))
           // && (string.IsNullOrEmpty(logFilter.Activity) || x.Action.Contains(logFilter.Activity))
           && (!logFilter.CompanyId.HasValue || (x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)))
           && (string.IsNullOrEmpty(logFilter.UserName) || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                                    log => log.EventTime).Skip(skip_count).Take(rowPerPage.Value);
        }

        public IEnumerable<Log> GetSerachedRecords(LogFilter logFilter, List<int> allowedUserIds, List<int> allowedCompanyIds, int? navPage, int? rowPerPage, int? sortDirection, int? sortField, out int searchedRowsCount)
        {
            searchedRowsCount = (base.All() as ObjectSet<Log>).Count(x =>
                (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
               && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
           && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
           && (string.IsNullOrEmpty(logFilter.Building) || x.Building.Contains(logFilter.Building))
           && (string.IsNullOrEmpty(logFilter.Node) || x.Node.Contains(logFilter.Node))
           && (string.IsNullOrEmpty(logFilter.Activity) || x.Action.Contains(logFilter.Activity))
           && (!logFilter.CompanyId.HasValue || (x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)))
           && (string.IsNullOrEmpty(logFilter.UserName) || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value))));

            int skip_count = 0;
            if (rowPerPage.HasValue && navPage.HasValue)
            {
                if (searchedRowsCount > rowPerPage * navPage)
                {
                    skip_count = rowPerPage.Value * navPage.Value;
                }
            }
            else
            {
                rowPerPage = searchedRowsCount;
            }
            if (sortField.HasValue && sortDirection.HasValue)
            {
                switch (sortField)
                {
                    case 1:
                        if (sortDirection.Value == 0)
                        {
                            return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                            .Where(
                                x => (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                                    && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
           && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
           && (string.IsNullOrEmpty(logFilter.Building) || x.Building.Contains(logFilter.Building))
           && (string.IsNullOrEmpty(logFilter.Node) || x.Node.Contains(logFilter.Node))
           && (string.IsNullOrEmpty(logFilter.Activity) || x.Action.Contains(logFilter.Activity))
           && (!logFilter.CompanyId.HasValue || (x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)))
           && (string.IsNullOrEmpty(logFilter.UserName) || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderBy(
                                    log => log.EventTime).Skip(skip_count).Take(rowPerPage.Value);
                        }
                        else return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                            .Where(
                                x => (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                                    &&
                                    (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
           && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
           && (string.IsNullOrEmpty(logFilter.Building) || x.Building.Contains(logFilter.Building))
           && (string.IsNullOrEmpty(logFilter.Node) || x.Node.Contains(logFilter.Node))
           && (string.IsNullOrEmpty(logFilter.Activity) || x.Action.Contains(logFilter.Activity))
           && (!logFilter.CompanyId.HasValue || (x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)))
           && (string.IsNullOrEmpty(logFilter.UserName) || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                                    log => log.EventTime).Skip(skip_count).Take(rowPerPage.Value);
                    case 2:
                        if (sortDirection.Value == 0)
                        {
                            return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                            .Where(
                                x => (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                                    &&
                                    (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
           && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
           && (string.IsNullOrEmpty(logFilter.Building) || x.Building.Contains(logFilter.Building))
           && (string.IsNullOrEmpty(logFilter.Node) || x.Node.Contains(logFilter.Node))
           && (string.IsNullOrEmpty(logFilter.Activity) || x.Action.Contains(logFilter.Activity))
           && (!logFilter.CompanyId.HasValue || (x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)))
           && (string.IsNullOrEmpty(logFilter.UserName) || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderBy(
                                    log => log.Building).Skip(skip_count).Take(rowPerPage.Value);
                        }
                        else
                        {
                            return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                            .Where(
                                x => (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                                    &&
                                    (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
           && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
           && (string.IsNullOrEmpty(logFilter.Building) || x.Building.Contains(logFilter.Building))
           && (string.IsNullOrEmpty(logFilter.Node) || x.Node.Contains(logFilter.Node))
           && (string.IsNullOrEmpty(logFilter.Activity) || x.Action.Contains(logFilter.Activity))
           && (!logFilter.CompanyId.HasValue || (x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)))
           && (string.IsNullOrEmpty(logFilter.UserName) || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                                    log => log.Building).Skip(skip_count).Take(rowPerPage.Value);
                        }
                    case 3:
                        if (sortDirection.Value == 0)
                        {
                            return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                            .Where(
                                x => (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                                    &&
                                    (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
           && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
           && (string.IsNullOrEmpty(logFilter.Building) || x.Building.Contains(logFilter.Building))
           && (string.IsNullOrEmpty(logFilter.Node) || x.Node.Contains(logFilter.Node))
           && (string.IsNullOrEmpty(logFilter.Activity) || x.Action.Contains(logFilter.Activity))
           && (!logFilter.CompanyId.HasValue || (x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)))
           && (string.IsNullOrEmpty(logFilter.UserName) || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderBy(
                                    log => log.Node).Skip(skip_count).Take(rowPerPage.Value);
                        }
                        else
                        {
                            return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                            .Where(
                                x => (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                                    &&
                                    (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
           && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
           && (string.IsNullOrEmpty(logFilter.Building) || x.Building.Contains(logFilter.Building))
           && (string.IsNullOrEmpty(logFilter.Node) || x.Node.Contains(logFilter.Node))
           && (string.IsNullOrEmpty(logFilter.Activity) || x.Action.Contains(logFilter.Activity))
           && (!logFilter.CompanyId.HasValue || (x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)))
           && (string.IsNullOrEmpty(logFilter.UserName) || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                                    log => log.Node).Skip(skip_count).Take(rowPerPage.Value);
                        }
                    case 4:
                        if (sortDirection.Value == 0)
                        {
                            return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                             .Where(
                                 x => (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                                     &&
                                     (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
            && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
            && (string.IsNullOrEmpty(logFilter.Building) || x.Building.Contains(logFilter.Building))
            && (string.IsNullOrEmpty(logFilter.Node) || x.Node.Contains(logFilter.Node))
            && (string.IsNullOrEmpty(logFilter.Activity) || x.Action.Contains(logFilter.Activity))
            && (!logFilter.CompanyId.HasValue || (x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)))
            && (string.IsNullOrEmpty(logFilter.UserName) || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderBy(
            log => log.Company != null ? string.Empty : log.Company.Name).Skip(skip_count).Take(rowPerPage.Value);
                        }
                        else
                        {
                            return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                            .Where(
                                x => (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                                    &&
                                    (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
           && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
           && (string.IsNullOrEmpty(logFilter.Building) || x.Building.Contains(logFilter.Building))
           && (string.IsNullOrEmpty(logFilter.Node) || x.Node.Contains(logFilter.Node))
           && (string.IsNullOrEmpty(logFilter.Activity) || x.Action.Contains(logFilter.Activity))
           && (!logFilter.CompanyId.HasValue || (x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)))
           && (string.IsNullOrEmpty(logFilter.UserName) || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                                    log => log.Company == null ? string.Empty : log.Company.Name).Skip(skip_count).Take(rowPerPage.Value);
                        }
                    case 5:
                        if (sortDirection.Value == 0)
                        {
                            return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                            .Where(
                                x => (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                                    &&
                                    (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
           && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
           && (string.IsNullOrEmpty(logFilter.Building) || x.Building.Contains(logFilter.Building))
           && (string.IsNullOrEmpty(logFilter.Node) || x.Node.Contains(logFilter.Node))
           && (string.IsNullOrEmpty(logFilter.Activity) || x.Action.Contains(logFilter.Activity))
           && (!logFilter.CompanyId.HasValue || (x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)))
           && (string.IsNullOrEmpty(logFilter.UserName) || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderBy(
           log => log.User == null ? string.Empty : (log.User.FirstName + " " + log.User.LastName)).Skip(skip_count).Take(rowPerPage.Value);
                        }
                        else
                        {
                            return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                            .Where(
                                x => (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                                    &&
                                    (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
           && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
           && (string.IsNullOrEmpty(logFilter.Building) || x.Building.Contains(logFilter.Building))
           && (string.IsNullOrEmpty(logFilter.Node) || x.Node.Contains(logFilter.Node))
           && (string.IsNullOrEmpty(logFilter.Activity) || x.Action.Contains(logFilter.Activity))
           && (!logFilter.CompanyId.HasValue || (x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)))
           && (string.IsNullOrEmpty(logFilter.UserName) || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                                    log => log.User == null ? string.Empty : (log.User.FirstName + " " + log.User.LastName)).Skip(skip_count).Take(rowPerPage.Value);
                        }
                    case 6:
                        if (sortDirection.Value == 0)
                        {
                            return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                            .Where(
                                x => (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                                    &&
                                    (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
           && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
           && (string.IsNullOrEmpty(logFilter.Building) || x.Building.Contains(logFilter.Building))
           && (string.IsNullOrEmpty(logFilter.Node) || x.Node.Contains(logFilter.Node))
           && (string.IsNullOrEmpty(logFilter.Activity) || x.Action.Contains(logFilter.Activity))
           && (!logFilter.CompanyId.HasValue || (x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)))
           && (string.IsNullOrEmpty(logFilter.UserName) || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderBy(
                                    log => log.Action).Skip(skip_count).Take(rowPerPage.Value);
                        }
                        else
                        {
                            return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                            .Where(
                                x => (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                                    &&
                                    (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
           && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
           && (string.IsNullOrEmpty(logFilter.Building) || x.Building.Contains(logFilter.Building))
           && (string.IsNullOrEmpty(logFilter.Node) || x.Node.Contains(logFilter.Node))
           && (string.IsNullOrEmpty(logFilter.Activity) || x.Action.Contains(logFilter.Activity))
           && (!logFilter.CompanyId.HasValue || (x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)))
           && (string.IsNullOrEmpty(logFilter.UserName) || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                                    log => log.Action).Skip(skip_count).Take(rowPerPage.Value);
                        }
                    default:
                        return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                            .Where(
                                x => (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                                    &&
                                    (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
           && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
           && (string.IsNullOrEmpty(logFilter.Building) || x.Building.Contains(logFilter.Building))
           && (string.IsNullOrEmpty(logFilter.Node) || x.Node.Contains(logFilter.Node))
           && (string.IsNullOrEmpty(logFilter.Activity) || x.Action.Contains(logFilter.Activity))
           && (!logFilter.CompanyId.HasValue || (x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)))
           && (string.IsNullOrEmpty(logFilter.UserName) || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                                    log => log.EventTime).Skip(skip_count).Take(rowPerPage.Value);
                }
            }
            return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                            .Where(
                                x => (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                                    &&
                                    (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
           && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
           && (string.IsNullOrEmpty(logFilter.Building) || x.Building.Contains(logFilter.Building))
           && (string.IsNullOrEmpty(logFilter.Node) || x.Node.Contains(logFilter.Node))
           && (string.IsNullOrEmpty(logFilter.Activity) || x.Action.Contains(logFilter.Activity))
           && (!logFilter.CompanyId.HasValue || (x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)))
           && (string.IsNullOrEmpty(logFilter.UserName) || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                                    log => log.EventTime).Skip(skip_count).Take(rowPerPage.Value);
        }

        //public IEnumerable<Log> GetSearchedRecordsCommonSearch(LogFilter logFilter, List<int> allowedUserIds, List<int> restrUserIds, List<int> allowedCompanyIds, int? navPage, int? rowPerPage, int? sortDirection, int? sortField, out int searchedRowsCount, bool isUserCm, bool isUserSa)
        //{
        //    searchedRowsCount = (base.All() as ObjectSet<Log>).Count(x =>
        //        ((logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
        //         && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
        //        && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
        //        && ((isUserCm && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)) || !isUserCm)
        //        && ((!isUserSa && x.UserId.HasValue && !restrUserIds.Contains(x.UserId.Value)) || isUserSa)) && (
        //         x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
        //        || (logFilter.CompanyId.HasValue && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value))
        //        || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value))));

        //    int skip_count = 0;
        //    if (rowPerPage.HasValue && navPage.HasValue)
        //    {
        //        if (searchedRowsCount > rowPerPage * navPage)
        //        {
        //            skip_count = rowPerPage.Value * navPage.Value;
        //        }
        //    }
        //    else
        //    {
        //        rowPerPage = searchedRowsCount;
        //    }
        //    if (sortField.HasValue && sortDirection.HasValue)
        //    {
        //        switch (sortField)
        //        {
        //            case 1:
        //                if (sortDirection.Value == 0)
        //                {
        //                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
        //                    .Where(
        //                        x =>
        //        ((logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
        //         && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
        //        && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
        //        && ((isUserCm && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)) || !isUserCm)
        //        && ((!isUserSa && x.UserId.HasValue && !restrUserIds.Contains(x.UserId.Value)) || isUserSa))
        //        && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
        //        || (logFilter.CompanyId.HasValue && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value))
        //        || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderBy(
        //                            log => log.EventTime).Skip(skip_count).Take(rowPerPage.Value);
        //                }
        //                else return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
        //                    .Where(
        //                        x =>
        //        ((logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
        //         && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
        //        && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
        //        && ((isUserCm && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)) || !isUserCm)
        //        && ((!isUserSa && x.UserId.HasValue && !restrUserIds.Contains(x.UserId.Value)) || isUserSa))
        //        && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
        //        || (logFilter.CompanyId.HasValue && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value))
        //        || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
        //                            log => log.EventTime).Skip(skip_count).Take(rowPerPage.Value);
        //            case 2:
        //                if (sortDirection.Value == 0)
        //                {
        //                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
        //                    .Where(
        //                        x =>
        //        ((logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
        //         && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
        //        && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
        //        && ((isUserCm && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)) || !isUserCm)
        //        && ((!isUserSa && x.UserId.HasValue && !restrUserIds.Contains(x.UserId.Value)) || isUserSa))
        //        && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
        //        || (logFilter.CompanyId.HasValue && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value))
        //        || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderBy(
        //                            log => log.Building).Skip(skip_count).Take(rowPerPage.Value);
        //                }
        //                else
        //                {
        //                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
        //                    .Where(
        //                        x =>
        //        ((logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
        //         && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
        //        && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
        //        && ((isUserCm && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)) || !isUserCm)
        //        && ((!isUserSa && x.UserId.HasValue && !restrUserIds.Contains(x.UserId.Value)) || isUserSa))
        //        && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
        //        || (logFilter.CompanyId.HasValue && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value))
        //        || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
        //                            log => log.Building).Skip(skip_count).Take(rowPerPage.Value);
        //                }
        //            case 3:
        //                if (sortDirection.Value == 0)
        //                {
        //                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
        //                    .Where(
        //                        x =>
        //        ((logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
        //         && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
        //        && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
        //        && ((isUserCm && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)) || !isUserCm)
        //        && ((!isUserSa && x.UserId.HasValue && !restrUserIds.Contains(x.UserId.Value)) || isUserSa))
        //        && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
        //        || (logFilter.CompanyId.HasValue && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value))
        //        || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderBy(
        //                            log => log.Node).Skip(skip_count).Take(rowPerPage.Value);
        //                }
        //                else
        //                {
        //                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
        //                    .Where(
        //                        x =>
        //        ((logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
        //         && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
        //        && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
        //        && ((isUserCm && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)) || !isUserCm)
        //        && ((!isUserSa && x.UserId.HasValue && !restrUserIds.Contains(x.UserId.Value)) || isUserSa))
        //        && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
        //        || (logFilter.CompanyId.HasValue && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value))
        //        || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
        //                            log => log.Node).Skip(skip_count).Take(rowPerPage.Value);
        //                }
        //            case 4:
        //                if (sortDirection.Value == 0)
        //                {
        //                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
        //                     .Where(
        //                         x =>
        //        ((logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
        //         && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
        //        && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
        //        && ((isUserCm && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)) || !isUserCm)
        //        && ((!isUserSa && x.UserId.HasValue && !restrUserIds.Contains(x.UserId.Value)) || isUserSa))
        //        && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
        //        || (logFilter.CompanyId.HasValue && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value))
        //        || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderBy(
        //    log => log.Company != null ? string.Empty : log.Company.Name).Skip(skip_count).Take(rowPerPage.Value);
        //                }
        //                else
        //                {
        //                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
        //                    .Where(
        //                        x =>
        //        ((logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
        //         && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
        //        && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
        //        && ((isUserCm && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)) || !isUserCm)
        //        && ((!isUserSa && x.UserId.HasValue && !restrUserIds.Contains(x.UserId.Value)) || isUserSa))
        //        && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
        //        || (logFilter.CompanyId.HasValue && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value))
        //        || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
        //                            log => log.Company == null ? string.Empty : log.Company.Name).Skip(skip_count).Take(rowPerPage.Value);
        //                }
        //            case 5:
        //                if (sortDirection.Value == 0)
        //                {
        //                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
        //                    .Where(
        //                        x =>
        //        ((logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
        //         && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
        //        && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
        //        && ((isUserCm && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)) || !isUserCm)
        //        && ((!isUserSa && x.UserId.HasValue && !restrUserIds.Contains(x.UserId.Value)) || isUserSa))
        //        && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
        //        || (logFilter.CompanyId.HasValue && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value))
        //        || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderBy(
        //   log => log.User == null ? string.Empty : (log.User.FirstName + " " + log.User.LastName)).Skip(skip_count).Take(rowPerPage.Value);
        //                }
        //                else
        //                {
        //                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
        //                    .Where(
        //                        x =>
        //        ((logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
        //         && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
        //        && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
        //        && ((isUserCm && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)) || !isUserCm)
        //        && ((!isUserSa && x.UserId.HasValue && !restrUserIds.Contains(x.UserId.Value)) || isUserSa))
        //        && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
        //        || (logFilter.CompanyId.HasValue && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value))
        //        || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
        //                            log => log.User == null ? string.Empty : (log.User.FirstName + " " + log.User.LastName)).Skip(skip_count).Take(rowPerPage.Value);
        //                }
        //            case 6:
        //                if (sortDirection.Value == 0)
        //                {
        //                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
        //                    .Where(
        //                        x =>
        //        ((logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
        //         && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
        //        && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
        //        && ((isUserCm && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)) || !isUserCm)
        //        && ((!isUserSa && x.UserId.HasValue && !restrUserIds.Contains(x.UserId.Value)) || isUserSa))
        //        && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
        //        || (logFilter.CompanyId.HasValue && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value))
        //        || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderBy(
        //                            log => log.Action).Skip(skip_count).Take(rowPerPage.Value);
        //                }
        //                else
        //                {
        //                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
        //                    .Where(
        //                        x =>
        //        ((logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
        //         && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
        //        && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
        //        && ((isUserCm && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)) || !isUserCm)
        //        && ((!isUserSa && x.UserId.HasValue && !restrUserIds.Contains(x.UserId.Value)) || isUserSa))
        //        && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
        //        || (logFilter.CompanyId.HasValue && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value))
        //        || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
        //                            log => log.Action).Skip(skip_count).Take(rowPerPage.Value);
        //                }
        //            default:
        //                return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
        //                    .Where(
        //                        x =>
        //        ((logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
        //         && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
        //        && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
        //        && ((isUserCm && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)) || !isUserCm)
        //        && ((!isUserSa && x.UserId.HasValue && !restrUserIds.Contains(x.UserId.Value)) || isUserSa))
        //        && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
        //        || (logFilter.CompanyId.HasValue && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value))
        //        || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
        //                            log => log.EventTime).Skip(skip_count).Take(rowPerPage.Value);
        //        }
        //    }
        //    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
        //                    .Where(
        //                        x =>
        //        ((logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
        //         && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
        //        && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
        //        && ((isUserCm && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)) || !isUserCm)
        //        && ((!isUserSa && x.UserId.HasValue && !restrUserIds.Contains(x.UserId.Value)) || isUserSa))
        //        && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
        //        || (logFilter.CompanyId.HasValue && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value))
        //        || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
        //                            log => log.EventTime).Skip(skip_count).Take(rowPerPage.Value);
        //}

           public IEnumerable<Log> GetSearchResultByIdList(List<int> logIds, List<int> logTypeIds)
        {
            return All().Where(x => logIds.Contains(x.Id) && logTypeIds.Contains(x.LogTypeId) && x.UserId.HasValue);
        }

        public IEnumerable<Log> GetListOfLogsByLogIds(List<int> logIds)
        {
            return All().Where(x => logIds.Contains(x.Id) && x.UserId.HasValue);
        }

        public IEnumerable<Log> GetSearchedRecordsCommonSearch(LogFilter logFilter, List<int> allowedUserIds, List<int> restrUserIds, List<int> allowedCompanyIds, int? navPage, int? rowPerPage, int? sortDirection, int? sortField, out int searchedRowsCount, bool isUserCm, bool isUserSa, int? compid)
        {
            logFilter.Building = !string.IsNullOrEmpty(logFilter.Building) ? logFilter.UserName.Trim() : logFilter.Building;
            logFilter.Node = !string.IsNullOrEmpty(logFilter.Node) ? logFilter.Node.Trim() : logFilter.Node;
            logFilter.Activity = !string.IsNullOrEmpty(logFilter.Activity) ? logFilter.Activity.Trim() : logFilter.Activity;

            if (compid == null)
            {
                searchedRowsCount = (base.All() as ObjectSet<Log>).Count(x =>
                    ((logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                    && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                    && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                    && ((isUserCm && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)) || !isUserCm)
                    && ((!isUserSa && x.UserId.HasValue && !restrUserIds.Contains(x.UserId.Value)) || isUserSa)) && (
                     x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                    && (logFilter.CompanyId.HasValue && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value))
                    || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value))));

                int skip_count = 0;
                if (rowPerPage.HasValue && navPage.HasValue)
                {
                    if (searchedRowsCount > rowPerPage * navPage)
                    {
                        skip_count = rowPerPage.Value * navPage.Value;
                    }
                }
                else
                {
                    rowPerPage = searchedRowsCount;
                }
                if (sortField.HasValue && sortDirection.HasValue)
                {
                    switch (sortField)
                    {
                        case 1:
                            if (sortDirection.Value == 0)
                            {
                                return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                .Where(
                                    x =>
                    ((logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                    && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                    && ((isUserCm && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)) || !isUserCm)
                    && ((!isUserSa && x.UserId.HasValue && !restrUserIds.Contains(x.UserId.Value)) || isUserSa))
                    && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                    || (logFilter.CompanyId.HasValue && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value))
                    || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderBy(
                                        log => log.EventTime).Skip(skip_count).Take(rowPerPage.Value);
                            }
                            else return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                .Where(
                                    x =>
                    ((logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                    && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                    && ((isUserCm && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)) || !isUserCm)
                    && ((!isUserSa && x.UserId.HasValue && !restrUserIds.Contains(x.UserId.Value)) || isUserSa))
                    && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                    || (logFilter.CompanyId.HasValue && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value))
                    || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                                        log => log.EventTime).Skip(skip_count).Take(rowPerPage.Value);
                        case 2:
                            if (sortDirection.Value == 0)
                            {
                                return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                .Where(
                                    x =>
                    ((logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                    && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                    && ((isUserCm && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)) || !isUserCm)
                    && ((!isUserSa && x.UserId.HasValue && !restrUserIds.Contains(x.UserId.Value)) || isUserSa))
                    && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                    || (logFilter.CompanyId.HasValue && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value))
                    || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderBy(
                                        log => log.Building).Skip(skip_count).Take(rowPerPage.Value);
                            }
                            else
                            {
                                return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                .Where(
                                    x =>
                    ((logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                    && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                    && ((isUserCm && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)) || !isUserCm)
                    && ((!isUserSa && x.UserId.HasValue && !restrUserIds.Contains(x.UserId.Value)) || isUserSa))
                    && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                    || (logFilter.CompanyId.HasValue && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value))
                    || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                                        log => log.Building).Skip(skip_count).Take(rowPerPage.Value);
                            }
                        case 3:
                            if (sortDirection.Value == 0)
                            {
                                return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                .Where(
                                    x =>
                    ((logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                    && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                    && ((isUserCm && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)) || !isUserCm)
                    && ((!isUserSa && x.UserId.HasValue && !restrUserIds.Contains(x.UserId.Value)) || isUserSa))
                    && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                    || (logFilter.CompanyId.HasValue && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value))
                    || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderBy(
                                        log => log.Node).Skip(skip_count).Take(rowPerPage.Value);
                            }
                            else
                            {
                                return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                .Where(
                                    x =>
                    ((logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                    && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                    && ((isUserCm && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)) || !isUserCm)
                    && ((!isUserSa && x.UserId.HasValue && !restrUserIds.Contains(x.UserId.Value)) || isUserSa))
                    && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                    || (logFilter.CompanyId.HasValue && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value))
                    || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                                        log => log.Node).Skip(skip_count).Take(rowPerPage.Value);
                            }
                        case 4:
                            if (sortDirection.Value == 0)
                            {
                                return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                 .Where(
                                     x =>
                    ((logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                    && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                    && ((isUserCm && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)) || !isUserCm)
                    && ((!isUserSa && x.UserId.HasValue && !restrUserIds.Contains(x.UserId.Value)) || isUserSa))
                    && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                    || (logFilter.CompanyId.HasValue && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value))
                    || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderBy(
                log => log.Company != null ? string.Empty : log.Company.Name).Skip(skip_count).Take(rowPerPage.Value);
                            }
                            else
                            {
                                return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                .Where(
                                    x =>
                    ((logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                    && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                    && ((isUserCm && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)) || !isUserCm)
                    && ((!isUserSa && x.UserId.HasValue && !restrUserIds.Contains(x.UserId.Value)) || isUserSa))
                    && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                    || (logFilter.CompanyId.HasValue && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value))
                    || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                                        log => log.Company == null ? string.Empty : log.Company.Name).Skip(skip_count).Take(rowPerPage.Value);
                            }
                        case 5:
                            if (sortDirection.Value == 0)
                            {
                                return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                .Where(
                                    x =>
                    ((logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                    && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                    && ((isUserCm && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)) || !isUserCm)
                    && ((!isUserSa && x.UserId.HasValue && !restrUserIds.Contains(x.UserId.Value)) || isUserSa))
                    && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                    || (logFilter.CompanyId.HasValue && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value))
                    || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderBy(
               log => log.User == null ? string.Empty : (log.User.FirstName + " " + log.User.LastName)).Skip(skip_count).Take(rowPerPage.Value);
                            }
                            else
                            {
                                return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                .Where(
                                    x =>
                    ((logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                    && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                    && ((isUserCm && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)) || !isUserCm)
                    && ((!isUserSa && x.UserId.HasValue && !restrUserIds.Contains(x.UserId.Value)) || isUserSa))
                    && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                    || (logFilter.CompanyId.HasValue && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value))
                    || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                                        log => log.User == null ? string.Empty : (log.User.FirstName + " " + log.User.LastName)).Skip(skip_count).Take(rowPerPage.Value);
                            }
                        case 6:
                            if (sortDirection.Value == 0)
                            {
                                return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                .Where(
                                    x =>
                    ((logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                    && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                    && ((isUserCm && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)) || !isUserCm)
                    && ((!isUserSa && x.UserId.HasValue && !restrUserIds.Contains(x.UserId.Value)) || isUserSa))
                    && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                    || (logFilter.CompanyId.HasValue && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value))
                    || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderBy(
                                        log => log.Action).Skip(skip_count).Take(rowPerPage.Value);
                            }
                            else
                            {
                                return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                .Where(
                                    x =>
                    ((logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                    && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                    && ((isUserCm && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)) || !isUserCm)
                    && ((!isUserSa && x.UserId.HasValue && !restrUserIds.Contains(x.UserId.Value)) || isUserSa))
                    && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                    || (logFilter.CompanyId.HasValue && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value))
                    || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                                        log => log.Action).Skip(skip_count).Take(rowPerPage.Value);
                            }
                        default:
                            return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                .Where(
                                    x =>
                    ((logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                    && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                    && ((isUserCm && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)) || !isUserCm)
                    && ((!isUserSa && x.UserId.HasValue && !restrUserIds.Contains(x.UserId.Value)) || isUserSa))
                    && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                    || (logFilter.CompanyId.HasValue && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value))
                    || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                                        log => log.EventTime).Skip(skip_count).Take(rowPerPage.Value);
                    }
                }
                return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                .Where(
                                    x =>
                    ((logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                    && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                    && ((isUserCm && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value)) || !isUserCm)
                    && ((!isUserSa && x.UserId.HasValue && !restrUserIds.Contains(x.UserId.Value)) || isUserSa))
                    && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                    || (logFilter.CompanyId.HasValue && x.CompanyId.HasValue && allowedCompanyIds.Contains(x.CompanyId.Value))
                    || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                                        log => log.EventTime).Skip(skip_count).Take(rowPerPage.Value);

            }
            else
            {
                searchedRowsCount = (base.All() as ObjectSet<Log>).Count(x => (
                    (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                 && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                 && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                 && (allowedCompanyIds.Contains(x.CompanyId.Value)))
                 && (!restrUserIds.Contains(x.UserId.Value))
                 && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                 && (allowedCompanyIds.Contains(x.CompanyId.Value))
                 || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value))));

                int skip_count = 0;

                if (searchedRowsCount == 0)
                {
                    searchedRowsCount = (base.All() as ObjectSet<Log>).Count(x => (
                   (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                && (allowedCompanyIds.Contains(x.CompanyId.Value)))
                && (!restrUserIds.Contains(x.UserId.Value))
                && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                || (allowedCompanyIds.Contains(x.CompanyId.Value))
                || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value))));

                    if (rowPerPage.HasValue && navPage.HasValue)
                    {
                        if (searchedRowsCount > rowPerPage * navPage)
                        {
                            skip_count = rowPerPage.Value * navPage.Value;
                        }
                    }
                    else
                    {
                        rowPerPage = searchedRowsCount;
                    }
                    if (sortField.HasValue && sortDirection.HasValue)
                    {
                        switch (sortField)
                        {
                            case 1:
                                if (sortDirection.Value == 0)
                                {
                                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                    .Where(
                                        x => (
                        (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                     && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value)))
                     && (!restrUserIds.Contains(x.UserId.Value))
                     && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                     || (allowedCompanyIds.Contains(x.CompanyId.Value))
                     || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderBy(
                                            log => log.EventTime).Skip(skip_count).Take(rowPerPage.Value);
                                }
                                else return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                    .Where(
                                       x => (

                        (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                     && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value)))
                     && (!restrUserIds.Contains(x.UserId.Value))
                     && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                     || (allowedCompanyIds.Contains(x.CompanyId.Value))
                     || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                     log => log.EventTime).Skip(skip_count).Take(rowPerPage.Value);
                            case 2:
                                if (sortDirection.Value == 0)
                                {
                                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                    .Where(x => (
                        (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                     && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value)))
                     && (!restrUserIds.Contains(x.UserId.Value))
                     && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                     || (allowedCompanyIds.Contains(x.CompanyId.Value))
                     || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderBy(
                                            log => log.Building).Skip(skip_count).Take(rowPerPage.Value);
                                }
                                else
                                {
                                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                    .Where(
                                        x => (

                        (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                     && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value)))
                     && (!restrUserIds.Contains(x.UserId.Value))
                     && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                     || (allowedCompanyIds.Contains(x.CompanyId.Value))
                     || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                                            log => log.Building).Skip(skip_count).Take(rowPerPage.Value);
                                }
                            case 3:
                                if (sortDirection.Value == 0)
                                {
                                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                    .Where(
                                        x => (

                        (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                     && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value)))
                     && (!restrUserIds.Contains(x.UserId.Value))
                     && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                     || (allowedCompanyIds.Contains(x.CompanyId.Value))
                     || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderBy(
                                            log => log.Node).Skip(skip_count).Take(rowPerPage.Value);
                                }
                                else
                                {
                                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                    .Where(
                                        x => (

                        (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                     && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value)))
                     && (!restrUserIds.Contains(x.UserId.Value))
                     && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                     || (allowedCompanyIds.Contains(x.CompanyId.Value))
                     || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                                            log => log.Node).Skip(skip_count).Take(rowPerPage.Value);
                                }
                            case 4:
                                if (sortDirection.Value == 0)
                                {
                                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                     .Where(
                                         x => (

                        (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                     && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value)))
                     && (!restrUserIds.Contains(x.UserId.Value))
                     && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                     || (allowedCompanyIds.Contains(x.CompanyId.Value))
                     || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderBy(
                    log => log.Company != null ? string.Empty : log.Company.Name).Skip(skip_count).Take(rowPerPage.Value);
                                }
                                else
                                {
                                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                    .Where(
                                        x => (

                        (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                     && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value)))
                     && (!restrUserIds.Contains(x.UserId.Value))
                     && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                     || (allowedCompanyIds.Contains(x.CompanyId.Value))
                     || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                                            log => log.Company == null ? string.Empty : log.Company.Name).Skip(skip_count).Take(rowPerPage.Value);
                                }
                            case 5:
                                if (sortDirection.Value == 0)
                                {
                                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                    .Where(
                                        x => (

                        (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                     && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value)))
                     && (!restrUserIds.Contains(x.UserId.Value))
                     && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                     || (allowedCompanyIds.Contains(x.CompanyId.Value))
                     || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderBy(
                   log => log.User == null ? string.Empty : (log.User.FirstName + " " + log.User.LastName)).Skip(skip_count).Take(rowPerPage.Value);
                                }
                                else
                                {
                                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                    .Where(
                                        x => (

                        (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                     && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value)))
                     && (!restrUserIds.Contains(x.UserId.Value))
                     && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                     || (allowedCompanyIds.Contains(x.CompanyId.Value))
                     || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                                            log => log.User == null ? string.Empty : (log.User.FirstName + " " + log.User.LastName)).Skip(skip_count).Take(rowPerPage.Value);
                                }
                            case 6:
                                if (sortDirection.Value == 0)
                                {
                                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                    .Where(
                                        x => (

                        (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                     && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value)))
                     && (!restrUserIds.Contains(x.UserId.Value))
                     && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                     || (allowedCompanyIds.Contains(x.CompanyId.Value))
                     || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderBy(
                                            log => log.Action).Skip(skip_count).Take(rowPerPage.Value);
                                }
                                else
                                {
                                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                    .Where(
                                        x => (

                        (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                     && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value)))
                     && (!restrUserIds.Contains(x.UserId.Value))
                     && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                     || (allowedCompanyIds.Contains(x.CompanyId.Value))
                     || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                                            log => log.Action).Skip(skip_count).Take(rowPerPage.Value);
                                }
                            default:
                                return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                    .Where(
                                        x => (

                        (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                     && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value)))
                     && (!restrUserIds.Contains(x.UserId.Value))
                     && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                     || (allowedCompanyIds.Contains(x.CompanyId.Value))
                     || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                                            log => log.EventTime).Skip(skip_count).Take(rowPerPage.Value);
                        }
                    }
                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                    .Where(
                                        x => (

                        (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                     && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value)))
                     && (!restrUserIds.Contains(x.UserId.Value))
                     && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                     || (allowedCompanyIds.Contains(x.CompanyId.Value))
                     || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                                            log => log.EventTime).Skip(skip_count).Take(rowPerPage.Value);
                }
                else
                {
                    if (rowPerPage.HasValue && navPage.HasValue)
                    {
                        if (searchedRowsCount > rowPerPage * navPage)
                        {
                            skip_count = rowPerPage.Value * navPage.Value;
                        }
                    }
                    else
                    {
                        rowPerPage = searchedRowsCount;
                    }
                    if (sortField.HasValue && sortDirection.HasValue)
                    {
                        switch (sortField)
                        {
                            case 1:
                                if (sortDirection.Value == 0)
                                {
                                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                    .Where(
                                        x => (
                        (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                     && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value)))
                     && (!restrUserIds.Contains(x.UserId.Value))
                     && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value))
                     || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderBy(
                                            log => log.EventTime).Skip(skip_count).Take(rowPerPage.Value);
                                }
                                else return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                    .Where(
                                       x => (

                        (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                     && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value)))
                     && (!restrUserIds.Contains(x.UserId.Value))
                     && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value))
                     || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                     log => log.EventTime).Skip(skip_count).Take(rowPerPage.Value);
                            case 2:
                                if (sortDirection.Value == 0)
                                {
                                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                    .Where(x => (
                        (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                     && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value)))
                     && (!restrUserIds.Contains(x.UserId.Value))
                     && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value))
                     || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderBy(
                                            log => log.Building).Skip(skip_count).Take(rowPerPage.Value);
                                }
                                else
                                {
                                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                    .Where(
                                        x => (

                        (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                     && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value)))
                     && (!restrUserIds.Contains(x.UserId.Value))
                     && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value))
                     || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                                            log => log.Building).Skip(skip_count).Take(rowPerPage.Value);
                                }
                            case 3:
                                if (sortDirection.Value == 0)
                                {
                                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                    .Where(
                                        x => (

                        (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                     && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value)))
                     && (!restrUserIds.Contains(x.UserId.Value))
                     && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value))
                     || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderBy(
                                            log => log.Node).Skip(skip_count).Take(rowPerPage.Value);
                                }
                                else
                                {
                                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                    .Where(
                                        x => (

                        (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                     && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value)))
                     && (!restrUserIds.Contains(x.UserId.Value))
                     && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value))
                     || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                                            log => log.Node).Skip(skip_count).Take(rowPerPage.Value);
                                }
                            case 4:
                                if (sortDirection.Value == 0)
                                {
                                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                     .Where(
                                         x => (

                        (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                     && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value)))
                     && (!restrUserIds.Contains(x.UserId.Value))
                     && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value))
                     || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderBy(
                    log => log.Company != null ? string.Empty : log.Company.Name).Skip(skip_count).Take(rowPerPage.Value);
                                }
                                else
                                {
                                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                    .Where(
                                        x => (

                        (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                     && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value)))
                     && (!restrUserIds.Contains(x.UserId.Value))
                     && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value))
                     || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                                            log => log.Company == null ? string.Empty : log.Company.Name).Skip(skip_count).Take(rowPerPage.Value);
                                }
                            case 5:
                                if (sortDirection.Value == 0)
                                {
                                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                    .Where(
                                        x => (

                        (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                     && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value)))
                     && (!restrUserIds.Contains(x.UserId.Value))
                     && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value))
                     || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderBy(
                   log => log.User == null ? string.Empty : (log.User.FirstName + " " + log.User.LastName)).Skip(skip_count).Take(rowPerPage.Value);
                                }
                                else
                                {
                                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                    .Where(
                                        x => (
                        (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                     && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value)))
                     && (!restrUserIds.Contains(x.UserId.Value))
                     && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value))
                     || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                                            log => log.User == null ? string.Empty : (log.User.FirstName + " " + log.User.LastName)).Skip(skip_count).Take(rowPerPage.Value);
                                }
                            case 6:
                                if (sortDirection.Value == 0)
                                {
                                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                    .Where(
                                        x => (
                        (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                     && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value)))
                     && (!restrUserIds.Contains(x.UserId.Value))
                     && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value))
                     || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderBy(
                                            log => log.Action).Skip(skip_count).Take(rowPerPage.Value);
                                }
                                else
                                {
                                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                    .Where(
                                        x => (

                        (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                     && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value)))
                     && (!restrUserIds.Contains(x.UserId.Value))
                     && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value))
                     || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                                            log => log.Action).Skip(skip_count).Take(rowPerPage.Value);
                                }
                            default:
                                return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                    .Where(
                                        x => (
                        (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                     && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value)))
                     && (!restrUserIds.Contains(x.UserId.Value))
                     && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value))
                     || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                                            log => log.EventTime).Skip(skip_count).Take(rowPerPage.Value);
                        }
                    }
                    return (base.All() as ObjectSet<Log>).Include("User").Include("Company").Include("LogType")
                                    .Where(
                                        x => (
                        (logFilter.IsShowDefaultLog != true || x.LogType.IsDefault == logFilter.IsShowDefaultLog)
                     && (!logFilter.FromDate.HasValue || x.EventTime > logFilter.FromDate)
                     && (!logFilter.ToDate.HasValue || x.EventTime < logFilter.ToDate)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value)))
                     && (!restrUserIds.Contains(x.UserId.Value))
                     && (x.Building.Contains(logFilter.Building) || x.Node.Contains(logFilter.Node) || x.Action.Contains(logFilter.Activity)
                     && (allowedCompanyIds.Contains(x.CompanyId.Value))
                     || (x.UserId.HasValue && allowedUserIds.Contains(x.UserId.Value)))).OrderByDescending(
                                            log => log.EventTime).Skip(skip_count).Take(rowPerPage.Value);
                }
            }
        }

    }
}