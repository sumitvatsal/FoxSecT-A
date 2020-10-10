using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxSec.DomainModel.DomainObjects
{
    public class HolidayBuilding : Entity
    {
        public virtual int HoliDayId { get; set; }

        public virtual int BuildingId { get; set; }

        public virtual bool IsDeleted { get; set; }
    }
}
