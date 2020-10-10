using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoxSec.Web.ViewModels
{
    public class VisitorUsersDetail
    {
        public List<VisitorNewUserDetails> VisitorUserDetails { get; set; }
    }
    public class VisitorNewUserDetails
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string PermissionGroupName { get; set; }
       
    }
}