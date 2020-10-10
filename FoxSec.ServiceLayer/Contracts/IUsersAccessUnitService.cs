using System;
using System.Collections.Generic;

namespace FoxSec.ServiceLayer.Contracts
{
    public interface IUsersAccessUnitService
    {
        void CreateCard(int? userId, int? typeId, int? companyId, int buildingId, string serial, string dk, string code, bool isFree, DateTime? from, DateTime? to,bool? IsMainUnit);
        bool CardIsBack(int id);
        bool EditCard(int Id, int? userId, int? typeId, int? companyId, int? buildingId, string serial, string dk, string code, bool isFree, DateTime? from, DateTime? to, string Comment, bool? IsMainUnit, bool? isActive=null);
        void Deactivate(int cardId, int? classificatorValueId);
        void Activate(int cardId, int? classificatorValueId);
        void Delete(int cardId);
        void SetFreeState(int cardId, int? classificatorValueId);
        void SetValidFrom(int cardId, DateTime date);
        void SetValidTo(int cardId, DateTime date);
        bool GiveCardBack(List<int> cardIds);
        bool EditCommentCard(int cardId, DateTime validTo, DateTime validFrom, string comment);
    }
}