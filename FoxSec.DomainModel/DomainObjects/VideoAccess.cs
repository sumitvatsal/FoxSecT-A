
using System;
using System.Collections.Generic;
using System.Linq;
using FoxSec.Common.Enums;

namespace FoxSec.DomainModel.DomainObjects
{
  public class VideoAccess: Entity
    {
  
        public virtual string CameraName { get; set; }
        public virtual int CompanyId { get; set; }                   

    }
}
