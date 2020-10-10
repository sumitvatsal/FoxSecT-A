using System;
using System.Collections.Generic;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Infrastructure.EF.Repositories.Interfaces
{
   public interface IVisitorRepository: IRepository<Visitor>
    {
        Visitor FindByUserId(int userId);
        bool IsSuperAdmin(int id);
        bool IsBuildingAdmin(int id);
        bool IsCompanyManager(int id);
        bool IsCommonUser(int id);
    }
}
