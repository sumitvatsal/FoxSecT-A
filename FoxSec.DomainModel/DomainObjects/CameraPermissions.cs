
using System;
using System.Collections.Generic;
using System.Linq;
using FoxSec.Common.Enums;

namespace FoxSec.DomainModel.DomainObjects
{
  public class CameraPermissions : Entity
    {
       
        public virtual int? CameraID { get; set; }
        public virtual string CameraName { get; set; }
        public virtual int? CompanyID { get; set; }
        public virtual bool? Access { get; set; }


    }

}

