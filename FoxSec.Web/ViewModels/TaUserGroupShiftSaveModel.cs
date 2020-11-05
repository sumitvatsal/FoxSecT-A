using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxSec.Web.ViewModels
{
    public class TaUserGroupShiftSaveModel
    {
        public string Name { get; set; }
        public int RepeatAfterWeeks { get; set; }
        public DateTime StartFromDate { get; set; }
        public int? CompanyId { get; set; }
        public bool IsDeleted { get; set; }
        public List<int> SelectedTaWeeks { get; set; }

    }
}