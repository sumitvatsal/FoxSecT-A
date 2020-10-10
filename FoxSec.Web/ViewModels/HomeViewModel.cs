using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Web.ViewModels
{
    public class HomeViewModel : ViewModelBase
	{
        public IEnumerable<UserAccessUnitType> CardTypes { get; set; }
        public IEnumerable<ClassificatorValue> ClassificatorValues { get; set; }
        public bool HRService { get; set; }
        public IEnumerable<FSINISetting> FSINISettings { get; set; }
        public bool menuflag { get; set; }
        public int TALicenseCount { get; set; }
        public int StaticId { get; set; }
        public bool DisableAddUser { get; set; }
    }
}