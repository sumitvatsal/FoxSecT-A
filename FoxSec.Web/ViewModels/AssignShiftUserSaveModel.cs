using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxSec.Web.ViewModels
{
    public class AssignShiftUserSaveModel
    {
        public AssignShiftUserSaveModel()
        {
            SelectedUsersId = new List<int>();
            SelectedUsersIsTA = new List<int>();
        }
        public int TaUserGroupShiftId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<int> SelectedUsersId { get; set; }
        public List<int> SelectedUsersIsTA { get; set; }
    }
}