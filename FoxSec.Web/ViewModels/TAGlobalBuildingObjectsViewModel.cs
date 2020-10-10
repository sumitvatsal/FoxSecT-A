using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FoxSec.DomainModel.DomainObjects;

namespace FoxSec.Web.ViewModels
{
    public class TAGlobalBuildingObjectsViewModel : ViewModelBase
    {
        public IEnumerable<BuildingObject> BuildingObjects { get; set; }

    }
    
}